using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using WaveTech.Scutex.Generators.StaticKeyGeneratorLarge;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Exceptions;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Providers.AsymmetricEncryptionProvider;
using WaveTech.Scutex.Providers.HashingProvider;
using WaveTech.Scutex.Providers.SymmetricEncryptionProvider;

namespace WaveTech.Scutex.UnitTests.Generators
{
	//namespace StaticKeyGeneratorLargeTests
	//{
	//      public class with_the_small_static_key_generator : FixtureBase
	//  {
	//    protected IAsymmetricEncryptionProvider asymmetricEncryptionProvider;
	//    protected ISymmetricEncryptionProvider symmetricEncryptionProvider;
	//    protected IHashingProvider hashingProvider;

	//    internal KeyGenerator smallKeyGenerator;
	//    protected ClientLicense license;
	//    protected LicenseGenerationOptions generationOptions;

	//    protected List<LicensePlaceholder> placeholders;
	//    protected Dictionary<int, LicensePlaceholder> placeholdersInTemplate;
	//    protected string key;

	//    protected override void Before_each_test()
	//    {
	//      base.Before_each_test();

	//      asymmetricEncryptionProvider = new AsymmetricEncryptionProvider();
	//      symmetricEncryptionProvider = new SymmetricEncryptionProvider();
	//      hashingProvider = new HashingProvider();

	//      smallKeyGenerator = new KeyGenerator(symmetricEncryptionProvider, asymmetricEncryptionProvider, hashingProvider);

	//      license = new ClientLicense();
	//      generationOptions = new LicenseGenerationOptions();

	//      license.UniqueId = Guid.NewGuid();
	//      license.Product = new Product();
	//      license.Product.Name = "My Great Uber Cool Product, with new juice!";
	//      license.Product.ProductId = 1;

	//      license.LicenseSets = new NotifyList<LicenseSet>();
	//      license.LicenseSets.Add(new LicenseSet());

	//      license.LicenseSets.First().SupportedLicenseTypes = LicenseKeyTypeFlag.SingleUser;
	//      license.LicenseSets.First().SupportedLicenseTypes |= LicenseKeyTypeFlag.Enterprise;
	//      license.LicenseSets.First().SupportedLicenseTypes |= LicenseKeyTypeFlag.Unlimited;

	//      generationOptions.LicenseKeyType = LicenseKeyTypes.Enterprise;

	//      string productHash = hashingProvider.Checksum32(license.GetLicenseProductIdentifier()).ToString("X");

	//      placeholders = smallKeyGenerator.CreateLicensePlaceholders(license, generationOptions);
	//      placeholdersInTemplate = KeyGenerator.FindAllPlaceholdersInTemplate(KeyGenerator.licenseKeyTemplate, placeholders);

	//      key = smallKeyGenerator.GenerateLicenseKey("TEST", license, generationOptions);
	//    }
	//  }

	//  [TestClass]
	//  public class when_using_the_licese_placeholders_with_generationoptions : with_the_small_static_key_generator
	//  {
	//    [TestMethod]
	//    public void should_not_be_null()
	//    {
	//      Assert.IsNotNull(placeholders);
	//    }

	//    [TestMethod]
	//    public void should_contain_four_items()
	//    {
	//      Assert.AreEqual(4, placeholders.Count);
	//    }

	//    [TestMethod]
	//    public void should_contain_valid_K_placeholder_item()
	//    {
	//      var placeholder = placeholders.Where(x => x.Token == Char.Parse("k"));

	//      Assert.AreEqual(1, placeholder.Count());
	//      Assert.AreEqual(((int)generationOptions.LicenseKeyType).ToString(), placeholder.First().Value);
	//    }

	//    [TestMethod]
	//    public void should_contain_valid_A_placeholder_item()
	//    {
	//      var placeholder = placeholders.Where(x => x.Token == Char.Parse("a"));

	//      Assert.AreEqual(1, placeholder.Count());
	//      Assert.AreEqual(license.Product.GetFormattedProductId(2), placeholder.First().Value);
	//    }

	//    [TestMethod]
	//    public void should_contain_valid_C_placeholder_item()
	//    {
	//      var placeholder = placeholders.Where(x => x.Token == Char.Parse("c"));

	//      Assert.AreEqual(1, placeholder.Count());
	//      Assert.AreEqual("", placeholder.First().Value);
	//    }

