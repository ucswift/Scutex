using StructureMap.Configuration.DSL;
using WaveTech.Scutex.Model.Interfaces.Providers;

namespace WaveTech.Scutex.Providers.WebServicesProvider
{
	internal class ProviderRegistry : Registry
	{
		public ProviderRegistry()
		{
			//ForRequestedType<ILicenseActiviationProvider>().TheDefault.Is.OfConcreteType<LicenseActiviationProvider>();
			For<ILicenseActiviationProvider>().Use<LicenseActiviationProvider>();

			//ForRequestedType<IServiceStatusProvider>().TheDefault.Is.OfConcreteType<ServiceStatusProvider>();
			For<IServiceStatusProvider>().Use<ServiceStatusProvider>();

			//ForRequestedType<IProductsProvider>().TheDefault.Is.OfConcreteType<ProductsProvider>();
			For<IProductsProvider>().Use<ProductsProvider>();
			For<IReportingProvider>().Use<ReportingProvider>();
		}
	}
}