using System.Collections.Generic;

namespace WaveTech.Scutex.Model.Interfaces.Services
{
	public interface ILicenseService
	{
		bool IsProductIdUsed(int productId);
		License SaveLicense(License license);
		bool IsLicenseProjectNameInUse(string name);
		List<License> GetLast10Licenses();
		License GetLicenseById(int licenseId);
		List<License> GetAllLicenses();
		Dictionary<License, List<LicenseSet>> GetAllLicensesAndSets();
	}
}