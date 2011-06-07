using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Exceptions;
using WaveTech.Scutex.Model.Interfaces.Generators;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Services
{
	internal class LicenseKeyService : ILicenseKeyService
	{
		private readonly IKeyGenerator keyGenerator;
		private readonly IPackingService _packingService;
		private readonly IClientLicenseService _clientLicenseService;

		public LicenseKeyService(IKeyGenerator keyGenerator, IPackingService packingService, IClientLicenseService clientLicenseService)
		{
			this.keyGenerator = keyGenerator;
			_packingService = packingService;
			_clientLicenseService = clientLicenseService;
		}

		public string GenerateLicenseKey(string rsaXmlString, LicenseBase scutexLicense, LicenseGenerationOptions generationOptions)
		{
			return keyGenerator.GenerateLicenseKey(rsaXmlString, scutexLicense, generationOptions);
		}

		public List<string> GenerateLicenseKeys(string rsaXmlString, LicenseBase scutexLicense, LicenseGenerationOptions generationOptions, int count)
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
				Debug.WriteLine(string.Format("{0} duplicate keys were generated at a {1}% chance", doupCount, doupCount * 100m / count));

			return licenses.ToList();
		}

		public bool ValidateLicenseKey(string licenseKey, LicenseBase scutexLicense, bool catchException)
		{
			if (catchException)
			{
				try
				{
					return keyGenerator.ValidateLicenseKey(licenseKey, scutexLicense);
				}
				catch (ScutexLicenseException)
				{
					return false;
				}
			}

			return keyGenerator.ValidateLicenseKey(licenseKey, scutexLicense);
		}

		public KeyData GetLicenseKeyData(string licenseKey, LicenseBase scutexLicense)
		{
			return keyGenerator.GetLicenseKeyData(licenseKey, scutexLicense);
		}
	}
}
