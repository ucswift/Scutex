using StructureMap.Configuration.DSL;
using WaveTech.Scutex.Model.Interfaces.Providers;

namespace WaveTech.Scutex.Providers.WmiDataProvider
{
	internal class ProviderRegistry : Registry
	{
		public ProviderRegistry()
		{
			//ForRequestedType<IWmiDataProvider>().TheDefault.Is.OfConcreteType<WmiDataProvider>();
			For<IWmiDataProvider>().Use<WmiDataProvider>();
		}
	}
}