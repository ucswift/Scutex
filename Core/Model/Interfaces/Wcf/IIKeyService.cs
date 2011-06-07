using System;
using System.Collections.Generic;

namespace WaveTech.Scutex.Model.Interfaces.Wcf
{
	public interface IKeyManagementService
	{
		bool DoesKeyExistForLicenseSet(string licenseKey, int licenseSetId);
		bool IsKeyAvialable(string licenseKey, int licenseSetId);
		bool AuthorizeLicenseForActivation(string licenseKey, ServiceLicense licenseBase);
		Guid? ActivateLicenseKey(string licenseKey, Guid? originalToken, ServiceLicense licenseBase);
		List<string> GetKeysForLicenseSet(int licenseSetId);
		void AddLicenseKeysForLicenseSet(int licenseSetId, List<string> licenseKeys);
		List<LicenseActivation> GetAllLicenseActivations();
	}
}
