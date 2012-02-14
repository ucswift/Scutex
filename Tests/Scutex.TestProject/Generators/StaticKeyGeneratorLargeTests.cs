using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WaveTech.Scutex.Generators.StaticKeyGeneratorLarge;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Exceptions;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Services;
using WaveTech.Scutex.Providers.AsymmetricEncryptionProvider;
using WaveTech.Scutex.Providers.HashingProvider;
using WaveTech.Scutex.Providers.SymmetricEncryptionProvider;

namespace WaveTech.Scutex.UnitTests.Generators
{
	namespace StaticKeyGeneratorLargeTests
	{
		[TestClass]
		public class with_the_large_static_key_generator
		{
			protected IAsymmetricEncryptionProvider asymmetricEncryptionProvider;
			protected ISymmetricEncryptionProvider symmetricEncryptionProvider;
			protected IHashingProvider hashingProvider;

			protected IHardwareFingerprintService _hardwareFingerprintService;

			internal KeyGenerator largeKeyGenerator;
			protected ClientLicense license;
			protected List<LicenseGenerationOptions> generationOptions;

			protected List<LicensePlaceholder> placeholders;
			protected Dictionary<int, LicensePlaceholder> placeholdersInTemplate;
			protected string key;

			[TestInitialize]
			public void Before_each_test()
			{
				asymmetricEncryptionProvider = new AsymmetricEncryptionProvider();
				symmetricEncryptionProvider = new SymmetricEncryptionProvider();
				hashingProvider = new HashingProvider();

				largeKeyGenerator = new KeyGenerator(symmetricEncryptionProvider, asymmetricEncryptionProvider, hashingProvider);

				license = new ClientLicense();
				generationOptions = new List<LicenseGenerationOptions>();
				generationOptions.Add(new LicenseGenerationOptions());
				generationOptions.Add(new LicenseGenerationOptions());
				generationOptions.Add(new LicenseGenerationOptions());
				generationOptions.Add(new LicenseGenerationOptions());

				license.UniqueId = Guid.NewGuid();
				license.Product = new Product();
				license.Product.Name = "My Great Uber Cool Product, with new juice!";
				license.Product.ProductId = 1;

				license.LicenseSets = new NotifyList<LicenseSet>();
				license.LicenseSets.Add(new LicenseSet());
				license.LicenseSets.Add(new LicenseSet());
				license.LicenseSets.Add(new LicenseSet());
				license.LicenseSets.Add(new LicenseSet());

				license.LicenseSets[0].LicenseSetId = 1;
				license.LicenseSets[0].Name = "Standard Edition";
				license.LicenseSets[0].MaxUsers = 5;
				license.LicenseSets[0].SupportedLicenseTypes = LicenseKeyTypeFlag.SingleUser;
				license.LicenseSets[0].SupportedLicenseTypes |= LicenseKeyTypeFlag.MultiUser;
				license.LicenseSets[0].SupportedLicenseTypes |= LicenseKeyTypeFlag.Unlimited;

				license.LicenseSets[1].LicenseSetId = 2;
				license.LicenseSets[1].Name = "Professional Edition";
				license.LicenseSets[1].MaxUsers = 5;
				license.LicenseSets[1].SupportedLicenseTypes = LicenseKeyTypeFlag.SingleUser;
				license.LicenseSets[1].SupportedLicenseTypes |= LicenseKeyTypeFlag.MultiUser;
				license.LicenseSets[1].SupportedLicenseTypes |= LicenseKeyTypeFlag.Unlimited;

				license.LicenseSets[2].LicenseSetId = 2;
				license.LicenseSets[2].Name = "Enterprise Edition";
				license.LicenseSets[2].MaxUsers = 5;
				license.LicenseSets[2].SupportedLicenseTypes = LicenseKeyTypeFlag.SingleUser;
				license.LicenseSets[2].SupportedLicenseTypes |= LicenseKeyTypeFlag.MultiUser;
				license.LicenseSets[2].SupportedLicenseTypes |= LicenseKeyTypeFlag.Unlimited;
				license.LicenseSets[2].SupportedLicenseTypes |= LicenseKeyTypeFlag.Enterprise;

				license.LicenseSets[3].LicenseSetId = 2;
				license.LicenseSets[3].Name = "Upgrade Edition";
				license.LicenseSets[3].MaxUsers = 0;
				license.LicenseSets[3].IsUpgradeOnly = true;
				license.LicenseSets[3].SupportedLicenseTypes = LicenseKeyTypeFlag.SingleUser;

				generationOptions[0].LicenseKeyType = LicenseKeyTypes.Enterprise;
				generationOptions[0].LicenseSetId = 1;

				string productHash = hashingProvider.Checksum32(license.GetLicenseProductIdentifier()).ToString("X");

				placeholders = largeKeyGenerator.CreateLicensePlaceholders(license, generationOptions[0]);
				placeholdersInTemplate = KeyGenerator.FindAllPlaceholdersInTemplate(KeyGenerator.licenseKeyTemplate, placeholders);

				key = largeKeyGenerator.GenerateLicenseKey("TEST", license, generationOptions[0]);
			}
		}

