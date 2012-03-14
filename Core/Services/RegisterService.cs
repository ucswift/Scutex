using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Services
{
	public class RegisterService : IRegisterService
	{
		private readonly ILicenseKeyService _licenseKeyService;
		private readonly ILicenseActivationService _licenseActivationService;

		public RegisterService(ILicenseKeyService licenseKeyService, ILicenseActivationService licenseActivationService)
		{
			_licenseKeyService = licenseKeyService;
			_licenseActivationService = licenseActivationService;
		}

		public RegisterResult Register(string licenseKey, LicenseBase scutexLicense, ScutexLicense license)
		{
			RegisterResult registerResult = new RegisterResult();
			registerResult.ScutexLicense = license;

			bool result = _licenseKeyService.ValidateLicenseKey(licenseKey, scutexLicense, true);
			ClientLicense cl = null;

			if (result)
			{
				KeyData keyData = _licenseKeyService.GetLicenseKeyData(licenseKey, scutexLicense);

				if (keyData.LicenseKeyType != LicenseKeyTypes.Enterprise && keyData.LicenseKeyType != LicenseKeyTypes.HardwareLockLocal)
				{
					cl = _licenseActivationService.ActivateLicenseKey(licenseKey, null, false, (ClientLicense)scutexLicense);

					if (cl.IsLicensed && cl.IsActivated)
					{
						registerResult.Result = ProcessCodes.ActivationSuccess;
						registerResult.ClientLicense = cl;
					}
					else
					{
						registerResult.Result = ProcessCodes.ActivationFailed;
					}
				}
				else
				{
					cl = _licenseActivationService.ActivateLicenseKey(licenseKey, null, true, (ClientLicense)scutexLicense);
					registerResult.Result = ProcessCodes.LicenseKeyNotActivated;
				}
			}
			else
			{
				registerResult.Result = ProcessCodes.KeyInvalid;
			}

			if (cl != null)
			{
				license.IsLicensed = cl.IsLicensed;
				license.IsActivated = cl.IsActivated;
				license.ActivatedOn = cl.ActivatedOn;
			}

			return registerResult;
		}
	}
}