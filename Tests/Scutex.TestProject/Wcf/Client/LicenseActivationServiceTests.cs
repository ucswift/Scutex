
using System.IO;
using System.Reflection;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WaveTech.Scutex.Generators.StaticKeyGeneratorSmall;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Repositories;
using WaveTech.Scutex.Model.Interfaces.Services;
using WaveTech.Scutex.Model.Interfaces.Wcf;
using WaveTech.Scutex.Model.Results;
using WaveTech.Scutex.Providers.AsymmetricEncryptionProvider;
using WaveTech.Scutex.Providers.DataGenerationProvider;
using WaveTech.Scutex.Providers.HashingProvider;
using WaveTech.Scutex.Providers.ObjectSerialization;
using WaveTech.Scutex.Providers.SymmetricEncryptionProvider;
using WaveTech.Scutex.Providers.WebServicesProvider;
using WaveTech.Scutex.Repositories.ClientDataRepository;
using WaveTech.Scutex.Repositories.ManagerDataRepository;
using WaveTech.Scutex.Repositories.ServicesDataRepository;
using WaveTech.Scutex.Services;
using WaveTech.Scutex.WcfServices.ServicesLibrary.Services;
using WaveTech.Scutex.WcfServices.ServicesLibrary.Services.Client;
using Service = WaveTech.Scutex.Model.Service;

namespace WaveTech.Scutex.UnitTests.Wcf.Client
{
	namespace LicenseActivationServiceTests
	{
		[TestClass]
		public class with_the_license_activation_service
		{
			internal SymmetricEncryptionProvider symmetricEncryptionProvider;
			internal AsymmetricEncryptionProvider asymmetricEncryptionProvider;
			internal HashingProvider hashingProvider;
			internal ObjectSerializationProvider objectSerializationProvider;
			internal NumberDataGenerator numberDataGenerator;
			internal PackingService packingService;
			internal CommonRepository commonRepository;
			internal MasterService masterService;
			internal KeyManagementService keyService;
			internal ControlService controlService;
			internal ActivationService activationService;
			internal ClientRepository clientRepository;
			internal LicenseKeyService licenseKeyService;
			internal KeyGenerator keyGenerator;
			internal ServicesService servicesService;
			internal ServicesRepository servicesRepository;
			internal ServiceStatusProvider serviceStatusProvider;
			internal LicenseActiviationProvider licenseActiviationProvider;
			internal CommonService commonService;
			internal KeyPairService keyPairService;
			internal IActivationLogRepoistory activationLogRepository;
			internal IActivationLogService activationLogService;
			internal IServiceProductsRepository serviceProductsRepository;
			internal IClientLicenseRepository clientLicenseRepoistory;
			internal IClientLicenseService clientLicenseService;

			internal Service service;
			internal MasterServiceData masterServiceData;

			[TestInitialize]
			public void Before_each_test()
			{
				clientLicenseRepoistory = new ClientLicenseRepository(objectSerializationProvider, symmetricEncryptionProvider);
				clientLicenseService = new ClientLicenseService(clientLicenseRepoistory);
				serviceProductsRepository = new ServiceProductsRepository(new ScutexServiceEntities());
				symmetricEncryptionProvider = new SymmetricEncryptionProvider();
				asymmetricEncryptionProvider = new AsymmetricEncryptionProvider();
				hashingProvider = new HashingProvider();
				objectSerializationProvider = new ObjectSerializationProvider();
				numberDataGenerator = new NumberDataGenerator();
				packingService = new PackingService(numberDataGenerator);
				commonRepository = new CommonRepository(new ScutexServiceEntities());
				clientRepository = new ClientRepository(new ScutexServiceEntities());
				keyGenerator = new KeyGenerator(symmetricEncryptionProvider, asymmetricEncryptionProvider, hashingProvider);
				masterService = new MasterService(commonRepository);

				var mockActivationLogRepository = new Mock<IActivationLogRepoistory>();
				mockActivationLogRepository.Setup(log => log.SaveActivationLog(It.IsAny<Scutex.Model.ActivationLog>()));

				activationLogService = new ActivationLogService(mockActivationLogRepository.Object, hashingProvider);
				commonService = new CommonService();

				string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
				path = path.Replace("file:\\", "");

				var mockCommonService = new Mock<ICommonService>();
				mockCommonService.Setup(common => common.GetPath()).Returns(path + "\\Data\\Client\\");

				string masterServiceDataText;

				using (TextReader reader = new StreamReader(path + "\\Data\\MasterService.dat"))
				{
					masterServiceDataText = reader.ReadToEnd().Trim();
				}

				masterServiceData = objectSerializationProvider.Deserialize<MasterServiceData>(masterServiceDataText);

				var mockCommonRepository = new Mock<ICommonRepository>();
				mockCommonRepository.Setup(repo => repo.GetMasterServiceData()).Returns(masterServiceData);

				masterService = new MasterService(mockCommonRepository.Object);

				keyPairService = new KeyPairService(mockCommonService.Object, mockCommonRepository.Object);
				controlService = new ControlService(symmetricEncryptionProvider, keyPairService, packingService, masterService, objectSerializationProvider, asymmetricEncryptionProvider);
				servicesRepository = new ServicesRepository(new ScutexEntities());
				serviceStatusProvider = new ServiceStatusProvider(symmetricEncryptionProvider, objectSerializationProvider, asymmetricEncryptionProvider);
				licenseActiviationProvider = new LicenseActiviationProvider(asymmetricEncryptionProvider, symmetricEncryptionProvider, objectSerializationProvider);
				servicesService = new ServicesService(servicesRepository, serviceStatusProvider, packingService, licenseActiviationProvider, null, null, null, null, null);
				licenseKeyService = new LicenseKeyService(keyGenerator, packingService, clientLicenseService);
				keyService = new KeyManagementService(clientRepository, licenseKeyService, activationLogService, hashingProvider, serviceProductsRepository);
				activationService = new ActivationService(controlService, keyService, keyPairService, objectSerializationProvider, asymmetricEncryptionProvider, activationLogService, masterService);
	
				string serviceData;

				using (TextReader reader = new StreamReader(path + "\\Data\\Service.dat"))
				{
					serviceData = reader.ReadToEnd().Trim();
				}

				service = objectSerializationProvider.Deserialize<Service>(serviceData);
			}
		}

