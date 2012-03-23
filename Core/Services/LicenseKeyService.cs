using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Exceptions;
using WaveTech.Scutex.Model.Interfaces.Generators;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Services
{
	internal class LicenseKeyService : ILicenseKeyService
	{
		private readonly ISmallKeyGenerator keyGenerator;
		private readonly ILargeKeyGenerator _largeKeyGenerator;
		private readonly IPackingService _packingService;
		private readonly IClientLicenseService _clientLicenseService;

		public LicenseKeyService(ISmallKeyGenerator staticSmallKeyGenerator, ILargeKeyGenerator staticLargeKeyGenerator,
		                         IPackingService packingService, IClientLicenseService clientLicenseService)
		{
			keyGenerator = staticSmallKeyGenerator;
			_largeKeyGenerator = staticLargeKeyGenerator;
			_packingService = packingService;
			_clientLicenseService = clientLicenseService;

			// Need to find a way to get these passed in correctly
			//keyGenerator = ObjectLocator.GetInstance<IKeyGenerator>(InstanceNames.SmallKeyGenerator);
			//_largeKeyGenerator = ObjectLocator.GetInstance<IKeyGenerator>(InstanceNames.LargeKeyGenerator);
		}

		public string GenerateLicenseKey(string rsaXmlString, LicenseBase scutexLicense,
		                                 LicenseGenerationOptions generationOptions)
		{
			if (generationOptions.GeneratorType == KeyGeneratorTypes.StaticSmall)
				return keyGenerator.GenerateLicenseKey(rsaXmlString, scutexLicense, generationOptions);
			else
				return _largeKeyGenerator.GenerateLicenseKey(rsaXmlString, scutexLicense, generationOptions);
		}

		public List<string> GenerateLicenseKeys(string rsaXmlString, LicenseBase scutexLicense,
		                                        LicenseGenerationOptions generationOptions, int count)
		{
			HashSet<string> licenses = new HashSet<string>();
			int doupCount = 0;

			while (licenses.Count < count)
			{
				string key = GenerateLicenseKey(rsaXmlString, scutexLicense, generationOptions);

				if (licenses.Contains(key) == false)
				{
					licenses.Add(key);
					//Debug.WriteLine(string.Format("{0} of {1} keys generated", licenses.Count, count));
				}
				else
				{
					doupCount++;
					Debug.WriteLine(string.Format("Duplicate key was generated {0}", key));
				}
			}

			if (doupCount > 0)
				Debug.WriteLine(string.Format("{0} duplicate keys were generated at a {1}% chance", doupCount, doupCount*100m/count));

			return licenses.ToList();
		}

		public bool ValidateLicenseKey(string licenseKey, LicenseBase scutexLicense, bool catchException)
		{
			if (catchException)
			{
				try
				{
					return ValidateKey(licenseKey, scutexLicense);
				}
				catch (ScutexLicenseException)
				{
					return false;
				}
			}

			return ValidateKey(licenseKey, scutexLicense);
		}

		public KeyData GetLicenseKeyData(string licenseKey, LicenseBase scutexLicense, bool catchException)
		{
			if (licenseKey == null)
				throw new ArgumentNullException("licenseKey");

			KeyData data = null;

			if (catchException)
			{
				return GetKeyData(licenseKey, scutexLicense);
			}

			return GetKeyData(licenseKey, scutexLicense);
		}

		#region Private Helpers
		private KeyData GetKeyData(string licenseKey, LicenseBase scutexLicense)
		{
			if (licenseKey.Length == 15)
				return keyGenerator.GetLicenseKeyData(licenseKey, scutexLicense);
			else
				return _largeKeyGenerator.GetLicenseKeyData(licenseKey, scutexLicense);
		}

		private bool ValidateKey(string licenseKey, LicenseBase scutexLicense)
		{
			if (licenseKey.Length == 15)
				return keyGenerator.ValidateLicenseKey(licenseKey, scutexLicense);
			else
				return _largeKeyGenerator.ValidateLicenseKey(licenseKey, scutexLicense);
		}
		#endregion Private Helpers
	}
}
