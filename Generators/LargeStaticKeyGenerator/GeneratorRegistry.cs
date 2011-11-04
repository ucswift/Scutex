using StructureMap.Configuration.DSL;
using WaveTech.Scutex.Model.Interfaces.Generators;

namespace WaveTech.Scutex.Generators.StaticKeyGeneratorLarge
{
	internal class GeneratorRegistry : Registry
	{
		public GeneratorRegistry()
		{
			For<IKeyGenerator>().Use<KeyGenerator>().Named("StaticLargeKeyGenerator");
		}
	}
}