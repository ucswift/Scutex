using System.Collections.Generic;

namespace WaveTech.Scutex.Model.Interfaces.Repositories
{
	public interface IServiceProductsRepository
	{
		List<ServiceProduct> GetAllProducts();
		ServiceProduct GetProduct(int licenseId);
		List<ServiceLicenseSet> GetAllServiceLicenseSets();
		List<ServiceLicenseSet> GetServiceLicenseSetsByProduct(int licenseId);
		List<ServiceLicenseKey> GetAllServiceLicenseKeys();
		List<ServiceLicenseKey> GetServiceLicenseKeysByLicenseSet(int licenseSetId);
		ServiceLicenseSet GetServiceLicenseSetById(int licenseSetId);
		ServiceLicenseSet SaveServiceLicenseSet(ServiceLicenseSet licenseSet);
		ServiceProduct SaveServiceProduct(ServiceProduct product);
		ServiceLicenseKey GetServiceLicenseKeyByKey(string key);
		ServiceLicenseKey GetServiceLicenseKeyById(int licenseKeyId);
		ServiceLicenseKey SaveServiceLicenseKey(ServiceLicenseKey serviceLicenseKey);
		void DeleteLicenseActivationsByKeyId(int licenseKeyId);
		void DeleteLicenseKeyByKey(string licenseKey);
		void DeleteLicenseKeyByLicenseSetId(int licenseSetId);
		void DeleteLicenseSetId(int licenseSetId);
		void DeleteLicenseId(int licenseId);
		ServiceLicenseKey GetServiceLicenseKeyByKeyLicenseSet(string key, int licenseSetId);
	}
}