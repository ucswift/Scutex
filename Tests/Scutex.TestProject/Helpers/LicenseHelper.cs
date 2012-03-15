
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WaveTech.Scutex.Generators.StaticKeyGeneratorSmall;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Generators;
using WaveTech.Scutex.Providers.AsymmetricEncryptionProvider;
using WaveTech.Scutex.Providers.CompressionProvider;
using WaveTech.Scutex.Providers.DataGenerationProvider;
using WaveTech.Scutex.Providers.HashingProvider;
using WaveTech.Scutex.Providers.ObjectSerialization;
using WaveTech.Scutex.Providers.SymmetricEncryptionProvider;
using WaveTech.Scutex.Providers.WebServicesProvider;
using WaveTech.Scutex.Providers.WmiDataProvider;
using WaveTech.Scutex.Repositories.ClientDataRepository;
using WaveTech.Scutex.Repositories.ManagerDataRepository;
using WaveTech.Scutex.Services;
using License = WaveTech.Scutex.Model.License;
using LicenseSet = WaveTech.Scutex.Model.LicenseSet;
using Product = WaveTech.Scutex.Model.Product;
using Service = WaveTech.Scutex.Model.Service;

namespace WaveTech.Scutex.UnitTests
{
	public class LicenseHelper
	{
		public string PublicKey { get; set; }
		public string DllHash { get; set; }
		public License License { get; set; }
		public Service Service { get; set; }

		AsymmetricEncryptionProvider asymmetricEncryptionProvider;
		HashingProvider hashingProvider;
		EncodingService encodingService;
		ObjectSerializationProvider objectSerializationProvider;
		SymmetricEncryptionProvider symmetricEncryptionProvider;
		ClientLicenseRepository clientLicenseRepository;
		ClientLicenseService clientLicenseService;
		ServiceStatusProvider serviceStatusProvider;
		NumberDataGenerator numberDataGenerator;
		PackingService packingService;
		HardwareFingerprintService hardwareFingerprintService;
		KeyGenerator keygen;
		ILargeKeyGenerator keyGeneratorLarge;
		LicenseActiviationProvider licenseActiviationProvider;
		LicenseKeyService service;
		private ProductsProvider productsProvider;
		LicenseService licenseService;
		private LicenseSetService licenseSetService;
		private LicenseSetsRepository licenseSetsRepository;
		WcfPackagingService wcfPackagingService;
		private ZipCompressionProvider zipCompressionProvider;
		ServicesService _servicesService;

		public LicenseHelper()
		{
			asymmetricEncryptionProvider = new AsymmetricEncryptionProvider();
			hashingProvider = new HashingProvider();
			encodingService = new EncodingService();
			objectSerializationProvider = new ObjectSerializationProvider();
			symmetricEncryptionProvider = new SymmetricEncryptionProvider();
			clientLicenseRepository = new ClientLicenseRepository(objectSerializationProvider, symmetricEncryptionProvider);
			clientLicenseService = new ClientLicenseService(clientLicenseRepository);
			serviceStatusProvider = new ServiceStatusProvider(symmetricEncryptionProvider, objectSerializationProvider, asymmetricEncryptionProvider);
			numberDataGenerator = new NumberDataGenerator();
			packingService = new PackingService(numberDataGenerator);
			hardwareFingerprintService = new HardwareFingerprintService(new WmiDataProvider(), new HashingProvider());
			keygen = new KeyGenerator(symmetricEncryptionProvider, asymmetricEncryptionProvider, hashingProvider);
			keyGeneratorLarge = new Scutex.Generators.StaticKeyGeneratorLarge.KeyGenerator(symmetricEncryptionProvider, asymmetricEncryptionProvider, hashingProvider, hardwareFingerprintService);

			licenseActiviationProvider = new LicenseActiviationProvider(asymmetricEncryptionProvider, symmetricEncryptionProvider, objectSerializationProvider);
			service = new LicenseKeyService(keygen, keyGeneratorLarge, packingService, clientLicenseService);
			productsProvider = new ProductsProvider(symmetricEncryptionProvider, objectSerializationProvider, asymmetricEncryptionProvider);
			zipCompressionProvider = new ZipCompressionProvider();
			wcfPackagingService = new WcfPackagingService(zipCompressionProvider);

			//licenseSetsRepository = new LicenseSetsRepository();

			//licenseSetService = new LicenseSetService();
			//licenseService = new LicenseService();

			License = new License();
			License.Name = "UnitTest License";
			License.UniqueId = Guid.NewGuid();
			License.KeyPair = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);

			//string path = System.Reflection.Assembly.GetAssembly(typeof(LicenseHelper)).Location;
			string path = Helper.AssemblyDirectory;

			DllHash = encodingService.Encode(hashingProvider.HashFile(path + "\\WaveTech.Scutex.Licensing.dll"));
			PublicKey = encodingService.Encode(License.KeyPair.PublicKey);
		}

