using StructureMap.Configuration.DSL;
using WaveTech.Scutex.Model.Interfaces.Providers;

namespace WaveTech.Scutex.Providers.DatabaseUpdateProvider
{
	internal class ProviderRegistry : Registry
	{
		public ProviderRegistry()
		{
			//ForRequestedType<IDatabaseUpdateProvider>().TheDefault.Is.OfConcreteType<DatabaseUpdater>();
			For<IDatabaseUpdateProvider>().Use<DatabaseUpdater>();
		}
	}
}