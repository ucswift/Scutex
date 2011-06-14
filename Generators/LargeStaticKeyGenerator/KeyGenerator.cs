using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using WaveTech.Scutex.Generators.StaticKeyGeneratorLarge.Properties;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Exceptions;
using WaveTech.Scutex.Model.Interfaces.Generators;
using WaveTech.Scutex.Model.Interfaces.Providers;

namespace WaveTech.Scutex.Generators.StaticKeyGeneratorLarge
{
	public class KeyGenerator : IKeyGenerator
	{
		#region Private Readonly Members
		private readonly ISymmetricEncryptionProvider symmetricEncryptionProvider;
		private readonly IAsymmetricEncryptionProvider asymmetricEncryptionProvider;
		private readonly IHashingProvider hashingProvider;

		private static string licenseKeyTemplate = "xxkxv-xaaax-xxxxp-pppxl-xcccc";
		#endregion Private Readonly Members

		public KeyGenerator(ISymmetricEncryptionProvider symmetricEncryptionProvider,
			IAsymmetricEncryptionProvider asymmetricEncryptionProvider, IHashingProvider hashingProvider)
		{
			this.symmetricEncryptionProvider = symmetricEncryptionProvider;
			this.asymmetricEncryptionProvider = asymmetricEncryptionProvider;
			this.hashingProvider = hashingProvider;
		}

		public string GenerateLicenseKey(string rsaXmlString, LicenseBase scutexLicense, LicenseGenerationOptions generationOptions)
		{
			// Init all required variables for the process.
			Dictionary<int, LicensePlaceholder> placeholerLocations;
			List<LicensePlaceholder> licensePlaceholders = CreateLicensePlaceholders(scutexLicense);

			string licenseKey;
			char[] licenseKeyArray;

			// Setup the license key to work on
			licenseKey = licenseKeyTemplate.Replace("-", "");

			// Locate all the placeholder tokens in the license template
			placeholerLocations = FindAllPlaceholdersInTemplate(licenseKey, licensePlaceholders);

			// Verify that all the registered placeholders were located in the template.
			if (placeholerLocations.Count != licensePlaceholders.Count)
				throw new Exception(string.Format(Resources.ErrorMsg_PlaceholderCount,
																					licensePlaceholders.Count, placeholerLocations.Count));

			// Change all non-checksum placeholders to their actual values in the key
			foreach (var p in placeholerLocations)
			{
				if (!p.Value.IsChecksum)
				{
					string token = "";
					for (int i = 0; i < p.Value.Length; i++)
					{
						token = token + p.Value.Token;
					}

					licenseKey = licenseKey.Replace(token, p.Value.Value);
				}
			}

			// Compute and change the random license key placeholders
			licenseKeyArray = licenseKey.ToCharArray();
			for (int i = 0; i < licenseKeyArray.Length; i++)
			{
				if (licenseKeyArray[i] == char.Parse("x"))
				{
					licenseKeyArray[i] = GetRandomCharacter();
				}
			}
			licenseKey = new string(licenseKeyArray);

			// Obfuscate key license placeholders that are not cheksums
			foreach (var p in placeholerLocations)
			{
				if (!p.Value.IsChecksum)
				{
					licenseKeyArray = licenseKey.ToCharArray();
					for (int i = 0; i < p.Value.Length; i++)
					{
						char previousChar = licenseKeyArray[(p.Key) - 1];
						int data = int.Parse(licenseKeyArray[p.Key + i].ToString(), System.Globalization.NumberStyles.HexNumber);
						char obfKey = KeyIntegerValueObfuscator(previousChar, data, p.Key + i);

						licenseKeyArray[p.Key + i] = obfKey;
					}

					licenseKey = new string(licenseKeyArray);
				}
			}

			// Now compute and change all the cheksum placeholders in the key
			foreach (var p in placeholerLocations)
			{
				if (p.Value.IsChecksum)
				{
					string token = "";
					for (int i = 0; i < p.Value.Length; i++)
					{
						token = token + p.Value.Token;
					}

					string hash = hashingProvider.Checksum16(licenseKey.Substring(0, p.Key)).ToString("X");
					hash = hash.PadLeft(4, char.Parse("0"));

					licenseKey = licenseKey.Replace(token, hash);
				}
			}

			// Insert the seperators 
			List<int> seperatorLocations = FindAllSeperatorsInTemplate(licenseKeyTemplate);
			string finalKey = licenseKey;
			if (seperatorLocations.Count > 0)
			{
				foreach (int location in seperatorLocations)
				{
					finalKey = finalKey.Insert(location, "-");
				}
			}

			return finalKey;
		}