		[TestClass]
		public class when_using_the_licese_placeholders_with_generationoptions : with_the_large_static_key_generator
		{
			[TestMethod]
			public void should_not_be_null()
			{
				Assert.IsNotNull(placeholders);
			}

			[TestMethod]
			public void should_contain_seven_items()
			{
				Assert.AreEqual(7, placeholders.Count);
			}

			[TestMethod]
			public void should_contain_valid_K_placeholder_item()
			{
				var placeholder = placeholders.Where(x => x.Token == Char.Parse("k"));

				Assert.AreEqual(1, placeholder.Count());
				Assert.AreEqual(((int)generationOptions[0].LicenseKeyType).ToString(), placeholder.First().Value);
			}

			[TestMethod]
			public void should_contain_valid_A_placeholder_item()
			{
				var placeholder = placeholders.Where(x => x.Token == Char.Parse("a"));

				Assert.AreEqual(1, placeholder.Count());
				Assert.AreEqual(license.Product.GetFormattedProductId(2), placeholder.First().Value);
			}

			[TestMethod]
			public void should_contain_valid_C_placeholder_item()
			{
				var placeholder = placeholders.Where(x => x.Token == Char.Parse("c"));

				Assert.AreEqual(1, placeholder.Count());
				Assert.AreEqual("", placeholder.First().Value);
			}

			[TestMethod]
			public void should_contain_valid_P_placeholder_item()
			{
				var placeholder = placeholders.Where(x => x.Token == Char.Parse("p"));

				Assert.AreEqual(1, placeholder.Count());
				Assert.AreEqual(hashingProvider.Checksum16(license.GetLicenseProductIdentifier()).ToString("X").PadLeft(4, char.Parse("0")),
												placeholder.First().Value);
			}
		}

		[TestClass]
		public class when_using_find_all_placeholders_in_template : with_the_large_static_key_generator
		{
			[TestMethod]
			public void should_not_be_null()
			{
				Assert.IsNotNull(placeholdersInTemplate);
			}

			[TestMethod]
			public void should_contain_seven_items()
			{
				Assert.AreEqual(7, placeholdersInTemplate.Count);
			}
		}

		[TestClass]
		public class when_using_find_all_seperators_in_template : with_the_large_static_key_generator
		{
			[TestMethod]
			public void should_not_be_null()
			{
				Assert.IsNotNull(KeyGenerator.FindAllSeperatorsInTemplate(KeyGenerator.licenseKeyTemplate));
			}

			[TestMethod]
			public void should_contain_four_items()
			{
				Assert.AreEqual(4, KeyGenerator.FindAllSeperatorsInTemplate(KeyGenerator.licenseKeyTemplate).Count);
			}
		}

		[TestClass]
		public class when_using_get_random_character : with_the_large_static_key_generator
		{
			[TestMethod]
			public void should_not_be_null()
			{
				Assert.IsNotNull(KeyGenerator.GetRandomCharacter());
			}
		}

		[TestClass]
		public class when_using_get_weighting_modifer : with_the_large_static_key_generator
		{
			[TestMethod]
			public void should_be_11_with_weights_of_3_and_4()
			{
				Assert.AreEqual(11.0, KeyGenerator.GetWeightingModifer(3, 4));
			}

			[TestMethod]
			public void should_be_9_with_weights_of_31_and_8()
			{
				Assert.AreEqual(9.0, KeyGenerator.GetWeightingModifer(31, 8));
			}

			[TestMethod]
			public void should_be_5_with_weights_of_29_and_19()
			{
				Assert.AreEqual(5.0, KeyGenerator.GetWeightingModifer(25, 19));
			}

			[TestMethod]
			public void should_be_able_to_handle_ramdom_numbers()
			{
				byte[] randomNumber = new byte[1];
				RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();

				for (int i = 0; i < 100000; i++)
				{
					Gen.GetBytes(randomNumber);
					int rand1 = Convert.ToInt32(randomNumber[0]);

					randomNumber = new byte[1];
					Gen.GetBytes(randomNumber);
					int rand2 = Convert.ToInt32(randomNumber[0]);

					rand1 = rand1 % 35;
					rand2 = rand2 % 25;

					double value1 = KeyGenerator.GetWeightingModifer(rand1, rand2);
					double value2 = KeyGenerator.GetWeightingModifer(rand1, rand2);

					Assert.AreEqual(value1, value2);
				}
			}
		}

