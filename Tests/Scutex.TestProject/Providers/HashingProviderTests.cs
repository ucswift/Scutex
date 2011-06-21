using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Generators;
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

namespace WaveTech.Scutex.UnitTests.Providers
{
	[TestClass]
	public class HashingProviderTests
	{
		private IHashingProvider provider;
		private string hashValue = "61-ED-D1-70-92-3D-73-8A-56-4E-F9-36-BD-33-C0-61-4E-83-17-50";
		private string checksumTest = "1209E0B6770C1340A1C2E";
		private string checksumHash = "C7C7";
		private int checksumNumber = 51143;

		private string testSmallLicenseChecksumData = "5ZGA1F64G8Z";
		private string testSmallLicenseChecksumDataWithCheck = "5ZGA1F64G8Z8";
		private string testSmallLicenseChecksumDataWithBadCheck = "5ZGA1F64G8Z1";
		private string testLargeChecksumData = "LeroyAlabasterJinkinsEnterpriseWidgetsSystemsIncorrporated";
		private string testLargeChecksumData2 = "LeroyAlabasterJinkinsEnterpriseWidgetsSystemsIncorrporate";

		[TestInitialize]
		public void WireUp()
		{
			provider = new HashingProvider();
		}

		[TestMethod]
		public void FileHashingShouldWork()
		{
			//string test = provider.HashFile(Directory.GetCurrentDirectory() + "\\FileHashingTestFile.txt");
			string test = provider.HashFile(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\FileHashingTestFile.txt");

			Assert.IsNotNull(test);
			Assert.AreEqual(hashValue, test);
		}

		[TestMethod]
		public void FileHashingShouldNOTWork()
		{
			//string test = provider.HashFile(Directory.GetCurrentDirectory() + "\\FileHashingTestFile2.txt");
			string test = provider.HashFile(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\FileHashingTestFile2.txt");

			Assert.IsNotNull(test);
			Assert.AreNotEqual(hashValue, test);
		}

		[TestMethod]
		public void Checksum16SmallDataSet()
		{
			int checksum = provider.Checksum16(checksumTest.Replace("-", ""));
			string hex = checksum.ToString("X");

			Assert.AreEqual(checksumNumber, checksum);
			Assert.AreEqual(checksumHash, hex);
		}

		[TestMethod]
		public void Checksum32LargeDataSet()
		{
			uint checksum = provider.Checksum32(testLargeChecksumData);
			string hex = checksum.ToString("X");
		}

		[TestMethod]
		public void Checksum16LargeDataSetCompare()
		{

			int checksum = provider.Checksum16(testLargeChecksumData);
			int checksum2 = provider.Checksum16(testLargeChecksumData2);

			Assert.AreNotEqual(checksum, checksum2);
		}

		[TestMethod]
		public void TestCheckCharacterGeneration()
		{
			char data = provider.GetChecksumChar(testSmallLicenseChecksumData);

			Assert.AreEqual(char.Parse("8"), data);
		}

		[TestMethod]
		public void TestCheckCharacterValidation()
		{
			bool data = provider.ValidateChecksumChar(testSmallLicenseChecksumDataWithCheck);

			Assert.IsTrue(data);
		}

		[TestMethod]
		public void TestBadCheckCharacterValidation()
		{
			bool data = provider.ValidateChecksumChar(testSmallLicenseChecksumDataWithBadCheck);

			Assert.IsFalse(data);
		}

		[TestMethod]
		public void TestLicenseKeyHashing()
		{
			IAsymmetricEncryptionProvider asymmetricEncryptionProvider = new AsymmetricEncryptionProvider();
			ISymmetricEncryptionProvider symmetricEncryptionProvider = new SymmetricEncryptionProvider();
			IHashingProvider hashingProvider = new HashingProvider();
			IObjectSerializationProvider objectSerializationProvider = new ObjectSerializationProvider();
			ILicenseActiviationProvider licenseActiviationProvider = new LicenseActiviationProvider(
				asymmetricEncryptionProvider, symmetricEncryptionProvider, objectSerializationProvider);
			INumberDataGeneratorProvider numberDataGeneratorProvider = new NumberDataGenerator();
			IPackingService packingService = new PackingService(numberDataGeneratorProvider);
			IClientLicenseRepository clientLicenseRepository = new ClientLicenseRepository(objectSerializationProvider,
																																										 symmetricEncryptionProvider);
			IClientLicenseService clientLicenseService = new ClientLicenseService(clientLicenseRepository);

			IKeyGenerator smallKeyGenerator = new WaveTech.Scutex.Generators.StaticKeyGeneratorSmall.KeyGenerator(symmetricEncryptionProvider, asymmetricEncryptionProvider, hashingProvider);
			LicenseKeyService licenseKeyService = new LicenseKeyService(smallKeyGenerator, packingService, clientLicenseService);

			ClientLicense license = new ClientLicense();
			LicenseGenerationOptions generationOptions = new LicenseGenerationOptions();

			license.UniqueId = Guid.NewGuid();
			license.Product = new Product();
			license.Product.Name = "My Great Uber Cool Product, with new juice!";
			license.Product.ProductId = 1;
			string productHash = hashingProvider.Checksum32(license.GetLicenseProductIdentifier()).ToString("X");

			Dictionary<string, string> licenseKeys = new Dictionary<string, string>();
			List<string> keys = licenseKeyService.GenerateLicenseKeys("TEST", license, generationOptions, 100000);

			foreach (string key in keys)
			{
				string hash = hashingProvider.ComputeHash(key, "SHA256");
				licenseKeys.Add(hash, key);
				Console.WriteLine(key + "\t" + hash);

				Assert.IsTrue(hash.Equals(hashingProvider.ComputeHash(key, "SHA256")));
				Assert.IsFalse(hash.Contains("'"));
			}
		}
	}
}
