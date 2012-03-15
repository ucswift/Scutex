using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using WaveTech.Scutex.Generators.StaticKeyGeneratorSmall.Properties;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Exceptions;
using WaveTech.Scutex.Model.Interfaces.Generators;
using WaveTech.Scutex.Model.Interfaces.Providers;

namespace WaveTech.Scutex.Generators.StaticKeyGeneratorSmall
{
	internal class KeyGenerator : IKeyGenerator, ISmallKeyGenerator
	{
		#region Private Readonly Members
		private readonly ISymmetricEncryptionProvider symmetricEncryptionProvider;
		private readonly IAsymmetricEncryptionProvider asymmetricEncryptionProvider;
		private readonly IHashingProvider hashingProvider;

		internal static string licenseKeyTemplate = "xkx-xaaxxp-pppc";
		#endregion Private Readonly Members

		#region Constructor
		public KeyGenerator(ISymmetricEncryptionProvider symmetricEncryptionProvider,
			IAsymmetricEncryptionProvider asymmetricEncryptionProvider, IHashingProvider hashingProvider)
		{
			this.symmetricEncryptionProvider = symmetricEncryptionProvider;
			this.asymmetricEncryptionProvider = asymmetricEncryptionProvider;
			this.hashingProvider = hashingProvider;
		}
		#endregion Constructor

		#region Generate License Key
		public string GenerateLicenseKey(string rsaXmlString, LicenseBase scutexLicense, LicenseGenerationOptions generationOptions)
		{
			// Init all required variables for the process.
			Dictionary<int, LicensePlaceholder> placeholerLocations;
			List<LicensePlaceholder> licensePlaceholders = CreateLicensePlaceholders(scutexLicense, generationOptions);
			string licenseKey;
			char[] licenseKeyArray;

			// Setup the license key to work on
			licenseKey = licenseKeyTemplate.Replace("-", "");

			// Locate all the placeholder tokens in the license template
			placeholerLocations = FindAllPlaceholdersInTemplate(licenseKey, licensePlaceholders);

			// Verify that all the registered placeholders were located in the template.
			if (placeholerLocations.Count != licensePlaceholders.Count)
				throw new ScutexLicenseException(string.Format(Resources.ErrorMsg_PlaceholderCount,
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
						int data = int.Parse(licenseKeyArray[p.Key + i].ToString(), NumberStyles.HexNumber);
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

					char hash = hashingProvider.GetChecksumChar((licenseKey.Substring(0, p.Key)));

					licenseKey = licenseKey.Replace(token, hash.ToString());
				}
			}

			// Insert the seperators 
			List<int> seperatorLocations = FindAllSeperatorsInTemplate(licenseKeyTemplate);
			string finalKey = licenseKey;
			if (seperatorLocations.Count > 0)
			{
				int remaining = seperatorLocations.Count - 1;
				for (int i = 0; i < seperatorLocations.Count; i++)
				{
					finalKey = finalKey.Insert(seperatorLocations[i] - remaining, "-");
					remaining--;
				}
			}

			return finalKey;
		}
		#endregion Generate License Key

