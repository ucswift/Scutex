using StructureMap.Configuration.DSL;
using WaveTech.Scutex.Model.Interfaces.Repositories;

namespace WaveTech.Scutex.Repositories.ServicesDataRepository
{
	internal class ServicesDataRegistry : Registry
	{
		public ServicesDataRegistry()
		{
			//ForRequestedType<ScutexServiceEntities>().TheDefault.IsThis(new ScutexServiceEntities());
			For<ScutexServiceEntities>().Use(new ScutexServiceEntities());

			//ForRequestedType<ICommonRepository>().TheDefault.Is.OfConcreteType<CommonRepository>();
			For<ICommonRepository>().Use<CommonRepository>();

			//ForRequestedType<IClientRepository>().TheDefault.Is.OfConcreteType<ClientRepository>();
			For<IClientRepository>().Use<ClientRepository>();

			//ForRequestedType<IServiceProductsRepository>().TheDefault.Is.OfConcreteType<ServiceProductsRepository>();
			For<IServiceProductsRepository>().Use<ServiceProductsRepository>();

			//ForRequestedType<IActivationLogRepoistory>().TheDefault.Is.OfConcreteType<ActivationLogRepoistory>();
			For<IActivationLogRepoistory>().Use<ActivationLogRepoistory>();

		}
	}
}