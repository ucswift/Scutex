using StructureMap.Configuration.DSL;
using WaveTech.Scutex.Model.Interfaces.Providers;

namespace WaveTech.Scutex.Providers.AsymmetricEncryptionProvider
{
	internal class ProviderRegistry : Registry
	{
		public ProviderRegistry()
		{
			//ForRequestedType<IAsymmetricEncryptionProvider>().TheDefault.Is.OfConcreteType<AsymmetricEncryptionProvider>();
			For<IAsymmetricEncryptionProvider>().Use<AsymmetricEncryptionProvider>();
		}
	}
}