		#region Validate License Key
		public bool ValidateLicenseKey(string licenseKey, LicenseBase scutexLicense)
		{
			// Init all required variables for the process.
			Dictionary<int, LicensePlaceholder> placeholerLocations;
			List<LicensePlaceholder> licensePlaceholders = CreateLicensePlaceholders(scutexLicense, null);
			char[] licenseKeyArray;
			string decodedLicenseKey = licenseKey.Replace("-", "");

			// Locate all the placeholder tokens in the license template
			placeholerLocations = FindAllPlaceholdersInTemplate(licenseKeyTemplate.Replace("-", ""), licensePlaceholders);

			// STEP 1: Basic length checks
			if (licenseKey.Length != licenseKeyTemplate.Length)
				throw new ScutexLicenseException(Resources.ErrorMsg_VerifyLicenseKey);

			if (decodedLicenseKey.Length != licenseKeyTemplate.Replace("-", "").Length)
				throw new ScutexLicenseException(Resources.ErrorMsg_VerifyLicenseKey);

			// STEP 2: Compute and verify all the cheksums in the key
			foreach (var p in placeholerLocations)
			{
				if (p.Value.IsChecksum)
				{
					string originalHash = decodedLicenseKey.Substring(p.Key, p.Value.Length);
					int originalHashValue = 0;

					// Try and covert the value from originalHash into a Hex number, if this value has been changed (altered) the key is invalid as
					// it should always be a hex value.
					bool verifyHexOriginalHash = int.TryParse(originalHash, NumberStyles.HexNumber, CultureInfo.InvariantCulture,
											 out originalHashValue);

					// If this isn't a hex value throw the standard exception
					if (!verifyHexOriginalHash)
						throw new ScutexLicenseException(Resources.ErrorMsg_VerifyLicenseKey);

					// Compute the hash again using the rest of hte license key.
					int verifyHashValue = int.Parse(hashingProvider.GetChecksumChar(decodedLicenseKey.Substring(0, p.Key)).ToString(), NumberStyles.HexNumber);

					// Verify that the stored has is the same as the newly computed hash
					if (verifyHashValue != originalHashValue)
						throw new ScutexLicenseException(Resources.ErrorMsg_VerifyLicenseKey);

					if (!hashingProvider.ValidateChecksumChar(decodedLicenseKey))
						throw new ScutexLicenseException(Resources.ErrorMsg_VerifyLicenseKey);
				}
			}

			// STEP 3: DeObfuscate key license placeholders that are not cheksums (so they can be verified)
			foreach (var p in placeholerLocations)
			{
				if (!p.Value.IsChecksum)
				{
					licenseKeyArray = decodedLicenseKey.ToCharArray();
					for (int i = 0; i < p.Value.Length; i++)
					{
						char previousChar = licenseKeyArray[(p.Key) - 1];
						char deObfKey = KeyIntegerValueDeObfuscator(previousChar, licenseKeyArray[p.Key + i], p.Key + i);

						licenseKeyArray[p.Key + i] = deObfKey;
					}

					decodedLicenseKey = new string(licenseKeyArray);
				}
			}

			// STEP 4: Validate each non-checksum placeholder
			foreach (var p in placeholerLocations)
			{
				if (!p.Value.IsChecksum)
				{
					switch (p.Value.ValidationType)
					{
						case ValidationTypes.LicenseKeyType:
							int licenseKeyTypeValue = 0;
							bool licenseKeyTypeValueCheck = int.TryParse(decodedLicenseKey.Substring(p.Key, p.Value.Length), out licenseKeyTypeValue);

							// The LicenseKeyType value should be able to be convered to an int, else it is invalid
							if (!licenseKeyTypeValueCheck)
								throw new ScutexLicenseException(Resources.ErrorMsg_VerifyLicenseKey);

							LicenseKeyTypeFlag typeFlag;

							// It is possible that a LicenseKeyType has no supporting LicenseKeyTypeFlag (which means it was 
							// placeholded in the LicenseKeyType enum but is not operable). If this parse (cast) fails then
							// there was a valid LicenseKeyType enum value but no valid LicenseKeyTypeFlag value.
							try
							{
								typeFlag = (LicenseKeyTypeFlag)Enum.Parse(typeof(LicenseKeyTypeFlag), ((LicenseKeyTypes)licenseKeyTypeValue).ToString(), true);
							}
							catch (Exception)
							{
								throw new ScutexLicenseException(Resources.ErrorMsg_VerifyLicenseKey);
							}

							// If the LicenseSet does not support the key type supplied then throw an error
							if (!scutexLicense.LicenseSets.First().SupportedLicenseTypes.IsSet(typeFlag))
								throw new ScutexLicenseException(Resources.ErrorMsg_VerifyLicenseKey);

							break;
						case ValidationTypes.None:
							string keyValue = decodedLicenseKey.Substring(p.Key, p.Value.Length);

							if (keyValue != p.Value.Value)
								throw new ScutexLicenseException(Resources.ErrorMsg_VerifyLicenseKey);

							break;
						default:
							throw new ScutexLicenseException(Resources.ErrorMsg_VerifyLicenseKey);
					}
				}
			}

			// If we've gotten this far then it must be valid, right?
			return true;
		}
		#endregion Validate License Key

		#region Get License Key Data
		public KeyData GetLicenseKeyData(string licenseKey, LicenseBase scutexLicense)
		{
			// Init all required variables for the process.
			Dictionary<int, LicensePlaceholder> placeholerLocations;
			List<LicensePlaceholder> licensePlaceholders = CreateLicensePlaceholders(scutexLicense, null);
			char[] licenseKeyArray;
			string decodedLicenseKey = licenseKey.Replace("-", "");
			KeyData keyData = new KeyData();
			keyData.IsKeyValid = true;

			// Locate all the placeholder tokens in the license template
			placeholerLocations = FindAllPlaceholdersInTemplate(licenseKeyTemplate.Replace("-", ""), licensePlaceholders);

			bool isKeyValid = ValidateLicenseKey(licenseKey, scutexLicense);
			keyData.IsKeyValid = isKeyValid;

			if (isKeyValid)
			{
				foreach (var p in placeholerLocations)
				{
					if (!p.Value.IsChecksum)
					{
						licenseKeyArray = decodedLicenseKey.ToCharArray();
						for (int i = 0; i < p.Value.Length; i++)
						{
							char previousChar = licenseKeyArray[(p.Key) - 1];
							char deObfKey = KeyIntegerValueDeObfuscator(previousChar, licenseKeyArray[p.Key + i], p.Key + i);

							licenseKeyArray[p.Key + i] = deObfKey;
						}

						decodedLicenseKey = new string(licenseKeyArray);
					}
				}

				foreach (var p in placeholerLocations)
				{
					if (!p.Value.IsChecksum)
					{
						if (p.Value.Token == Char.Parse("k"))
						{
							int licenseKeyTypeValue = 0;
							bool licenseKeyTypeValueCheck = int.TryParse(decodedLicenseKey.Substring(p.Key, p.Value.Length), out licenseKeyTypeValue);

							if (licenseKeyTypeValueCheck)
								keyData.LicenseKeyType = (LicenseKeyTypes)licenseKeyTypeValue;
						}
						else if (p.Value.Token == Char.Parse("a"))
						{
							int prodValue = 0;
							bool prodValueCheck = int.TryParse(decodedLicenseKey.Substring(p.Key, p.Value.Length), out prodValue);

							if (prodValueCheck)
								keyData.ProductId = prodValue;
						}
					}
				}
			}

			return keyData;
		}
		#endregion Get License Key Data