	//    [TestMethod]
	//    public void should_contain_valid_P_placeholder_item()
	//    {
	//      var placeholder = placeholders.Where(x => x.Token == Char.Parse("p"));

	//      Assert.AreEqual(1, placeholder.Count());
	//      Assert.AreEqual(hashingProvider.Checksum16(license.GetLicenseProductIdentifier()).ToString("X").PadLeft(4, char.Parse("0")),
	//                      placeholder.First().Value);
	//    }
	//  }

	//  [TestClass]
	//  public class when_using_find_all_placeholders_in_template : with_the_small_static_key_generator
	//  {
	//    [TestMethod]
	//    public void should_not_be_null()
	//    {
	//      Assert.IsNotNull(placeholdersInTemplate);
	//    }

	//    [TestMethod]
	//    public void should_contain_four_items()
	//    {
	//      Assert.AreEqual(4, placeholdersInTemplate.Count);
	//    }
	//  }

	//  [TestClass]
	//  public class when_using_find_all_seperators_in_template : with_the_small_static_key_generator
	//  {
	//    [TestMethod]
	//    public void should_not_be_null()
	//    {
	//      Assert.IsNotNull(KeyGenerator.FindAllSeperatorsInTemplate(KeyGenerator.licenseKeyTemplate));
	//    }

	//    [TestMethod]
	//    public void should_contain_two_items()
	//    {
	//      Assert.AreEqual(2, KeyGenerator.FindAllSeperatorsInTemplate(KeyGenerator.licenseKeyTemplate).Count);
	//    }
	//  }

	//  [TestClass]
	//  public class when_using_get_random_character : with_the_small_static_key_generator
	//  {
	//    [TestMethod]
	//    public void should_not_be_null()
	//    {
	//      Assert.IsNotNull(KeyGenerator.GetRandomCharacter());
	//    }
	//  }

	//  [TestClass]
	//  public class when_using_get_weighting_modifer : with_the_small_static_key_generator
	//  {
	//    [TestMethod]
	//    public void should_be_11_with_weights_of_3_and_4()
	//    {
	//      Assert.AreEqual(11.0, KeyGenerator.GetWeightingModifer(3, 4));
	//    }

	//    [TestMethod]
	//    public void should_be_9_with_weights_of_31_and_8()
	//    {
	//      Assert.AreEqual(9.0, KeyGenerator.GetWeightingModifer(31, 8));
	//    }

	//    [TestMethod]
	//    public void should_be_5_with_weights_of_29_and_19()
	//    {
	//      Assert.AreEqual(5.0, KeyGenerator.GetWeightingModifer(25, 19));
	//    }

	//    [TestMethod]
	//    public void should_be_able_to_handle_ramdom_numbers()
	//    {
	//      byte[] randomNumber = new byte[1];
	//      RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();

	//      for (int i = 0; i < 100000; i++)
	//      {
	//        Gen.GetBytes(randomNumber);
	//        int rand1 = Convert.ToInt32(randomNumber[0]);

	//        randomNumber = new byte[1];
	//        Gen.GetBytes(randomNumber);
	//        int rand2 = Convert.ToInt32(randomNumber[0]);

	//        rand1 = rand1 % 35;
	//        rand2 = rand2 % 25;

	//        double value1 = KeyGenerator.GetWeightingModifer(rand1, rand2);
	//        double value2 = KeyGenerator.GetWeightingModifer(rand1, rand2);

	//        Assert.AreEqual(value1, value2);
	//      }
	//    }
	//  }

	//  [TestClass]
	//  public class when_using_the_key_value_obfuscator : with_the_small_static_key_generator
	//  {
	//    [TestMethod]
	//    public void should_be_6_with_char_of_R_and_weights_of_5_and_3()
	//    {
	//      Assert.AreEqual(char.Parse("6"), KeyGenerator.KeyIntegerValueObfuscator(char.Parse("R"), 5, 3));
	//    }

	//    [TestMethod]
	//    public void should_be_G_with_char_of_R_and_weights_of_15_and_3()
	//    {
	//      Assert.AreEqual(char.Parse("G"), KeyGenerator.KeyIntegerValueObfuscator(char.Parse("R"), 15, 3));
	//    }

