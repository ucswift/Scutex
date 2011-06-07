using System.Linq;

namespace WaveTech.Scutex.Model.Interfaces.Repositories
{
	public interface IServicesRepository
	{
		IQueryable<Service> GetAllServices();
		IQueryable<Service> GetServiceById(int serviceId);
		IQueryable<Service> InsertService(Service service);
		IQueryable<Service> UpdateService(Service service);
		void DeleteServiceById(int serviceId);
	}
}