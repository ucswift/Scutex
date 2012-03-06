using System.Collections.Generic;

namespace WaveTech.Scutex.Model.Interfaces.Services
{
	public interface IServicesService
	{
		List<Service> GetAllServices();
		Service SaveService(Service service);
		List<Service> GetAllNonInitializedServices();
		Service GetServiceById(int serviceId);
		void DeleteServiceById(int serviceId);
		bool InitializeService(Service service);
		bool TestService(Service service);
		List<Service> GetAllNonInitializedNonTestedServices();
		List<Service> GetAllInitializedActiveServices();
		Dictionary<License, List<LicenseSet>> GetServiceLicenses(Service service);
		bool AddProductToService(License license, List<LicenseSet> licenseSets, Service service);
		List<string> GetServiceLicenseKeysForSet(LicenseSet licenseSet, Service service);
		bool AddLicenseKeysToService(LicenseSet licenseSet, Service service, List<string> keys);
		bool TestClientServiceUrl(Service service);
		bool TestManagementServiceUrl(Service service);
		bool TestClientServiceFileSystem(Service service);
		bool TestManagementServiceFileSystem(Service service);
		bool TestClientServiceDatabase(Service service);
		bool TestManagementServiceDatabase(Service service);
	}
}