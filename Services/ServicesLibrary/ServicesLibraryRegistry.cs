using StructureMap.Configuration.DSL;
using WaveTech.Scutex.Model.Interfaces.Wcf;
using WaveTech.Scutex.WcfServices.ServicesLibrary.Services;

namespace WaveTech.Scutex.WcfServices.ServicesLibrary
{
	public class ServicesLibraryRegistry : Registry
	{
		public ServicesLibraryRegistry()
		{
			//ForRequestedType<IKeyManagementService>().TheDefault.Is.OfConcreteType<KeyManagementService>();
			For<IKeyManagementService>().Use<KeyManagementService>();

			//ForRequestedType<IMasterService>().TheDefault.Is.OfConcreteType<MasterService>();
			For<IMasterService>().Use<MasterService>();

			//ForRequestedType<IControlService>().TheDefault.Is.OfConcreteType<ControlService>();
			For<IControlService>().Use<ControlService>();

			//ForRequestedType<IProductManagementService>().TheDefault.Is.OfConcreteType<ProductManagementService>();
			For<IProductManagementService>().Use<ProductManagementService>();

			//ForRequestedType<ICommonService>().TheDefault.Is.OfConcreteType<CommonService>();
			For<ICommonService>().Use<CommonService>();

			//ForRequestedType<IKeyPairService>().TheDefault.Is.OfConcreteType<KeyPairService>();
			For<IKeyPairService>().Use<KeyPairService>();

			//ForRequestedType<IActivationLogService>().TheDefault.Is.OfConcreteType<ActivationLogService>();
			For<IActivationLogService>().Use<ActivationLogService>();

		}
	}
}