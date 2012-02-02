using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

		internal static string licenseKeyTemplate = "xxkxv-xxaax-xsxxp-pppxl-xcccc";
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
					if (p.Value.Type == PlaceholderTypes.Number)
					{
						//Console.WriteLine(p.Value.Token);
						//Console.WriteLine("-----------------");

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
					else if (p.Value.Type == PlaceholderTypes.String)
					{
						licenseKeyArray = licenseKey.ToCharArray();

						for (int i = 0; i < p.Value.Length; i++)
						{
							char previousChar = licenseKeyArray[(p.Key) - 1];

							int modData = CharacterMap.ReverseMap[licenseKeyArray[p.Key + i]];
							//Console.WriteLine(string.Format("Char: {0}", licenseKeyArray[p.Key + i]));
								
							char obfKey = KeyIntegerValueObfuscator(previousChar, modData, p.Key + i);

							licenseKeyArray[p.Key + i] = obfKey;
						}

						licenseKey = new string(licenseKeyArray);
					}
				}
			}

			// Now compute and change all the checksum placeholders in the key
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
				int remaining = seperatorLocations.Count - 1;
				for (int i = 0; i < seperatorLocations.Count; i++)
				{
					finalKey = finalKey.Insert(seperatorLocations[i] - remaining, "-");
					remaining--;
				}
			}

			return finalKey;
		}

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

			// STEP 2: Compute and verify all the checksum placeholders in the key
			foreach (var p in placeholerLocations)
			{
				if (p.Value.IsChecksum)
				{
					string originalHash;
					int originalHashValue;
					int verifyHashValue;

					try
					{
						originalHash = decodedLicenseKey.Substring(p.Key, p.Value.Length);
						originalHashValue = int.Parse(originalHash, System.Globalization.NumberStyles.HexNumber);
						verifyHashValue = hashingProvider.Checksum16(decodedLicenseKey.Substring(0, p.Key));
					}
					catch
					{
						throw new ScutexLicenseException(Resources.ErrorMsg_VerifyLicenseKey);
					}

					if (originalHashValue != verifyHashValue)
						throw new ScutexLicenseException(Resources.ErrorMsg_VerifyLicenseKey);
				}
			}

			// STEP 3: DeObfuscate key license placeholders that are not checksums
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

							// The LicenseKeyType value should be able to be converted to an int, else it is invalid
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

							LicenseSet ls = null;
							try
							{
								var licSetPH = placeholerLocations.Where(x => x.Value.ValidationType == ValidationTypes.LicenseSet).SingleOrDefault();
								int licenseSetIdValue1 = int.Parse(decodedLicenseKey.Substring(licSetPH.Key, licSetPH.Value.Length));

								ls = scutexLicense.LicenseSets.Where(x => x.LicenseSetId == licenseSetIdValue1).SingleOrDefault();
							}
							catch 
							{
								throw new ScutexLicenseException(Resources.ErrorMsg_VerifyLicenseKey);
							}

							if (ls == null)
								throw new ScutexLicenseException(Resources.ErrorMsg_VerifyLicenseKey);

							// If the LicenseSet does not support the key type supplied then throw an error
							if (!ls.SupportedLicenseTypes.IsSet(typeFlag))
								throw new ScutexLicenseException(Resources.ErrorMsg_VerifyLicenseKey);

							break;
						case ValidationTypes.LicenseSet:
							int licenseSetIdValue = 0;
							bool licenseSetIdValueCheck = int.TryParse(decodedLicenseKey.Substring(p.Key, p.Value.Length), out licenseSetIdValue);

							// The LicenseSetId value should be able to be converted to an int, else it is invalid
							if (!licenseSetIdValueCheck)
								throw new ScutexLicenseException(Resources.ErrorMsg_VerifyLicenseKey);

							LicenseSet ls1 = null;
							try
							{
								ls1 = scutexLicense.LicenseSets.Where(x => x.LicenseSetId == licenseSetIdValue).SingleOrDefault();
							}
							catch
							{
								throw new ScutexLicenseException(Resources.ErrorMsg_VerifyLicenseKey);
							}

							if (ls1 == null)
								throw new ScutexLicenseException(Resources.ErrorMsg_VerifyLicenseKey);

							break;
						case ValidationTypes.Product:
							int productIdValue = 0;
							bool productIdValueCheck = int.TryParse(decodedLicenseKey.Substring(p.Key, p.Value.Length), out productIdValue);

							// The ProductId value should be able to be converted to an int, else it is invalid
							if (!productIdValueCheck)
								throw new ScutexLicenseException(Resources.ErrorMsg_VerifyLicenseKey);

							if (scutexLicense.Product.ProductId != productIdValue)
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

			return true;
		}

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
						else if (p.Value.Token == Char.Parse("s"))
						{
							int licenseSetValue = 0;
							bool licenseSetValueCheck = int.TryParse(decodedLicenseKey.Substring(p.Key, p.Value.Length), out licenseSetValue);

							if (licenseSetValueCheck)
								keyData.LicenseSetId = licenseSetValue;
						}
					}
				}
			}

			return keyData;
		}

		public LicenseCapability GetLicenseCapability()
		{
			LicenseCapability lc = new LicenseCapability();

			// Set the license capabilities using a bitwise OR to stack the values.
			lc.SupportedLicenseKeyTypes = LicenseKeyTypeFlag.SingleUser;
			lc.SupportedLicenseKeyTypes = lc.SupportedLicenseKeyTypes | LicenseKeyTypeFlag.MultiUser;
			lc.SupportedLicenseKeyTypes = lc.SupportedLicenseKeyTypes | LicenseKeyTypeFlag.Unlimited;
			lc.SupportedLicenseKeyTypes = lc.SupportedLicenseKeyTypes | LicenseKeyTypeFlag.Enterprise;
			lc.SupportedLicenseKeyTypes = lc.SupportedLicenseKeyTypes | LicenseKeyTypeFlag.HardwareLock;

			lc.MaxLicenseKeysPerBatch = 500000;
			lc.MaxTotalLicenseKeys = 10000000;

			return lc;
		}

		#region Internals
		internal List<LicensePlaceholder> CreateLicensePlaceholders(LicenseBase scutexLicense, LicenseGenerationOptions generationOptions)
		{
			List<LicensePlaceholder> placeholders = new List<LicensePlaceholder>();

			if (generationOptions != null)
			{
				placeholders.Add(new LicensePlaceholder
				                 	{
				                 		Length = 1,
				                 		Token = Char.Parse("k"),
				                 		Type = PlaceholderTypes.Number,
				                 		Value = ((int) generationOptions.LicenseKeyType).ToString(),
				                 		IsChecksum = false,
				                 		ValidationType = ValidationTypes.LicenseKeyType
				                 	});

				placeholders.Add(new LicensePlaceholder
				                 	{
				                 		Length = 1,
				                 		Token = Char.Parse("s"),
				                 		Type = PlaceholderTypes.Number,
				                 		Value = generationOptions.LicenseSetId.ToString(),
				                 		IsChecksum = false,
				                 		ValidationType = ValidationTypes.LicenseSet
				                 	});
			}
			else
			{
				placeholders.Add(new LicensePlaceholder
				{
					Length = 1,
					Token = Char.Parse("k"),
					Type = PlaceholderTypes.Number,
					Value = "0",
					IsChecksum = false,
					ValidationType = ValidationTypes.LicenseKeyType
				});

				placeholders.Add(new LicensePlaceholder
				{
					Length = 1,
					Token = Char.Parse("s"),
					Type = PlaceholderTypes.Number,
					Value = "0",
					IsChecksum = false,
					ValidationType = ValidationTypes.LicenseSet
				});
			}

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
				Length = 2,
				Token = Char.Parse("a"),
				Type = PlaceholderTypes.Number,
				Value = scutexLicense.Product.GetFormattedProductId(2),
				IsChecksum = false,
				ValidationType = ValidationTypes.Product
			});

			string payload = hashingProvider.Checksum16(scutexLicense.GetLicenseProductIdentifier()).ToString("X");
			payload = payload.PadLeft(4, char.Parse("0"));

			placeholders.Add(new LicensePlaceholder
			{
				Length = 4,
				Token = Char.Parse("p"),
				Type = PlaceholderTypes.String,
				Value = payload,
				IsChecksum = false,
				ValidationType = ValidationTypes.None
			});

			placeholders.Add(new LicensePlaceholder
			{
				Length = 4,
				Token = Char.Parse("c"),
				Type = PlaceholderTypes.Number,
				Value = "",
				IsChecksum = true,
				ValidationType = ValidationTypes.None
			});

			placeholders.Add(new LicensePlaceholder
			{
				Length = 1,
				Token = Char.Parse("l"),
				Type = PlaceholderTypes.String,
				Value = "F",
				IsChecksum = false
			});

			//placeholders.Add(new LicensePlaceholder
			//{
			//  Length = 1,
			//  Token = Char.Parse("e"),
			//  Type = PlaceholderTypes.String,
			//  Value = "0",
			//  IsChecksum = false
			//});

			return placeholders;
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
