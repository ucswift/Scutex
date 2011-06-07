using StructureMap.Configuration.DSL;
using WaveTech.Scutex.Model.Interfaces.Repositories;

namespace WaveTech.Scutex.Repositories.ManagerDataRepository
{
	internal class DataRegistry : Registry
	{
		public DataRegistry()
		{
			//ForRequestedType<ScutexEntities>().TheDefault.IsThis(new ScutexEntities());
			For<ScutexEntities>().Singleton().Use(new ScutexEntities());

			//ForRequestedType<IProductsRepository>().TheDefault.Is.OfConcreteType<ProductsRepository>();
			For<IProductsRepository>().Use<ProductsRepository>();

			//ForRequestedType<ILicensesRepository>().TheDefault.Is.OfConcreteType<LicensesRepository>();
			For<ILicensesRepository>().Use<LicensesRepository>();

			//ForRequestedType<IFeaturesRepository>().TheDefault.Is.OfConcreteType<FeaturesRepository>();
			For<IFeaturesRepository>().Use<FeaturesRepository>();

			//ForRequestedType<ILicenseSetsRepository>().TheDefault.Is.OfConcreteType<LicenseSetsRepository>();
			For<ILicenseSetsRepository>().Use<LicenseSetsRepository>();

			//ForRequestedType<ITrialSettingsRepository>().TheDefault.Is.OfConcreteType<TrialSettingsRepository>();
			For<ITrialSettingsRepository>().Use<TrialSettingsRepository>();

			//ForRequestedType<IServicesRepository>().TheDefault.Is.OfConcreteType<ServicesRepository>();
			For<IServicesRepository>().Use<ServicesRepository>();

			For<ILicenseKeyRepository>().Use<LicenseKeyRepository>();
		}
	}
}