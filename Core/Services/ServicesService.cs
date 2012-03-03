using System;
using System.Collections.Generic;
using System.Linq;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Repositories;
using WaveTech.Scutex.Model.Interfaces.Services;
using WaveTech.Scutex.Model.Results;
using WaveTech.Scutex.Model.ServiceData;
using WaveTech.Scutex.Services.Properties;

namespace WaveTech.Scutex.Services
{
	internal class ServicesService : IServicesService
	{
		#region Private Readonly Members
		private readonly IServicesRepository _servicesRepository;
		private readonly IServiceStatusProvider _serviceStatusProvider;
		private readonly IPackingService _packingService;
		private readonly ILicenseActiviationProvider _licenseActiviationProvider;
		private readonly ILicenseKeyService _licenseKeyService;
		private readonly ILicenseService _licenseService;
		private readonly ILicenseSetService _licenseSetService;
		private readonly IClientLicenseService _clientLicenseService;
		private readonly IProductsProvider _productsProvider;
		#endregion Private Readonly Members

		#region Constructor
		public ServicesService(IServicesRepository servicesRepository, IServiceStatusProvider serviceStatusProvider,
			IPackingService packingService, ILicenseActiviationProvider licenseActiviationProvider, ILicenseKeyService licenseKeyService,
			ILicenseService licenseService, ILicenseSetService licenseSetService, IClientLicenseService clientLicenseService,
			IProductsProvider productsProvider)
		{
			_servicesRepository = servicesRepository;
			_serviceStatusProvider = serviceStatusProvider;
			_packingService = packingService;
			_licenseActiviationProvider = licenseActiviationProvider;
			_licenseKeyService = licenseKeyService;
			_licenseService = licenseService;
			_licenseSetService = licenseSetService;
			_clientLicenseService = clientLicenseService;
			_productsProvider = productsProvider;
		}
		#endregion Constructor

		public List<Service> GetAllServices()
		{
			return _servicesRepository.GetAllServices().ToList();
		}

		public Service SaveService(Service service)
		{
			if (service.ServiceId == 0)
				return _servicesRepository.InsertService(service).First();
			else
				return _servicesRepository.UpdateService(service).First();
		}

		public List<Service> GetAllNonInitializedServices()
		{
			return (from s in GetAllServices()
							where s.Initialized == false
							select s).ToList();
		}

		public List<Service> GetAllNonInitializedNonTestedServices()
		{
			return (from s in GetAllServices()
							where s.Initialized == false || s.Tested == false
							select s).ToList();
		}

		public List<Service> GetAllInitializedActiveServices()
		{
			return (from s in GetAllServices()
							where s.Initialized == true && s.Tested == true
							select s).ToList();
		}

		public Service GetServiceById(int serviceId)
		{
			return (from s in GetAllServices()
							where s.ServiceId == serviceId
							select s).FirstOrDefault();
		}

		public void DeleteServiceById(int serviceId)
		{
			_servicesRepository.DeleteServiceById(serviceId);
		}

		public EncryptionInfo GetManagementStandardEncryptionInfo(Service service)
		{
			EncryptionInfo ei = new EncryptionInfo();
			ei.HashAlgorithm = "SHA1";
			ei.InitVector = Resources.ServicesIV;
			ei.Iterations = 2;
			ei.KeySize = 192;
			ei.PassPhrase = service.GetManagementOutboundKeyPart2();
			ei.SaltValue = service.GetManagementInboundKeyPart2();

			return ei;
		}

		public EncryptionInfo GetClientStandardEncryptionInfo(Service service)
		{
			EncryptionInfo ei = new EncryptionInfo();
			ei.HashAlgorithm = "SHA1";
			ei.InitVector = Resources.ServicesIV;
			ei.Iterations = 2;
			ei.KeySize = 192;
			ei.PassPhrase = service.GetClientOutboundKeyPart2();
			ei.SaltValue = service.GetClientInboundKeyPart2();

			return ei;
		}

