using System;

namespace WaveTech.Scutex.Model.Interfaces.Services
{
	public interface ILicenseActivationService
	{
		ClientLicense ActivateLicenseKey(string licenseKey, Guid? token, bool isOffline, ClientLicense scutexLicense);
	}
}
