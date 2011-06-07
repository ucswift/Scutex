using StructureMap.Configuration.DSL;
using WaveTech.Scutex.Model.Interfaces.Repositories;

namespace WaveTech.Scutex.Repositories.ClientDataRepository
{
	internal class DataRegistry : Registry
	{
		public DataRegistry()
		{
			//ForRequestedType<IClientLicenseRepository>().TheDefault.Is.OfConcreteType<ClientLicenseRepository>();
			For<IClientLicenseRepository>().Use<ClientLicenseRepository>();
		}
	}
}