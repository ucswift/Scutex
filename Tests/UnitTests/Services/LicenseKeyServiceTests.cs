using System;
using System.Collections.Generic;
using NUnit.Framework;
using WaveTech.Scutex.Generators.StaticKeyGeneratorSmall;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Repositories;
using WaveTech.Scutex.Model.Interfaces.Services;
using WaveTech.Scutex.Providers.AsymmetricEncryptionProvider;
using WaveTech.Scutex.Providers.DataGenerationProvider;
using WaveTech.Scutex.Providers.HashingProvider;
using WaveTech.Scutex.Providers.ObjectSerialization;
using WaveTech.Scutex.Providers.SymmetricEncryptionProvider;
using WaveTech.Scutex.Providers.WebServicesProvider;
using WaveTech.Scutex.Repositories.ClientDataRepository;
using WaveTech.Scutex.Services;

namespace WaveTech.Scutex.UnitTests.Services
{
	[TestFixture]
	public class LicenseKeyServiceTests
	{
		private IAsymmetricEncryptionProvider asymmetricEncryptionProvider = new AsymmetricEncryptionProvider();
		private ISymmetricEncryptionProvider symmetricEncryptionProvider = new SymmetricEncryptionProvider();
		private IHashingProvider hashingProvider = new HashingProvider();
		private IObjectSerializationProvider objectSerializationProvider = new ObjectSerializationProvider();
		private INumberDataGeneratorProvider numberDataGenerationProvider = new NumberDataGenerator();
		private IPackingService packingService;
		public IClientLicenseRepository clientLicenseRepository;
		private IClientLicenseService clientLicenseService;

		private ClientLicense license;
		private LicenseGenerationOptions generationOptions;
		private LicenseKeyService licenseKeyService;
		private KeyGenerator staticKeyGenerator;

		[SetUp]
		public void SetupTests()
		{
			license = new ClientLicense();
			license.UniqueId = Guid.NewGuid();

			generationOptions = new LicenseGenerationOptions();

			license.Product = new Product();
			license.Product.Name = "My Great Uber Cool Product, with new juice!";
			license.Product.ProductId = 1;

			packingService = new PackingService(numberDataGenerationProvider);
			clientLicenseRepository = new ClientLicenseRepository(objectSerializationProvider, symmetricEncryptionProvider);
			clientLicenseService = new ClientLicenseService(clientLicenseRepository);

			string productHash = hashingProvider.Checksum32(license.GetLicenseProductIdentifier()).ToString("X");

			LicenseActiviationProvider licenseActiviationProvider = new LicenseActiviationProvider(asymmetricEncryptionProvider, symmetricEncryptionProvider, objectSerializationProvider);

			staticKeyGenerator = new KeyGenerator(symmetricEncryptionProvider, asymmetricEncryptionProvider, hashingProvider);
			licenseKeyService = new LicenseKeyService(staticKeyGenerator, packingService, clientLicenseService);
		}

		[Test]
		public void VerifyLicenseKeyLength()
		{
			string key = licenseKeyService.GenerateLicenseKey("TEST", license, generationOptions);

			Assert.AreEqual(29, key.Length);
		}

		[Test]
		public void NoDouplicateLicenseKeys()
		{
			Dictionary<string, string> licenseKeys = new Dictionary<string, string>();

			for (int i = 0; i < 100000; i++)
			{
				string key = licenseKeyService.GenerateLicenseKey("TEST", license, generationOptions);
				licenseKeys.Add(key, key);
			}
		}

		[Test]
		public void LicenseKeyBatchGeneratorVerify()
		{
			List<string> keys = licenseKeyService.GenerateLicenseKeys("TEST", license, generationOptions, 100000);

			Assert.IsNotNull(keys);
			Assert.AreEqual(100000, keys.Count);
		}

		[Test]
		public void LicenseValidatorShouldWork()
		{
			string key = licenseKeyService.GenerateLicenseKey("TEST", license, generationOptions);

			Assert.IsNotNull(key);

			bool check = licenseKeyService.ValidateLicenseKey(key, license, false);

			Assert.IsTrue(check);
		}

		//[Test]
		//[ExpectedException(typeof(ScutexLicenseException))]
		//public void LicenseValidatorShouldThowError()
		//{
		//  string key = licenseKeyService.GenerateLicenseKey("TEST", license, generationOptions);

		//  Assert.IsNotNull(key);

		//  char[] keyArray = key.ToCharArray();
		//  keyArray[3] = Char.Parse("0");

		//  string newKey = new string(keyArray);

		//  licenseKeyService.ValidateLicenseKey(newKey, "TEST", license);
		//}
	}
}