using System.Collections.Generic;

namespace WaveTech.Scutex.Model.Interfaces.Services
{
	public interface IKeyService
	{
		List<string> GetAllLicenseKeysByLicenseSet(int licenseSetId);
		List<string> GetAllHashedLicenseKeysByLicenseSet(int licenseSetId);
		void SaveLicenseKeysForLicenseSet(LicenseSet licenseSet, List<string> keys);
	}
}