		public bool InitializeService(Service service)
		{
			string token = _packingService.PackToken(service.GetManagementToken());
			StatusRequestResult statusResult = _serviceStatusProvider.GetServiceStatus(service.ManagementUrl, token, GetManagementStandardEncryptionInfo(service));

			if (statusResult.IsInitialized == true)
				return false;

			if (statusResult.IsActive == true)
				return false;

			if (statusResult.IsRequestValid == false)
				return false;

			MasterServiceData masterData = new MasterServiceData();
			masterData.ServiceId = service.UniquePad;
			masterData.Token = service.Token;
			masterData.Initialized = true;
			masterData.ClientInboundKey = service.GetClientInboundKeyPart1();
			masterData.ClientOutboundKey = service.GetClientOutboundKeyPart1();
			masterData.ManagementInboundKey = service.GetManagementInboundKeyPart1();
			masterData.ManagementOutboundKey = service.GetManagementOutboundKeyPart1();

			InitializationResult result = _serviceStatusProvider.InitializeService(service.ManagementUrl, token, masterData,
																							 GetManagementStandardEncryptionInfo(service));

			if (!result.WasInitializionSucessful)
				return false;

			return true;
		}

		public bool TestService(Service service)
		{
			string clientToken = _packingService.PackToken(service.GetClientToken());
			string mgmtToken = _packingService.PackToken(service.GetManagementToken());

			LicenseActivationPayload payload = new LicenseActivationPayload();
			payload.ServiceLicense = new ServiceLicense(CreateTestClientLicense(service));

			LicenseGenerationOptions options = new LicenseGenerationOptions();
			options.LicenseKeyType = LicenseKeyTypes.MultiUser;

			payload.LicenseKey = _licenseKeyService.GenerateLicenseKey(null, payload.ServiceLicense, options);

			SetupTestProductResult result = _serviceStatusProvider.SetupTestProduct(service.ManagementUrl, mgmtToken, payload.LicenseKey, GetManagementStandardEncryptionInfo(service));

			if (result.WasRequestValid == false)
				return false;

			if (result.WasOperationSuccessful == false)
				return false;

			ActivationResult activationResult1 = _licenseActiviationProvider.ActivateLicense(service.ClientUrl, clientToken, GetClientStandardEncryptionInfo(service),
				payload, CreateTestClientLicense(service));

			ActivationResult activationResult2 = _licenseActiviationProvider.ActivateLicense(service.ClientUrl, clientToken, GetClientStandardEncryptionInfo(service),
				payload, CreateTestClientLicense(service));

			ActivationResult activationResult3 = _licenseActiviationProvider.ActivateLicense(service.ClientUrl, clientToken, GetClientStandardEncryptionInfo(service),
				payload, CreateTestClientLicense(service));

			if (activationResult1.WasRequestValid == false || activationResult1.ActivationSuccessful == false)
				return false;

			if (activationResult2.WasRequestValid == false || activationResult2.ActivationSuccessful == false)
				return false;

			if (activationResult3.WasRequestValid == false || activationResult3.ActivationSuccessful == true)
				return false;

			SetupTestProductResult cleanUpResult = _serviceStatusProvider.CleanUpTestProductData(service.ManagementUrl, mgmtToken,
																																													 GetManagementStandardEncryptionInfo
																																														(service));

			if (cleanUpResult.WasOperationSuccessful == false)
				return false;

			return true;
		}

		public Dictionary<License, List<LicenseSet>> GetServiceLicenses(Service service)
		{
			Dictionary<License, List<LicenseSet>> data = new Dictionary<License, List<LicenseSet>>();
			string mgmtToken = _packingService.PackToken(service.GetManagementToken());

			QueryActiveServiceProductsResult result = _serviceStatusProvider.GetActiveServiceProducts(service.ManagementUrl, mgmtToken,
																										GetManagementStandardEncryptionInfo(service), service.GetManagementServiceKeyPair());

			if (IsResultValid(result))
			{
				foreach (var v in result.ProductsAndLicenseSets)
				{
					License l = _licenseService.GetLicenseById(v.Id);

					List<LicenseSet> sets = new List<LicenseSet>();

					foreach (int i in v.SetIds)
					{
						sets.Add(_licenseSetService.GetLiceseSetById(i));
					}

					data.Add(l, sets);
				}
			}

			return data;
		}

		internal bool IsResultValid(BaseServiceResult result)
		{
			if (result.WasException)
				return false;

			if (result.WasRequestValid == false)
				return false;

			if (result.WasOperationSuccessful == false)
				return false;

			return true;
		}

		internal ClientLicense CreateTestClientLicense(Service service)
		{
			ClientLicense cl = new ClientLicense();

			cl.ServicesKeys = service.GetClientServiceKeyPair();
			cl.Product = new Product();
			cl.Product.Name = "Test Product";

			cl.LicenseSets = new NotifyList<LicenseSet>();
			LicenseSet ls = new LicenseSet();
			ls.LicenseId = int.MaxValue;
			ls.LicenseSetId = int.MaxValue;
			ls.Name = "Test Product License Set";
			ls.MaxUsers = 2;
			ls.SupportedLicenseTypes = LicenseKeyTypeFlag.MultiUser;

			cl.LicenseSets.Add(ls);

			return cl;
		}