		#region Get License Capability
		public LicenseCapability GetLicenseCapability()
		{
			LicenseCapability lc = new LicenseCapability();

			// Set the license capabilities using a bitwise OR to stack the values.
			lc.SupportedLicenseKeyTypes = LicenseKeyTypeFlag.SingleUser;
			lc.SupportedLicenseKeyTypes = lc.SupportedLicenseKeyTypes | LicenseKeyTypeFlag.MultiUser;
			lc.SupportedLicenseKeyTypes = lc.SupportedLicenseKeyTypes | LicenseKeyTypeFlag.Unlimited;
			lc.SupportedLicenseKeyTypes = lc.SupportedLicenseKeyTypes | LicenseKeyTypeFlag.Enterprise;

			lc.MaxLicenseKeysPerBatch = 50000;
			lc.MaxTotalLicenseKeys = 500000;

			return lc;
		}
		#endregion Get License Capability

		#region Internals
		internal List<LicensePlaceholder> CreateLicensePlaceholders(LicenseBase scutexLicense, LicenseGenerationOptions generationOptions)
		{
			List<LicensePlaceholder> Placeholders = new List<LicensePlaceholder>();

			if (generationOptions != null)
			{
				Placeholders.Add(new LicensePlaceholder
													{
														Length = 1,
														Token = Char.Parse("k"),
														Type = PlaceholderTypes.Number,
														Value = ((int)generationOptions.LicenseKeyType).ToString(),
														IsChecksum = false,
														ValidationType = ValidationTypes.LicenseKeyType
													});
			}
			else
			{
				Placeholders.Add(new LicensePlaceholder
													{
														Length = 1,
														Token = Char.Parse("k"),
														Type = PlaceholderTypes.Number,
														Value = "0",
														IsChecksum = false,
														ValidationType = ValidationTypes.LicenseKeyType
													});
			}

			Placeholders.Add(new LicensePlaceholder
			{
				Length = 2,
				Token = Char.Parse("a"),
				Type = PlaceholderTypes.Number,
				Value = scutexLicense.Product.GetFormattedProductId(2),
				IsChecksum = false,
				ValidationType = ValidationTypes.None
			});

			Placeholders.Add(new LicensePlaceholder
			{
				Length = 1,
				Token = Char.Parse("c"),
				Type = PlaceholderTypes.Number,
				Value = "",
				IsChecksum = true,
				ValidationType = ValidationTypes.None
			});

			string licProdChecksum = hashingProvider.Checksum16(scutexLicense.GetLicenseProductIdentifier()).ToString("X");

			if (licProdChecksum.Length < 4)
				licProdChecksum = licProdChecksum.PadLeft(4, char.Parse("0"));

			Placeholders.Add(new LicensePlaceholder
			{
				Length = 4,
				Token = Char.Parse("p"),
				Type = PlaceholderTypes.Number,
				Value = licProdChecksum,
				IsChecksum = false,
				ValidationType = ValidationTypes.None
			});

			return Placeholders;
		}

		internal static Dictionary<int, LicensePlaceholder> FindAllPlaceholdersInTemplate(string template, List<LicensePlaceholder> placeholders)
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

		internal static List<int> FindAllSeperatorsInTemplate(string template)
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


		internal static char KeyIntegerValueObfuscator(char previousValue, int data, int position)
		{
			int prevWeight = CharacterMap.ReverseMap[previousValue];
			double weightValue = GetWeightingModifer(prevWeight, position);

			double obfKey = data + weightValue;

			return CharacterMap.Map[(int)obfKey];
		}

		internal static char KeyIntegerValueDeObfuscator(char previousValue, char data, int position)
		{
			int prevWeight = CharacterMap.ReverseMap[previousValue];
			int obfKey = CharacterMap.ReverseMap[data];
			double weightValue = GetWeightingModifer(prevWeight, position);

			double origKey = obfKey - weightValue;

			// When DeObfuscating a Key value it -must- be positive or zero, as that is the only
			// data when originally obfuscating the key
			if (origKey < 0)
				throw new ScutexLicenseException(Resources.ErrorMsg_VerifyLicenseKey);

			return CharacterMap.Map[(int)origKey];
		}


		internal static char GetRandomCharacter()
		{
			byte[] randomBytes = new byte[1];
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			rng.GetBytes(randomBytes);
			int rand = Convert.ToInt32(randomBytes[0]);

			return CharacterMap.Map[rand % 36];
		}

		internal static double GetWeightingModifer(int weight1, int weight2)
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