		[TestClass]
		public class when_activating_a_license_key : with_the_license_activation_service
		{
			[TestMethod]
			public void should_not_be_null()
			{
				string clientToken = packingService.PackToken(service.GetClientToken());

				LicenseActivationPayload payload = new LicenseActivationPayload();
				payload.LicenseKey = "999-999999-9999";
				payload.ServiceLicense = new ServiceLicense(servicesService.CreateTestClientLicense(service));

				string encryptedToken = symmetricEncryptionProvider.Encrypt(clientToken, servicesService.GetClientStandardEncryptionInfo(service));
				string serializedPayload = objectSerializationProvider.Serialize(payload);
				string encryptedData = asymmetricEncryptionProvider.EncryptPrivate(serializedPayload, servicesService.CreateTestClientLicense(service).ServicesKeys);


				string encryptedResult = activationService.ActivateLicense(encryptedToken, encryptedData);

				Assert.IsNotNull(encryptedResult);
			}

			[TestMethod]
			public void should_be_valid_request()
			{
				string clientToken = packingService.PackToken(service.GetClientToken());

				LicenseActivationPayload payload = new LicenseActivationPayload();
				payload.LicenseKey = "999-999999-9999";
				payload.ServiceLicense = new ServiceLicense(servicesService.CreateTestClientLicense(service));

				string encryptedToken = symmetricEncryptionProvider.Encrypt(clientToken, servicesService.GetClientStandardEncryptionInfo(service));
				string serializedPayload = objectSerializationProvider.Serialize(payload);
				string encryptedData = asymmetricEncryptionProvider.EncryptPrivate(serializedPayload, servicesService.CreateTestClientLicense(service).ServicesKeys);


				string encryptedResult = activationService.ActivateLicense(encryptedToken, encryptedData);
				string decryptedResult = asymmetricEncryptionProvider.DecryptPublic(encryptedResult, servicesService.CreateTestClientLicense(service).ServicesKeys);
				ActivationResult result = objectSerializationProvider.Deserialize<ActivationResult>(decryptedResult);

				Assert.IsTrue(result.WasRequestValid);
			}

			[TestMethod]
			public void should_not_have_exception()
			{
				string clientToken = packingService.PackToken(service.GetClientToken());

				LicenseActivationPayload payload = new LicenseActivationPayload();
				payload.LicenseKey = "999-999999-9999";
				payload.ServiceLicense = new ServiceLicense(servicesService.CreateTestClientLicense(service));

				string encryptedToken = symmetricEncryptionProvider.Encrypt(clientToken, servicesService.GetClientStandardEncryptionInfo(service));
				string serializedPayload = objectSerializationProvider.Serialize(payload);
				string encryptedData = asymmetricEncryptionProvider.EncryptPrivate(serializedPayload, servicesService.CreateTestClientLicense(service).ServicesKeys);


				string encryptedResult = activationService.ActivateLicense(encryptedToken, encryptedData);
				string decryptedResult = asymmetricEncryptionProvider.DecryptPublic(encryptedResult, servicesService.CreateTestClientLicense(service).ServicesKeys);
				ActivationResult result = objectSerializationProvider.Deserialize<ActivationResult>(decryptedResult);

				Assert.IsFalse(result.WasException);
			}

			[TestMethod]
			public void should_not_have_been_successful()
			{
				string clientToken = packingService.PackToken(service.GetClientToken());

				LicenseActivationPayload payload = new LicenseActivationPayload();
				payload.LicenseKey = "999-999999-9999";
				payload.ServiceLicense = new ServiceLicense(servicesService.CreateTestClientLicense(service));

				string encryptedToken = symmetricEncryptionProvider.Encrypt(clientToken, servicesService.GetClientStandardEncryptionInfo(service));
				string serializedPayload = objectSerializationProvider.Serialize(payload);
				string encryptedData = asymmetricEncryptionProvider.EncryptPrivate(serializedPayload, servicesService.CreateTestClientLicense(service).ServicesKeys);


				string encryptedResult = activationService.ActivateLicense(encryptedToken, encryptedData);
				string decryptedResult = asymmetricEncryptionProvider.DecryptPublic(encryptedResult, servicesService.CreateTestClientLicense(service).ServicesKeys);
				ActivationResult result = objectSerializationProvider.Deserialize<ActivationResult>(decryptedResult);

				Assert.IsFalse(result.WasOperationSuccessful);
			}
		}
	}
}
