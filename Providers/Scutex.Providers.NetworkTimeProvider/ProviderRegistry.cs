using StructureMap.Configuration.DSL;
using WaveTech.Scutex.Model.Interfaces.Providers;

namespace WaveTech.Scutex.Providers.NetworkTimeProvider
{
	internal class ProviderRegistry : Registry
	{
		public ProviderRegistry()
		{
			//ForRequestedType<INetworkTimeProvider>().TheDefault.Is.OfConcreteType<NtpProvider>();
			For<INetworkTimeProvider>().Use<NtpProvider>();
		}
	}
}