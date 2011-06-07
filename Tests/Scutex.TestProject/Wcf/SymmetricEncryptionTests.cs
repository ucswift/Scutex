using System.IO;
using System.Reflection;
using System.Web.Hosting.Moles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

namespace Scutex.TestProject.Wcf
{
	[TestClass]
	public class SymmetricEncryptionTests
	{
		internal ServicesService servicesService;
		protected ControlService controlService;
		protected WaveTech.Scutex.Model.Service service;

		[TestMethod]
		[HostType("Moles")]
		public void management_encryption_infos_should_match()
		{

			ServicesRepository servicesRepository = new ServicesRepository(new ScutexEntities());
			CommonRepository commonRepository = new CommonRepository(new ScutexServiceEntities());

			AsymmetricEncryptionProvider asymmetricEncryptionProvider = new AsymmetricEncryptionProvider();
			SymmetricEncryptionProvider symmetricEncryptionProvider = new SymmetricEncryptionProvider();
			ObjectSerializationProvider objectSerializationProvider = new ObjectSerializationProvider();
			NumberDataGenerator numberDataGenerator = new NumberDataGenerator();
			PackingService packingService = new PackingService(numberDataGenerator);
			MasterService masterService = new MasterService(commonRepository);

			ServiceStatusProvider serviceStatusProvider = new ServiceStatusProvider(symmetricEncryptionProvider, objectSerializationProvider, asymmetricEncryptionProvider);
			servicesService = new ServicesService(servicesRepository, serviceStatusProvider, packingService, null, null, null, null, null, null);


			controlService = new ControlService(symmetricEncryptionProvider, null, packingService, masterService, objectSerializationProvider, asymmetricEncryptionProvider);

			service = new WaveTech.Scutex.Model.Service();
			service.OutboundKeyPair = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);
			service.InboundKeyPair = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);
			service.ManagementInboundKeyPair = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);
			service.ManagementOutboundKeyPair = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);

			string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
			path = path.Replace("file:\\", "");

			MHostingEnvironment.ApplicationPhysicalPathGet = () => path;

			string inFile = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\999900000-IN.config";
			string outFile = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath + "\\999900000-OUT.config";

			if (File.Exists(inFile))
				File.Delete(inFile);

			if (File.Exists(outFile))
				File.Delete(outFile);


			string inKey1 = service.ManagementInboundKeyPair.PublicKey.Substring(0, (service.ManagementInboundKeyPair.PublicKey.Length / 2));
			string inKey2 = service.ManagementInboundKeyPair.PublicKey.Substring(inKey1.Length, (service.ManagementInboundKeyPair.PublicKey.Length / 2));

			string outKey1 = service.ManagementOutboundKeyPair.PrivateKey.Substring(0, (service.ManagementOutboundKeyPair.PublicKey.Length / 2));
			string outKey2 = service.ManagementOutboundKeyPair.PrivateKey.Substring(outKey1.Length, (service.ManagementOutboundKeyPair.PublicKey.Length / 2));

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

			Assert.IsTrue(ei1.Equals(ei2));
		}
	}
}