		public bool ValidateLicenseKey(string licenseKey, LicenseBase scutexLicense)
		{
			// Init all required variables for the process.
			Dictionary<int, LicensePlaceholder> placeholerLocations;
			List<LicensePlaceholder> licensePlaceholders = CreateLicensePlaceholders(scutexLicense);
			char[] licenseKeyArray;
			string decodedLicenseKey = licenseKey.Replace("-", "");

			// Locate all the placeholder tokens in the license template
			placeholerLocations = FindAllPlaceholdersInTemplate(decodedLicenseKey, licensePlaceholders);

			// Compute and verify all the cheksum placeholders in the key
			foreach (var p in placeholerLocations)
			{
				if (p.Value.IsChecksum)
				{
					string originalHash = decodedLicenseKey.Substring(p.Key, p.Value.Length);
					int originalHashValue = int.Parse(originalHash, System.Globalization.NumberStyles.HexNumber);

					int verifyHashValue = hashingProvider.Checksum16(decodedLicenseKey.Substring(0, p.Key));

					if (originalHashValue != verifyHashValue)
						throw new ScutexLicenseException(Resources.ErrorMsg_VerifyLicenseKey);
				}
			}

			// DeObfuscate key license placeholders that are not cheksums
			foreach (var p in placeholerLocations)
			{
				if (!p.Value.IsChecksum)
				{
					licenseKeyArray = licenseKey.ToCharArray();
					for (int i = 0; i < p.Value.Length; i++)
					{
						char previousChar = licenseKeyArray[(p.Key) - 1];
						char deObfKey = KeyIntegerValueDeObfuscator(previousChar, licenseKeyArray[p.Key + i], p.Key + i);

						licenseKeyArray[p.Key + i] = deObfKey;
					}

					licenseKey = new string(licenseKeyArray);
				}
			}

			return true;
		}

		public KeyData GetLicenseKeyData(string licenseKey, LicenseBase scutexLicense)
		{
			throw new NotImplementedException();
		}

		public LicenseCapability GetLicenseCapability()
		{
			throw new NotImplementedException();
		}

		public string GetKeyHash(string key)
		{
			throw new NotImplementedException();
		}

		#region Internals
		internal List<LicensePlaceholder> CreateLicensePlaceholders(LicenseBase license)
		{
			List<LicensePlaceholder> placeholders = new List<LicensePlaceholder>();

			placeholders.Add(new LicensePlaceholder
			{
				Length = 1,
				Token = Char.Parse("k"),
				Type = PlaceholderTypes.Number,
				Value = ((int)LicenseKeyTypes.Enterprise).ToString(),
				IsChecksum = false
			});

			placeholders.Add(new LicensePlaceholder
			{
				Length = 1,
				Token = Char.Parse("v"),
				Type = PlaceholderTypes.Number,
				Value = ((int)LicenseDataCheckTypes.Standard).ToString(),
				IsChecksum = false
			});

			placeholders.Add(new LicensePlaceholder
			{
				Length = 3,
				Token = Char.Parse("a"),
				Type = PlaceholderTypes.Number,
				Value = license.Product.GetFormattedProductId(3),
				IsChecksum = false
			});

			placeholders.Add(new LicensePlaceholder
			{
				Length = 4,
				Token = Char.Parse("p"),
				Type = PlaceholderTypes.Number,
				Value = hashingProvider.Checksum16(license.GetLicenseProductIdentifier()).ToString("X"),
				IsChecksum = false
			});

			placeholders.Add(new LicensePlaceholder
			{
				Length = 4,
				Token = Char.Parse("c"),
				Type = PlaceholderTypes.Number,
				Value = "",
				IsChecksum = true
			});

			placeholders.Add(new LicensePlaceholder
			{
				Length = 1,
				Token = Char.Parse("l"),
				Type = PlaceholderTypes.Number,
				Value = "F",
				IsChecksum = false
			});

			return placeholders;
		}

