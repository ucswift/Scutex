using System;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Services;
using WaveTech.Scutex.Model.Results;
using WaveTech.Scutex.Services.Properties;

namespace WaveTech.Scutex.Services
{
	internal class LicenseActivationService : ILicenseActivationService
	{
		private readonly ILicenseKeyService _licenseKeyService;
		private readonly IPackingService _packingService;
		private readonly ILicenseActiviationProvider _licenseActiviationProvider;
		private readonly IClientLicenseService _clientLicenseService;

		public LicenseActivationService(ILicenseKeyService licenseKeyService, IPackingService packingService, ILicenseActiviationProvider licenseActiviationProvider, IClientLicenseService clientLicenseService)
		{
			_licenseKeyService = licenseKeyService;
			_packingService = packingService;
			_licenseActiviationProvider = licenseActiviationProvider;
			_clientLicenseService = clientLicenseService;
		}

		internal EncryptionInfo GetClientStandardEncryptionInfo(ClientLicense clientLicense)
		{
			EncryptionInfo ei = new EncryptionInfo();
			ei.HashAlgorithm = "SHA1";
			ei.InitVector = Resources.ServicesIV;
			ei.Iterations = 2;
			ei.KeySize = 192;
			ei.PassPhrase = clientLicense.Ces1;
			ei.SaltValue = clientLicense.Ces2;

			return ei;
		}

		public ClientLicense ActivateLicenseKey(string licenseKey, Guid? token, bool isOffline, ClientLicense scutexLicense)
		{
			/* This method used to live in the LicenseKeyService class, where it should be
			 *  but because of a circular reference in the WebServicesProvider and the ServicesLibrary
			 *  project requiring the LicenseKeyService to valid keys it was causing and error and had 
			 *  to be moved here.
			 */

			if (_licenseKeyService.ValidateLicenseKey(licenseKey, scutexLicense, true))
			{
				Token t = new Token();
				t.Data = scutexLicense.ServiceToken;
				t.Timestamp = DateTime.Now;

				string packedToken = _packingService.PackToken(t);

				LicenseActivationPayload payload = new LicenseActivationPayload();
				payload.LicenseKey = licenseKey;
				payload.ServiceLicense = new ServiceLicense(scutexLicense);
				payload.Token = token;

				if (!isOffline)
				{
					ActivationResult result = _licenseActiviationProvider.ActivateLicense(scutexLicense.ServiceAddress, packedToken,
																																								GetClientStandardEncryptionInfo(scutexLicense),
																																								payload, scutexLicense);

					if (result != null && result.WasRequestValid && result.ActivationSuccessful)
					{
						scutexLicense.IsLicensed = true;
						scutexLicense.IsActivated = true;
						scutexLicense.ActivatingServiceId = result.ServiceId;
						scutexLicense.ActivationToken = result.ActivationToken;
						scutexLicense.ActivatedOn = DateTime.Now;
						scutexLicense.ActivationLastCheckedOn = DateTime.Now;

						_clientLicenseService.SaveClientLicense(scutexLicense);

						return scutexLicense;
					}
				}
				else
				{
					scutexLicense.IsLicensed = true;
					scutexLicense.IsActivated = false;
					scutexLicense.ActivatingServiceId = null;
					scutexLicense.ActivationToken = null;
					scutexLicense.ActivatedOn = DateTime.Now;
					scutexLicense.ActivationLastCheckedOn = DateTime.Now;

					_clientLicenseService.SaveClientLicense(scutexLicense);

					return scutexLicense;
				}
			}

			return scutexLicense;
		}
	}
}
