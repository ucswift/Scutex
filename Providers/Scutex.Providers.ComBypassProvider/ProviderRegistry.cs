using StructureMap.Configuration.DSL;
using WaveTech.Scutex.Model.Interfaces.Providers;

namespace WaveTech.Scutex.Providers.ComBypassProvider
{
	internal class ProviderRegistry : Registry
	{
		public ProviderRegistry()
		{
			For<IComBypassProvider>().Use<ComBypass>();
		}
	}
}