		internal Dictionary<int, LicensePlaceholder> FindAllPlaceholdersInTemplate(string template, List<LicensePlaceholder> placeholders)
		{
			Dictionary<int, LicensePlaceholder> locations = new Dictionary<int, LicensePlaceholder>();

			foreach (var placeholder in placeholders)
			{
				int location = template.IndexOf(placeholder.Token, 0);

				if (location > 0)
					locations.Add(location, placeholder);
			}

			return locations;
		}

		internal List<int> FindAllSeperatorsInTemplate(string template)
		{
			List<int> positions = new List<int>();

			// Work in reverse so when adding the seperators
			// back in the values are still valid.
			for (int i = template.Length - 1; i >= 0; i--)
			{
				if (template[i] == Char.Parse("-"))
				{
					positions.Add(i);
				}
			}

			return positions;
		}


		internal char KeyIntegerValueObfuscator(char previousValue, int data, int position)
		{
			int prevWeight = CharacterMap.ReverseMap[previousValue];
			double weightValue = GetWeightingModifer(prevWeight, position);

			double obfKey = data + weightValue;

			return CharacterMap.Map[(int)obfKey];
		}

		internal char KeyIntegerValueDeObfuscator(char previousValue, char data, int position)
		{
			int prevWeight = CharacterMap.ReverseMap[previousValue];
			int obfKey = CharacterMap.ReverseMap[data];
			double weightValue = GetWeightingModifer(prevWeight, position);

			double origKey = obfKey - weightValue;

			return CharacterMap.Map[(int)origKey];
		}


		internal char GetRandomCharacter()
		{
			byte[] randomBytes = new byte[1];
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			rng.GetBytes(randomBytes);
			int rand = Convert.ToInt32(randomBytes[0]);

			return CharacterMap.Map[rand % 35];
		}

		internal double GetWeightingModifer(int weight1, int weight2)
		{
			/* 
			 * The formula below contains no mathmatical signifiance that I
			 * am aware of. Instead it was design to create a small, yet varried
			 * numerical value based off of a pair of weights. If it's discovered
			 * that the formula below solves all the worlds problems or cures cancer
			 * or solves P = NP, I totally knew what I was doing.
			 */

			double c0;

			if (weight1 == 0)
				c0 = Math.PI;
			else
				c0 = weight1;

			double c1 = Math.Log((c0 * (weight2 * 75)));
			double c2 = Math.Sin(c1) + Math.Cos(c1);
			double c3 = Math.Abs(c2 * c1 + (weight2 / 2d));
			double c4 = Math.Round(c3, MidpointRounding.ToEven);

			if (c4 > 15)	// Less then half of the avialble alphabet characters
			{
				c4 = c4 - (double)Math.Truncate((weight2 / 2m));

				if (c4 > 15)
					c4 = 15;
			}

			if (c4 < 0)	// Can't go below zero
			{
				c4 = c4 + (double)Math.Truncate((weight2 / 2m));

				if (c4 < 0)
					c4 = 1;
			}

			// Not a bomb threat!
			return c4;
		}
		#endregion Internals
	}
}
