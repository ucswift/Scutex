using StructureMap.Configuration.DSL;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Services
{
	internal class ServicesRegistry : Registry
	{
		public ServicesRegistry()
		{
			//ForRequestedType<IEncodingService>().TheDefault.Is.OfConcreteType<EncodingService>();
			For<IEncodingService>().Use<EncodingService>();

			//ForRequestedType<IHardwareFingerprintService>().TheDefault.Is.OfConcreteType<HardwareFingerprintService>();
			For<IHardwareFingerprintService>().Use<HardwareFingerprintService>();

			//ForRequestedType<ILicenseKeyService>().TheDefault.Is.OfConcreteType<LicenseKeyService>();
			For<ILicenseKeyService>().Use<LicenseKeyService>();

			//ForRequestedType<IProductsService>().TheDefault.Is.OfConcreteType<ProductsService>();
			For<IProductsService>().Use<ProductsService>();

			//ForRequestedType<ILicenseService>().TheDefault.Is.OfConcreteType<LicenseService>();
			For<ILicenseService>().Use<LicenseService>();

			//ForRequestedType<IKeyExportService>().TheDefault.Is.OfConcreteType<KeyExportService>();
			For<IKeyExportService>().Use<KeyExportService>();

			//ForRequestedType<ILicenseSetService>().TheDefault.Is.OfConcreteType<LicenseSetService>();
			For<ILicenseSetService>().Use<LicenseSetService>();

			//ForRequestedType<IClientLicenseService>().TheDefault.Is.OfConcreteType<ClientLicenseService>();
			For<IClientLicenseService>().Use<ClientLicenseService>();

			//ForRequestedType<ILicenseManagerService>().TheDefault.Is.OfConcreteType<LicenseManagerService>();
			For<ILicenseManagerService>().Use<LicenseManagerService>();

			//ForRequestedType<IServicesService>().TheDefault.Is.OfConcreteType<ServicesService>();
			For<IServicesService>().Use<ServicesService>();

			//ForRequestedType<IPackingService>().TheDefault.Is.OfConcreteType<PackingService>();
			For<IPackingService>().Use<PackingService>();

			//ForRequestedType<ILicenseActivationService>().TheDefault.Is.OfConcreteType<LicenseActivationService>();
			For<ILicenseActivationService>().Use<LicenseActivationService>();

			//ForRequestedType<IValidationService>().TheDefault.Is.OfConcreteType<ValidationService>();
			For<IValidationService>().Use<ValidationService>();

			For<IKeyService>().Use<KeyService>();
			For<IReportingService>().Use<ReportingService>();
			For<IWcfPackagingService>().Use<WcfPackagingService>();
			For<IComApiWrappingService>().Use<ComApiWrappingService>();
			For<IConsoleInteractionService>().Use<ConsoleInteractionService>();
			For<IEventLogInteractionService>().Use<EventLogInteractionService>();
			For<IRegisterService>().Use<RegisterService>();
		}
	}
}