		public void SetupService(int clientPort, int mgmtPort)
		{
			SingleUserSetup();

			Service = new Service();
			Service.OutboundKeyPair = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);
			Service.InboundKeyPair = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);
			Service.ManagementInboundKeyPair = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);
			Service.ManagementOutboundKeyPair = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);
			Service.Token = Guid.NewGuid().ToString();

			StringDataGenerator stringDataGenerator = new StringDataGenerator();
			Service.ClientRequestToken = stringDataGenerator.GenerateRandomString(10, 25, true, true);
			Service.ManagementRequestToken = stringDataGenerator.GenerateRandomString(10, 25, true, true);

			Service.ClientUrl = string.Format("http://localhost:{0}/", clientPort);
			Service.ManagementUrl = string.Format("http://localhost:{0}/", mgmtPort);
			Service.Name = "UnitTest Web Services";
			Service.UniquePad = Guid.NewGuid();
			Service.Initialized = false;
			Service.CreatedDate = DateTime.Now;
			Service.LockToIp = false;

			wcfPackagingService.WriteClientKeys(Helper.AssemblyDirectory + "\\WebServices\\Client", Service);
			wcfPackagingService.WriteManagementKeys(Helper.AssemblyDirectory + "\\WebServices\\Mgmt", Service);

			try
			{
				_servicesService = new ServicesService(null, serviceStatusProvider, packingService,
					licenseActiviationProvider, service, null, null, clientLicenseService, productsProvider);

				Service.Initialized = _servicesService.InitializeService(Service);

				_servicesService.AddProductToService(License, License.LicenseSets.ToList(), Service);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				throw;
			}

			License.Service = Service;
		}

		public void BasicSetup()
		{
			License.Name = "UnitTest License";

			Product p = new Product();
			p.Name = "UnitTest Product";
			p.Description = "Just a product for unit testing";

			License.LicenseId = 1;
			License.Product = p;
			License.KeyGeneratorType = KeyGeneratorTypes.StaticSmall;

			LicenseTrialSettings ts = new LicenseTrialSettings();
			ts.ExpirationOptions = TrialExpirationOptions.Days;
			ts.ExpirationData = "30";

			License.TrialSettings = ts;

			LicenseSet ls = new LicenseSet();
			ls.LicenseId = 1;
			ls.LicenseSetId = 1;
			ls.Name = "Unit Test License Set";
			ls.MaxUsers = 5;
			ls.SupportedLicenseTypes = LicenseKeyTypeFlag.Enterprise;

			License.LicenseSets = new NotifyList<LicenseSet>();
			License.LicenseSets.Add(ls);

			CreateFile(new ClientLicense(License));
		}

		public void ExpiredTrialSetup()
		{
			License.Name = "UnitTest License";

			Product p = new Product();
			p.Name = "UnitTest Product";
			p.Description = "Just a product for unit testing";

			License.LicenseId = 1;
			License.Product = p;
			License.KeyGeneratorType = KeyGeneratorTypes.StaticSmall;

			LicenseTrialSettings ts = new LicenseTrialSettings();
			ts.ExpirationOptions = TrialExpirationOptions.Days;
			ts.ExpirationData = "30";

			License.TrialSettings = ts;

			LicenseSet ls = new LicenseSet();
			ls.LicenseId = 1;
			ls.LicenseSetId = 1;
			ls.Name = "Unit Test License Set";
			ls.MaxUsers = 5;
			ls.SupportedLicenseTypes = LicenseKeyTypeFlag.Enterprise;

			License.LicenseSets = new NotifyList<LicenseSet>();
			License.LicenseSets.Add(ls);

			ClientLicense lic2 = new ClientLicense(License);
			lic2.RunCount = 10;
			lic2.LastRun = DateTime.Now.AddMonths(-12);
			lic2.FirstRun = DateTime.Now.AddMonths(-24);

			CreateFile(lic2);
		}

		public void InvalidTrialSetup()
		{
			License.Name = "UnitTest License";

			Product p = new Product();
			p.Name = "UnitTest Product";
			p.Description = "Just a product for unit testing";

			License.LicenseId = 1;
			License.Product = p;
			License.KeyGeneratorType = KeyGeneratorTypes.StaticSmall;

			LicenseTrialSettings ts = new LicenseTrialSettings();
			ts.ExpirationOptions = TrialExpirationOptions.Days;
			ts.ExpirationData = "30";

			License.TrialSettings = ts;

			LicenseSet ls = new LicenseSet();
			ls.LicenseId = 1;
			ls.LicenseSetId = 1;
			ls.Name = "Unit Test License Set";
			ls.MaxUsers = 5;
			ls.SupportedLicenseTypes = LicenseKeyTypeFlag.Enterprise;

			License.LicenseSets = new NotifyList<LicenseSet>();
			License.LicenseSets.Add(ls);

			ClientLicense lic2 = new ClientLicense(License);
			lic2.RunCount = 10;
			lic2.LastRun = DateTime.Now.AddMonths(12);
			lic2.FirstRun = DateTime.Now.AddMonths(24);

			CreateFile(lic2);
		}

		public void BrokenTrialSetup()
		{
			License.Name = "UnitTest License";

			Product p = new Product();
			p.Name = "UnitTest Product";
			p.Description = "Just a product for unit testing";

			License.LicenseId = 1;
			License.Product = p;
			License.KeyGeneratorType = KeyGeneratorTypes.StaticSmall;

			LicenseTrialSettings ts = new LicenseTrialSettings();
			ts.ExpirationOptions = TrialExpirationOptions.Days;
			ts.ExpirationData = "30";

			License.TrialSettings = ts;

			LicenseSet ls = new LicenseSet();
			ls.LicenseId = 1;
			ls.LicenseSetId = 1;
			ls.Name = "Unit Test License Set";
			ls.MaxUsers = 5;
			ls.SupportedLicenseTypes = LicenseKeyTypeFlag.Enterprise;

			License.LicenseSets = new NotifyList<LicenseSet>();
			License.LicenseSets.Add(ls);

			License lic = License;
			AsymmetricEncryptionProvider asymmetricEncryptionProvider = new AsymmetricEncryptionProvider();
			lic.KeyPair = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);

			ClientLicense lic2 = new ClientLicense(License);
			lic2.RunCount = 10;
			lic2.LastRun = DateTime.Now.AddMonths(-12);
			lic2.FirstRun = DateTime.Now.AddMonths(-24);

			CreateFile(lic2);
		}

		public void SingleUserSetup()
		{
			License.Name = "UnitTest License";

			Product p = new Product();
			p.Name = "UnitTest Product";
			p.Description = "Just a product for unit testing";

			License.LicenseId = 1;
			License.Product = p;
			License.KeyGeneratorType = KeyGeneratorTypes.StaticSmall;

			LicenseTrialSettings ts = new LicenseTrialSettings();
			ts.ExpirationOptions = TrialExpirationOptions.Days;
			ts.ExpirationData = "30";

			License.TrialSettings = ts;

			LicenseSet ls = new LicenseSet();
			ls.LicenseId = 1;
			ls.LicenseSetId = 1;
			ls.Name = "Unit Test License Set";
			ls.MaxUsers = 5;
			ls.SupportedLicenseTypes = LicenseKeyTypeFlag.SingleUser;

			License.LicenseSets = new NotifyList<LicenseSet>();
			License.LicenseSets.Add(ls);

			CreateFile(new ClientLicense(License));
		}

		public void HardwareUserSetup()
		{
			License.Name = "UnitTest Hardware License";

			Product p = new Product();
			p.Name = "UnitTest Hardware Product";
			p.Description = "Just a hardware product for unit testing";

			License.LicenseId = 1;
			License.Product = p;
			License.KeyGeneratorType = KeyGeneratorTypes.StaticLarge;

			LicenseTrialSettings ts = new LicenseTrialSettings();
			ts.ExpirationOptions = TrialExpirationOptions.Days;
			ts.ExpirationData = "30";

			License.TrialSettings = ts;

			LicenseSet ls = new LicenseSet();
			ls.LicenseId = 1;
			ls.LicenseSetId = 1;
			ls.Name = "Unit Test Hardware License Set";
			ls.MaxUsers = 5;
			ls.SupportedLicenseTypes = LicenseKeyTypeFlag.HardwareLock;

			License.LicenseSets = new NotifyList<LicenseSet>();
			License.LicenseSets.Add(ls);

			CreateFile(new ClientLicense(License));
		}

		public void CreateFile(ClientLicense license)
		{
			string path = Helper.AssemblyDirectory;

			if (File.Exists(path + "\\sxu.dll"))
				File.Delete(path + "\\sxu.dll");

			clientLicenseService.SaveClientLicense(license, path + "\\sxu.dll");
		}

		public string GenerateLicenseKey(LicenseKeyTypes keyTypes)
		{
			LicenseGenerationOptions options = new LicenseGenerationOptions();
			options.LicenseKeyType = keyTypes;

			List<string> keys = service.GenerateLicenseKeys(License.KeyPair.PrivateKey,
																											License,
																											options,
																											1);

			return keys.First();
		}

		public void PushKeyToService(string key)
		{
			List<string> keys = new List<string>();
			keys.Add(hashingProvider.ComputeHash(key, "SHA256"));

			_servicesService.AddLicenseKeysToService(License.LicenseSets.First(), Service, keys);
		}

		public void DeleteFile()
		{
			string path = Helper.AssemblyDirectory;

			if (File.Exists(path + "\\sxu.dll"))
				File.Delete(path + "\\sxu.dll");
		}
	}
}