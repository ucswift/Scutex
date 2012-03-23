using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Repositories;
using WaveTech.Scutex.Model.Interfaces.Services;
using WaveTech.Scutex.Model.Interfaces.Wcf;
using WaveTech.Scutex.WcfServices.ServicesLibrary.Properties;

namespace WaveTech.Scutex.WcfServices.ServicesLibrary.Services
{
	public class KeyManagementService : IKeyManagementService
	{
		private readonly IClientRepository _clientRepository;
		private readonly ILicenseKeyService _licenseKeyService;
		private readonly IActivationLogService _activationLogService;
		private readonly IHashingProvider _hashingProvider;
		private readonly IServiceProductsRepository _serviceProductsRepository;

		public KeyManagementService(IClientRepository clientRepository, ILicenseKeyService licenseKeyService,
			IActivationLogService activationLogService, IHashingProvider hashingProvider, IServiceProductsRepository serviceProductsRepository)
		{
			_clientRepository = clientRepository;
			_licenseKeyService = licenseKeyService;
			_activationLogService = activationLogService;
			_hashingProvider = hashingProvider;
			_serviceProductsRepository = serviceProductsRepository;
		}

		public List<LicenseActivation> GetAllLicenseActivations()
		{
			return _clientRepository.GetAllLicenseActivations().ToList();
		}

		public bool DoesKeyExistForLicenseSet(string licenseKey, int licenseSetId)
		{
			if (_clientRepository.GetLiceseKeyByKeyLicenseSetId(licenseKey, licenseSetId) != null)
				return true;

			return false;
		}

		public List<string> GetKeysForLicenseSet(int licenseSetId)
		{
			return (from k in _clientRepository.GetAllLicenseKeys()
							where k.LicenseSetId == licenseSetId
							select k.Key).ToList();
		}

		public void AddLicenseKeysForLicenseSet(int licenseSetId, List<string> licenseKeys)
		{
			foreach (string k in licenseKeys)
			{
				if (DoesKeyExistForLicenseSet(k, licenseSetId) == false)
				{
					ServiceLicenseKey key = new ServiceLicenseKey();
					key.ActivationCount = 0;
					key.CreatedOn = DateTime.Now;
					key.Deactivated = false;
					key.Key = k;
					key.LicenseSetId = licenseSetId;

					_serviceProductsRepository.SaveServiceLicenseKey(key);
				}
			}
		}

		public bool IsKeyAvialable(string licenseKey, int licenseSetId)
		{
			ServiceLicenseSet ls = _serviceProductsRepository.GetServiceLicenseSetById(licenseSetId);
			ServiceLicenseKey lk = _serviceProductsRepository.GetServiceLicenseKeyByKeyLicenseSet(licenseKey, licenseSetId);
			LicenseActivation la = _clientRepository.GetLicenseActivationByKeyAndSetId(licenseKey, licenseSetId);

			if (la == null)
				return true;

			Debug.WriteLine(ls.LicenseType.ToString());

			if (ls.LicenseType.IsSet(LicenseKeyTypeFlag.Unlimited) || ls.LicenseType.IsSet(LicenseKeyTypeFlag.Enterprise))
				return true;

			if (ls.LicenseType.IsSet(LicenseKeyTypeFlag.MultiUser))
			{
				if (ls.MaxUsers.HasValue && ((lk.ActivationCount + 1) <= ls.MaxUsers.Value))
					return true;
			}

			if (ls.LicenseType.IsSet(LicenseKeyTypeFlag.HardwareLock))
			{
				
			}

			return false;
		}

		public bool AuthorizeLicenseForActivation(string licenseKey, ServiceLicense licenseBase, string hardwareFingerprint)
		{
			KeyData keyData = _licenseKeyService.GetLicenseKeyData(licenseKey, licenseBase);
			
			// Step 1: Validate the physical key
			bool keyValid = _licenseKeyService.ValidateLicenseKey(licenseKey, licenseBase, true);

			if (!keyValid)
			{
				_activationLogService.LogActiviation(licenseKey, ActivationResults.ValidationFailure, null);

				return false;
			}

			SecureString hashedLicenseKey = SecureStringHelper.StringToSecureString(_hashingProvider.ComputeHash(licenseKey, Resources.KeyHashAlgo));
			hashedLicenseKey.MakeReadOnly();

			// Step 2: Validate the key against the service
			if (!DoesKeyExistForLicenseSet(SecureStringHelper.SecureStringToString(hashedLicenseKey), keyData.LicenseSetId))
			{
				_activationLogService.LogActiviation(licenseKey, ActivationResults.UnknownKeyFailure, null);

				return false;
			}

			// Step 3: Is this key used already
			if (!IsKeyAvialable(SecureStringHelper.SecureStringToString(hashedLicenseKey), keyData.LicenseSetId))
			{
				_activationLogService.LogActiviation(licenseKey, ActivationResults.TooManyFailure, null);

				return false;
			}

			return true;
		}

		public Guid? ActivateLicenseKey(string licenseKey, Guid? originalToken, ServiceLicense licenseBase, string hardwareFingerprint)
		{
			KeyData keyData = _licenseKeyService.GetLicenseKeyData(licenseKey, licenseBase);
			SecureString hashedLicenseKey = SecureStringHelper.StringToSecureString(_hashingProvider.ComputeHash(licenseKey, Resources.KeyHashAlgo));
			hashedLicenseKey.MakeReadOnly();

			int licenseSetId = keyData.LicenseSetId;// TODO: Only works for SSLK
			LicenseSet ls = _clientRepository.GetLicenseSetById(licenseSetId);
			LicenseActivation la = _clientRepository.GetLicenseActivationByKeyAndSetId(SecureStringHelper.SecureStringToString(hashedLicenseKey), licenseSetId);

			if (AuthorizeLicenseForActivation(licenseKey, licenseBase, hardwareFingerprint))	// TODO: Possible double call with two log entries here, as this is called in the parent as well. -SJ
			{
				ServiceLicenseKey lk = _serviceProductsRepository.GetServiceLicenseKeyByKeyLicenseSet(SecureStringHelper.SecureStringToString(hashedLicenseKey), licenseSetId);

				la = new LicenseActivation();
				la.LicenseKeyId = lk.LicenseKeyId;
				la.ActivatedOn = DateTime.Now;
				la.ActivationToken = Guid.NewGuid();
				la.OriginalToken = originalToken;
				la.HardwareHash = licenseBase.HardwareHash;
				la.ActivationStatus = ActivationStatus.Normal;
				la.ActivationStatusUpdatedOn = DateTime.Now;

				if (!String.IsNullOrEmpty(hardwareFingerprint))
					la.HardwareHash = hardwareFingerprint;

				_clientRepository.InsertLicenseActivation(la);

				lk.ActivationCount++;

				_serviceProductsRepository.SaveServiceLicenseKey(lk);

				return la.ActivationToken;
			}

			return null;
		}
	}
}