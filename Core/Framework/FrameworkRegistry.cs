using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;
using WaveTech.Scutex.Model.Interfaces.Framework;

namespace WaveTech.Scutex.Framework
{
	internal class FrameworkRegistry : Registry
	{
		public FrameworkRegistry()
		{
			//ForRequestedType<IEventAggregator>().TheDefaultIsConcreteType<EventAggregator>().CacheBy(InstanceScope.Singleton);
			For<IEventAggregator>().LifecycleIs(new SingletonLifecycle()).Use<EventAggregator>();
		}
	}
}