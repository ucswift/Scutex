using StructureMap.Configuration.DSL;
using WaveTech.Scutex.Model.Interfaces.Providers;

namespace WaveTech.Scutex.Providers.HashingProvider
{
	internal class ProviderRegistry : Registry
	{
		public ProviderRegistry()
		{
			//ForRequestedType<IHashingProvider>().TheDefault.Is.OfConcreteType<HashingProvider>();
			For<IHashingProvider>().Use<HashingProvider>();
		}
	}
}