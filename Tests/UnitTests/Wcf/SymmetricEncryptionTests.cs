using System.IO;
using System.Reflection;
using System.Web.Hosting.Moles;
using NUnit.Framework;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Providers.AsymmetricEncryptionProvider;
using WaveTech.Scutex.Providers.DataGenerationProvider;
using WaveTech.Scutex.Providers.ObjectSerialization;
using WaveTech.Scutex.Providers.SymmetricEncryptionProvider;
using WaveTech.Scutex.Providers.WebServicesProvider;
using WaveTech.Scutex.Repositories.ManagerDataRepository;
using WaveTech.Scutex.Repositories.ServicesDataRepository;
using WaveTech.Scutex.Services;
using WaveTech.Scutex.WcfServices.ServicesLibrary.Services;

namespace WaveTech.Scutex.UnitTests.Wcf
{
	namespace SymmetricEncryptionTests
	{
		public class with_the_services_and_wcf_system : FixtureBase
		{
			internal ServicesService servicesService;
			protected ControlService controlService;
			protected Scutex.Model.Service service;

			protected override void Before_each_test()
			{
				base.Before_each_test();

				ServicesRepository servicesRepository = new ServicesRepository(new ScutexEntities());
				CommonRepository commonRepository = new CommonRepository(new ScutexServiceEntities());

				AsymmetricEncryptionProvider asymmetricEncryptionProvider = new AsymmetricEncryptionProvider();
				SymmetricEncryptionProvider symmetricEncryptionProvider = new SymmetricEncryptionProvider();
				ObjectSerializationProvider objectSerializationProvider = new ObjectSerializationProvider();
				NumberDataGenerator numberDataGenerator = new NumberDataGenerator();
				PackingService packingService = new PackingService(numberDataGenerator);
				MasterService masterService = new MasterService(commonRepository);

				CommonService commonService = new CommonService();
				KeyPairService keyPairService = new KeyPairService(commonService, commonRepository);

				ServiceStatusProvider serviceStatusProvider = new ServiceStatusProvider(symmetricEncryptionProvider, objectSerializationProvider, asymmetricEncryptionProvider);
				servicesService = new ServicesService(servicesRepository, serviceStatusProvider, packingService, null, null, null, null, null, null);


				controlService = new ControlService(symmetricEncryptionProvider, keyPairService, packingService, masterService, objectSerializationProvider, asymmetricEncryptionProvider);

				service = new Scutex.Model.Service();
				service.OutboundKeyPair = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);
				service.InboundKeyPair = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);
				service.ManagementInboundKeyPair = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);
				service.ManagementOutboundKeyPair = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);
			}
		}

		[TestFixture]
		public class when_encrypting_for_symmetric_communication : with_the_services_and_wcf_system
		{
			[Test]
			public void encryption_infos_should_match()
			{
				string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
				path = path.Replace("file:\\", "");

				MHostingEnvironment.ApplicationPhysicalPathGet = () => path;

				string inFile = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\999900000-IN.config";
				string outFile = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\999900000-OUT.config";

				if (File.Exists(inFile))
					File.Delete(inFile);

				if (File.Exists(outFile))
					File.Delete(outFile);


				string inKey1 = service.InboundKeyPair.PublicKey.Substring(0, (service.InboundKeyPair.PublicKey.Length / 2));
				string inKey2 = service.InboundKeyPair.PublicKey.Substring(inKey1.Length, (service.InboundKeyPair.PublicKey.Length / 2));

				string outKey1 = service.OutboundKeyPair.PrivateKey.Substring(0, (service.OutboundKeyPair.PublicKey.Length / 2));
				string outKey2 = service.OutboundKeyPair.PrivateKey.Substring(outKey1.Length, (service.OutboundKeyPair.PublicKey.Length / 2));

				using (StreamWriter writer = new StreamWriter(inFile))
				{
					writer.WriteLine(inKey2);
				}

				using (StreamWriter writer = new StreamWriter(outFile))
				{
					writer.WriteLine(outKey2);
				}


				EncryptionInfo ei1 = servicesService.GetManagementStandardEncryptionInfo(service);
				EncryptionInfo ei2 = controlService.GetStandardEncryptionInfo();



				//Assert.IsNotNullOrEmpty(accountNumber);
			}
		}
	}
}