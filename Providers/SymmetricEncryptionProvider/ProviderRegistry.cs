using StructureMap.Configuration.DSL;
using WaveTech.Scutex.Model.Interfaces.Providers;

namespace WaveTech.Scutex.Providers.SymmetricEncryptionProvider
{
	internal class ProviderRegistry : Registry
	{
		public ProviderRegistry()
		{
			//ForRequestedType<ISymmetricEncryptionProvider>().TheDefault.Is.OfConcreteType<SymmetricEncryptionProvider>();
			For<ISymmetricEncryptionProvider>().Use<SymmetricEncryptionProvider>();
		}
	}
}