		[TestClass]
		public class when_using_the_key_value_obfuscator : with_the_large_static_key_generator
		{
			[TestMethod]
			public void should_be_6_with_char_of_R_and_weights_of_5_and_3()
			{
				Assert.AreEqual(char.Parse("6"), KeyGenerator.KeyIntegerValueObfuscator(char.Parse("R"), 5, 3));
			}

			[TestMethod]
			public void should_be_G_with_char_of_R_and_weights_of_15_and_3()
			{
				Assert.AreEqual(char.Parse("G"), KeyGenerator.KeyIntegerValueObfuscator(char.Parse("R"), 15, 3));
			}

			[TestMethod]
			public void should_be_G_with_char_of_C_and_weights_of_4_and_19()
			{
				Assert.AreEqual(char.Parse("7"), KeyGenerator.KeyIntegerValueObfuscator(char.Parse("C"), 4, 19));
			}
		}

		[TestClass]
		public class when_using_the_key_value_deobfuscator : with_the_large_static_key_generator
		{
			[TestMethod]
			public void should_be_5_with_char_of_R_and_weights_of_6_and_3()
			{
				Assert.AreEqual(char.Parse("5"), KeyGenerator.KeyIntegerValueDeObfuscator(char.Parse("R"), char.Parse("6"), 3));
			}

			[TestMethod]
			public void should_be_F_with_char_of_R_and_weights_of_G_and_3()
			{
				Assert.AreEqual(CharacterMap.Map[15], KeyGenerator.KeyIntegerValueDeObfuscator(char.Parse("R"), char.Parse("G"), 3));
			}

			[TestMethod]
			public void should_be_4_with_char_of_C_and_weights_of_7_and_19()
			{
				Assert.AreEqual(char.Parse("4"), KeyGenerator.KeyIntegerValueDeObfuscator(char.Parse("C"), char.Parse("7"), 19));
			}
		}

		[TestClass]
		public class when_generating_a_single_key : with_the_large_static_key_generator
		{
			[TestMethod]
			public void should_not_be_null()
			{
				Assert.IsNotNull(key);
			}

			[TestMethod]
			public void should_be_29_chars_long()
			{
				if (key.Length != 29)
					Console.WriteLine(string.Format("Large Static Key does not equal 29 characters long: {0}", key));

				Assert.AreEqual(29, key.Length);
			}

			[TestMethod]
			public void should_be_25_chars_without_dashes()
			{
				Console.WriteLine(key);
				string key1 = key.Replace("-", "");
				Console.WriteLine(key1);

				Assert.AreEqual(25, key1.Length);
			}
		}

		[TestClass]
		public class when_validating_a_single_key : with_the_large_static_key_generator
		{
			[TestMethod]
			[ExpectedException(typeof(ScutexLicenseException))]
			public void should_throw_exception_when_invalid()
			{
				char[] keyArray = key.ToCharArray();
				keyArray[3] = Char.Parse("0");

				string newKey = new string(keyArray);

				largeKeyGenerator.ValidateLicenseKey(newKey, license);
			}

			[TestMethod]
			public void should_be_98_precent_accurate_when_validating_200000_bad_licenses()
			{
				const int count = 200000;

				int errorCount = 0;
				byte[] randomNumber = new byte[1];
				RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();

				for (int i = 0; i < count; i++)
				{
					char[] keyArray = key.ToCharArray();

					Gen.GetBytes(randomNumber);
					int randLocation = Convert.ToInt32(randomNumber[0]);
					randLocation = randLocation % keyArray.Count();

					randomNumber = new byte[1];
					Gen.GetBytes(randomNumber);
					int randValue = Convert.ToInt32(randomNumber[0]);
					randValue = randValue % 35;

					keyArray[randLocation] = CharacterMap.Map[randValue];

					string newKey = new string(keyArray);

					if (newKey == key)
					{
						errorCount++;
						continue;
					}

					try
					{
						largeKeyGenerator.ValidateLicenseKey(newKey, license);
					}
					catch (ScutexLicenseException)
					{
						errorCount++;
					}
				}

				double accuracy = (double)errorCount / count;

				Console.WriteLine(string.Format("Actual LargeKeyGenerator validation accuracy: {0} with {1} validation failures out of {2}", accuracy, errorCount, count));
				Assert.IsTrue(accuracy > .98);
			}

			[TestMethod]
			public void should_work_when_using_valid_key()
			{
				bool test = largeKeyGenerator.ValidateLicenseKey(key, license);

				Assert.IsTrue(test);
			}
		}

		[TestClass]
		public class when_hardware_locking_a_key : with_the_large_static_key_generator
		{
			[TestMethod]
			public void should_be_able_to_lock_a_key_to_a_fingerprint()
			{
				
			}
		}
	}
}