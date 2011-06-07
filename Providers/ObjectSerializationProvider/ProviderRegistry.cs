using StructureMap.Configuration.DSL;
using WaveTech.Scutex.Model.Interfaces.Providers;

namespace WaveTech.Scutex.Providers.ObjectSerialization
{
	internal class ProviderRegistry : Registry
	{
		public ProviderRegistry()
		{
			//ForRequestedType<IObjectSerializationProvider>().TheDefault.Is.OfConcreteType<ObjectSerializationProvider>();
			For<IObjectSerializationProvider>().Use<ObjectSerializationProvider>();
		}
	}
}