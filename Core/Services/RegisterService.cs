using System;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Services
{
	public class RegisterService : IRegisterService
	{
		private readonly ILicenseKeyService _licenseKeyService;
		private readonly ILicenseActivationService _licenseActivationService;
		private readonly IHardwareFingerprintService _hardwareFingerprintService;

		public RegisterService(ILicenseKeyService licenseKeyService, ILicenseActivationService licenseActivationService, IHardwareFingerprintService hardwareFingerprintService)
		{
			_licenseKeyService = licenseKeyService;
			_licenseActivationService = licenseActivationService;
			_hardwareFingerprintService = hardwareFingerprintService;
		}

		public RegisterResult Register(string licenseKey, LicenseBase scutexLicense, ScutexLicense license)
		{
			RegisterResult registerResult = new RegisterResult();
			registerResult.ScutexLicense = license;

			bool result = _licenseKeyService.ValidateLicenseKey(licenseKey, scutexLicense, true);
			ClientLicense cl = null;

			if (result)
			{
				KeyData keyData = _licenseKeyService.GetLicenseKeyData(licenseKey, scutexLicense, false);

				if (keyData.LicenseKeyType != LicenseKeyTypes.Enterprise && keyData.LicenseKeyType != LicenseKeyTypes.HardwareLockLocal)
				{
					try
					{
						if (keyData.LicenseKeyType == LicenseKeyTypes.HardwareLock)
							cl = _licenseActivationService.ActivateLicenseKey(licenseKey, null, false, (ClientLicense)scutexLicense, _hardwareFingerprintService.GetHardwareFingerprint(FingerprintTypes.Default));
						else
							cl = _licenseActivationService.ActivateLicenseKey(licenseKey, null, false, (ClientLicense)scutexLicense, null);

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
					catch
					{
						registerResult.Result = ProcessCodes.ActivationFailed;
					}
				}
				else
				{
					cl = _licenseActivationService.ActivateLicenseKey(licenseKey, null, true, (ClientLicense)scutexLicense, null);
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