	//    [TestMethod]
	//    public void should_be_G_with_char_of_C_and_weights_of_4_and_19()
	//    {
	//      Assert.AreEqual(char.Parse("7"), KeyGenerator.KeyIntegerValueObfuscator(char.Parse("C"), 4, 19));
	//    }
	//  }

	//  [TestClass]
	//  public class when_using_the_key_value_deobfuscator : with_the_small_static_key_generator
	//  {
	//    [TestMethod]
	//    public void should_be_5_with_char_of_R_and_weights_of_6_and_3()
	//    {
	//      Assert.AreEqual(char.Parse("5"), KeyGenerator.KeyIntegerValueDeObfuscator(char.Parse("R"), char.Parse("6"), 3));
	//    }

	//    [TestMethod]
	//    public void should_be_F_with_char_of_R_and_weights_of_G_and_3()
	//    {
	//      Assert.AreEqual(CharacterMap.Map[15], KeyGenerator.KeyIntegerValueDeObfuscator(char.Parse("R"), char.Parse("G"), 3));
	//    }

	//    [TestMethod]
	//    public void should_be_4_with_char_of_C_and_weights_of_7_and_19()
	//    {
	//      Assert.AreEqual(char.Parse("4"), KeyGenerator.KeyIntegerValueDeObfuscator(char.Parse("C"), char.Parse("7"), 19));
	//    }
	//  }

	//  [TestClass]
	//  public class when_generating_a_single_key : with_the_small_static_key_generator
	//  {
	//    [TestMethod]
	//    public void should_not_be_null()
	//    {
	//      Assert.IsNotNull(key);
	//    }

	//    [TestMethod]
	//    public void should_be_15_chars_long()
	//    {
	//      if (key.Length < 15)
	//        Console.WriteLine(string.Format("Small Static Key less then 15 characters long: {0}", key));

	//      Assert.AreEqual(15, key.Length);
	//    }

	//    [TestMethod]
	//    public void should_be_13_chars_without_dashes()
	//    {
	//      string key1 = key.Replace("-", "");

	//      Assert.AreEqual(13, key1.Length);
	//    }
	//  }

	//  [TestClass]
	//  public class when_validating_a_single_key : with_the_small_static_key_generator
	//  {
	//    [TestMethod]
	//    [ExpectedException(typeof(ScutexLicenseException))]
	//    public void should_throw_exception_when_invalid()
	//    {
	//      char[] keyArray = key.ToCharArray();
	//      keyArray[3] = Char.Parse("0");

	//      string newKey = new string(keyArray);

	//      smallKeyGenerator.ValidateLicenseKey(newKey, license);
	//    }

	//    [TestMethod]
	//    public void should_be_98_precent_accurate_when_validating_100000_bad_licenses()
	//    {
	//      int errorCount = 0;
	//      byte[] randomNumber = new byte[1];
	//      RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();

	//      for (int i = 0; i < 100000; i++)
	//      {
	//        char[] keyArray = key.ToCharArray();

	//        Gen.GetBytes(randomNumber);
	//        int randLocation = Convert.ToInt32(randomNumber[0]);
	//        randLocation = randLocation % keyArray.Count();

	//        randomNumber = new byte[1];
	//        Gen.GetBytes(randomNumber);
	//        int randValue = Convert.ToInt32(randomNumber[0]);
	//        randValue = randValue % 35;

	//        keyArray[randLocation] = CharacterMap.Map[randValue];

	//        string newKey = new string(keyArray);

	//        if (newKey == key)
	//        {
	//          errorCount++;
	//          continue;
	//        }

	//        try
	//        {
	//          smallKeyGenerator.ValidateLicenseKey(newKey, license);
	//          string test1 = key.Replace("-", "");
	//          string test2 = newKey;
	//        }
	//        catch (ScutexLicenseException)
	//        {
	//          errorCount++;
	//        }
	//      }

	//      double accuracy = (double)errorCount / 100000;

	//      Console.WriteLine(string.Format("Actual SmallKeyGenerator validation accuracy: {0} with {1} validation failures out of 100000", accuracy, errorCount));
	//      Assert.IsTrue(accuracy > .98);
	//    }

	//    [TestMethod]
	//    public void should_work_when_using_valid_key()
	//    {
	//      bool test = smallKeyGenerator.ValidateLicenseKey(key, license);

	//      Assert.IsTrue(test);
	//    }
	//  }
	//}
}