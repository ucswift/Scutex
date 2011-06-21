
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Providers.AsymmetricEncryptionProvider;
using WaveTech.Scutex.Providers.DataGenerationProvider;
using WaveTech.Scutex.Providers.HashingProvider;
using WaveTech.Scutex.Providers.ObjectSerialization;
using WaveTech.Scutex.Services;

namespace WaveTech.Scutex.UnitTests.Wcf.Client
{
	namespace EncryptionTests
	{
		[TestClass]
		public class with_the_client_encryption_protocol
		{
			internal HashingProvider hashingProvider;
			internal ObjectSerializationProvider objectSerializationProvider;
			internal AsymmetricEncryptionProvider asymmetricEncryptionProvider;
			internal NumberDataGenerator numberDataGenerator;
			internal PackingService packingService;
			internal ServicesService servicesService;
			internal Service service;

			[TestInitialize]
			public void Before_each_test()
			{
				asymmetricEncryptionProvider = new AsymmetricEncryptionProvider();
				hashingProvider = new HashingProvider();
				objectSerializationProvider = new ObjectSerializationProvider();
				numberDataGenerator = new NumberDataGenerator();
				packingService = new PackingService(numberDataGenerator);
				servicesService = new ServicesService(null, null, packingService, null, null, null, null, null, null);

				string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
				path = path.Replace("file:\\", "");

				string serviceData;

				using (TextReader reader = new StreamReader(path + "\\Data\\Service.dat"))
				{
					serviceData = reader.ReadToEnd().Trim();
				}

				service = objectSerializationProvider.Deserialize<Service>(serviceData);
			}
		}

		[TestClass]
		public class when_encrypting_payload_data : with_the_client_encryption_protocol
		{
			[TestMethod]
			public void should_not_be_null()
			{
				LicenseActivationPayload payload = new LicenseActivationPayload();
				payload.LicenseKey = "999-999999-9999";
				payload.ServiceLicense = new ServiceLicense(servicesService.CreateTestClientLicense(service));

				string serializedPayload = objectSerializationProvider.Serialize(payload);
				string encryptedData = asymmetricEncryptionProvider.EncryptPrivate(serializedPayload, servicesService.CreateTestClientLicense(service).ServicesKeys);

				Assert.IsNotNull(encryptedData);
			}

			[TestMethod]
			public void should_decrypt()
			{
				LicenseActivationPayload payload = new LicenseActivationPayload();
				payload.LicenseKey = "999-999999-9999";
				payload.ServiceLicense = new ServiceLicense(servicesService.CreateTestClientLicense(service));

				string serializedPayload = objectSerializationProvider.Serialize(payload);
				string encryptedData = asymmetricEncryptionProvider.EncryptPrivate(serializedPayload, servicesService.CreateTestClientLicense(service).ServicesKeys);

				KeyPair kp = new KeyPair();
				kp.PrivateKey = service.OutboundKeyPair.PrivateKey;
				kp.PublicKey = service.InboundKeyPair.PublicKey;

				string unencryptedData = asymmetricEncryptionProvider.DecryptPublic(encryptedData, kp);

				Assert.IsNotNull(unencryptedData);
				Assert.AreEqual(serializedPayload, unencryptedData);
			}

			[TestMethod]
			public void should_deserialize()
			{
				LicenseActivationPayload payload = new LicenseActivationPayload();
				payload.LicenseKey = "999-999999-9999";
				payload.ServiceLicense = new ServiceLicense(servicesService.CreateTestClientLicense(service));

				string serializedPayload = objectSerializationProvider.Serialize(payload);
				string encryptedData = asymmetricEncryptionProvider.EncryptPrivate(serializedPayload, servicesService.CreateTestClientLicense(service).ServicesKeys);

				KeyPair kp = new KeyPair();
				kp.PrivateKey = service.OutboundKeyPair.PrivateKey;
				kp.PublicKey = service.InboundKeyPair.PublicKey;

				string unencryptedData = asymmetricEncryptionProvider.DecryptPublic(encryptedData, kp);

				LicenseActivationPayload newPayload =
					objectSerializationProvider.Deserialize<LicenseActivationPayload>(unencryptedData);

				Assert.IsNotNull(newPayload);
			}
		}
	}
}
