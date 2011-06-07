using StructureMap.Configuration.DSL;
using WaveTech.Scutex.Model.Interfaces.Providers;

namespace WaveTech.Scutex.Providers.DataGenerationProvider
{
	internal class ProviderRegistry : Registry
	{
		public ProviderRegistry()
		{
			//ForRequestedType<IStringDataGeneratorProvider>().TheDefault.Is.OfConcreteType<StringDataGenerator>();
			For<IStringDataGeneratorProvider>().Use<StringDataGenerator>();

			//ForRequestedType<INumberDataGeneratorProvider>().TheDefault.Is.OfConcreteType<NumberDataGenerator>();
			For<INumberDataGeneratorProvider>().Use<NumberDataGenerator>();
		}
	}
}