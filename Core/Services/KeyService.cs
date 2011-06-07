
using System;
using System.Collections.Generic;
using System.Linq;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Repositories;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Services
{
	internal class KeyService : IKeyService
	{
		#region Private Readonly Members
		private readonly ILicenseKeyRepository _licenseKeyRepoistory;
		private readonly IHashingProvider _hashingProvider;
		#endregion Private Readonly Members

		#region Constructors
		public KeyService(ILicenseKeyRepository licenseKeyRepoistory, IHashingProvider hashingProvider)
		{
			_licenseKeyRepoistory = licenseKeyRepoistory;
			_hashingProvider = hashingProvider;
		}
		#endregion Constructors

		public List<string> GetAllLicenseKeysByLicenseSet(int licenseSetId)
		{
			return (from k in _licenseKeyRepoistory.GetAllLicenseKeys()
							where k.LicenseSetId == licenseSetId
							select k.Key).ToList();
		}

		public List<string> GetAllHashedLicenseKeysByLicenseSet(int licenseSetId)
		{
			List<string> hashedKeys = new List<string>();
			List<string> keys = GetAllLicenseKeysByLicenseSet(licenseSetId);

			foreach (string k in keys)
			{
				hashedKeys.Add(_hashingProvider.ComputeHash(k, Properties.Resources.KeyHashingAlgo));
			}

			return hashedKeys;
		}

		public void SaveLicenseKeysForLicenseSet(LicenseSet licenseSet, List<string> keys)
		{
			foreach (string k in keys)
			{
				LicenseKey lk = new LicenseKey();
				lk.CreatedOn = DateTime.Now;
				lk.Key = k;
				lk.LicenseSetId = licenseSet.LicenseSetId;
				lk.HashedLicenseKey = _hashingProvider.ComputeHash(k, Properties.Resources.KeyHashingAlgo);

				_licenseKeyRepoistory.InsertLicenseKey(lk);
			}
		}
	}
}