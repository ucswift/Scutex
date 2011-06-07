using System.Collections.Generic;

namespace WaveTech.Scutex.Model.Interfaces.Services
{
	public interface ILicenseKeyService
	{
		string GenerateLicenseKey(string rsaXmlString, LicenseBase licenseinfo, LicenseGenerationOptions generationOptions);
		List<string> GenerateLicenseKeys(string rsaXmlString, LicenseBase scutexLicense, LicenseGenerationOptions generationOptions, int count);
		bool ValidateLicenseKey(string licenseKey, LicenseBase scutexLicense, bool catchException);
		KeyData GetLicenseKeyData(string licenseKey, LicenseBase scutexLicense);
	}
}