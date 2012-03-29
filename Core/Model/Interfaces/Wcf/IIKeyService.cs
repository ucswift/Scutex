using System;
using System.Collections.Generic;

namespace WaveTech.Scutex.Model.Interfaces.Wcf
{
	public interface IKeyManagementService
	{
		bool DoesKeyExistForLicenseSet(string licenseKey, int licenseSetId);
		bool IsKeyAvialable(string licenseKey, int licenseSetId, string hardwareFingerprint);
		bool AuthorizeLicenseForActivation(string licenseKey, ServiceLicense licenseBase, string hardwareFingerprint);
		Guid? ActivateLicenseKey(string licenseKey, Guid? originalToken, ServiceLicense licenseBase, string hardwareFingerprint);
		List<string> GetKeysForLicenseSet(int licenseSetId);
		void AddLicenseKeysForLicenseSet(int licenseSetId, List<string> licenseKeys);
		List<LicenseActivation> GetAllLicenseActivations();
	}
}
