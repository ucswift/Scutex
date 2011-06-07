using StructureMap.Configuration.DSL;
using WaveTech.Scutex.Model.Interfaces.Generators;

namespace WaveTech.Scutex.Generators.StaticKeyGeneratorSmall
{
	internal class GeneratorRegistry : Registry
	{
		public GeneratorRegistry()
		{
			//ForRequestedType<IKeyGenerator>().TheDefault.Is.OfConcreteType<KeyGenerator>().WithName("StaticSmallKeyGenerator");
			For<IKeyGenerator>().Use<KeyGenerator>().Named("StaticSmallKeyGenerator");
		}
	}
}