		internal EncryptionInfo GetClientStandardEncryptionInfo(ClientLicense clientLicense)
		{
			EncryptionInfo ei = new EncryptionInfo();
			ei.HashAlgorithm = "SHA1";
			ei.InitVector = Resources.ServicesIV;
			ei.Iterations = 2;
			ei.KeySize = 192;

			// Outbound Key
			string outKey1 = clientLicense.ServicesKeys.PrivateKey.Substring(0, (clientLicense.ServicesKeys.PrivateKey.Length / 2));
			string outKey2 = clientLicense.ServicesKeys.PrivateKey.Substring(outKey1.Length, (clientLicense.ServicesKeys.PrivateKey.Length - outKey1.Length));

			// Inbound Key
			string inKey1 = clientLicense.ServicesKeys.PublicKey.Substring(0, (clientLicense.ServicesKeys.PublicKey.Length / 2));
			string inKey2 = clientLicense.ServicesKeys.PublicKey.Substring(inKey1.Length, (clientLicense.ServicesKeys.PublicKey.Length - inKey1.Length));

			ei.PassPhrase = outKey2;
			ei.SaltValue = inKey2;

			return ei;
		}

		public bool AddProductToService(License license, List<LicenseSet> licenseSets, Service service)
		{
			ServiceProduct sp = new ServiceProduct();
			sp.LicenseId = license.LicenseId;
			sp.LicenseName = license.Name;
			sp.LicenseSets = new List<ServiceLicenseSet>();

			foreach (var ls in licenseSets)
			{
				ServiceLicenseSet set = new ServiceLicenseSet();
				set.LicenseId = license.LicenseId;
				set.LicenseSetId = ls.LicenseSetId;
				set.LicenseSetName = ls.Name;
				set.LicenseType = ls.SupportedLicenseTypes;
				set.MaxUsers = ls.MaxUsers;

				sp.LicenseSets.Add(set);
			}

			string mgmtToken = _packingService.PackToken(service.GetManagementToken());

			AddProductResult result = _productsProvider.AddProduct(service.ManagementUrl, mgmtToken,
																														 GetManagementStandardEncryptionInfo(service),
																														 service.GetManagementServiceKeyPair(), sp);

			if (IsResultValid(result))
				return true;

			return false;
		}

		public List<string> GetServiceLicenseKeysForSet(LicenseSet licenseSet, Service service)
		{
			string mgmtToken = _packingService.PackToken(service.GetManagementToken());

			GetLicenseKeysForProductData data = new GetLicenseKeysForProductData();
			data.LicenseSetId = licenseSet.LicenseSetId;

			GetLicenseKeysForProductResult result = _productsProvider.GetLicenseKeysForLicenseSet(service.ManagementUrl, mgmtToken,
																														 GetManagementStandardEncryptionInfo(service),
																														 service.GetManagementServiceKeyPair(), data);

			if (IsResultValid(result))
				return result.LicenseKeys;

			return new List<string>();
		}

		public bool AddLicenseKeysToService(LicenseSet licenseSet, Service service, List<string> keys)
		{
			string mgmtToken = _packingService.PackToken(service.GetManagementToken());

			AddLicenseKeysForProductData data = new AddLicenseKeysForProductData();
			data.LicenseSetId = licenseSet.LicenseSetId;
			data.Keys = keys;

			AddLicenseKeysForProductResult result = _productsProvider.AddLicenseKeysForLicenseSet(service.ManagementUrl, mgmtToken,
																														 GetManagementStandardEncryptionInfo(service),
																														 service.GetManagementServiceKeyPair(), data);

			if (IsResultValid(result))
				return true;

			return false;
		}

		public bool TestClientServiceUrl(Service service)
		{
			string result;

			try
			{
				result = _licenseActiviationProvider.BasicServiceTest(service.ClientUrl);
			}
			catch(Exception ex)
			{
				return false;
			}

			return result == "Ok";
		}

		public bool TestManagementServiceUrl(Service service)
		{
			string result;

			try
			{
				result = _serviceStatusProvider.BasicServiceTest(service.ManagementUrl);
			}
			catch (Exception ex)
			{
				return false;
			}

			return result == "Ok";
		}
	}
}