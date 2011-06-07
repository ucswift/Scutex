using StructureMap.Configuration.DSL;
using WaveTech.Scutex.Model.Interfaces.Providers;

namespace WaveTech.Scutex.Providers.CompressionProvider
{
	internal class ProviderRegistry : Registry
	{
		public ProviderRegistry()
		{
			//ForRequestedType<IZipCompressionProvider>().TheDefault.Is.OfConcreteType<ZipCompressionProvider>();
			For<IZipCompressionProvider>().Use<ZipCompressionProvider>();
		}
	}
}