using System;
using System.Collections.Generic;
using System.Linq;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Repositories;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Services
{
	internal class LicenseService : ILicenseService
	{
		private readonly ILicensesRepository _licensesRepository;
		private readonly ILicenseSetService _licenseSetService;

		public LicenseService(ILicensesRepository licensesRepository, ILicenseSetService licenseSetService)
		{
			_licensesRepository = licensesRepository;
			_licenseSetService = licenseSetService;
		}

		public List<License> GetAllLicenses()
		{
			return _licensesRepository.GetAllLicenses().ToList();
		}

		public bool IsProductIdUsed(int productId)
		{
			return _licensesRepository.IsProductIdUsed(productId);
		}

		public License SaveLicense(License license)
		{
			if (license.LicenseId == 0)
			{
				license.CreatedOn = DateTime.Now;
				return _licensesRepository.InsertLicense(license).FirstOrDefault();
			}
			else
			{
				license.UpdatedOn = DateTime.Now;
				return _licensesRepository.UpdateLicense(license).FirstOrDefault();
			}
		}

		public bool IsLicenseProjectNameInUse(string name)
		{
			var lics = from l in _licensesRepository.GetAllLicenses()
								 where l.Name == name
								 select l;

			if (lics.Count() > 0)
				return true;

			return false;
		}

		public License GetLicenseById(int licenseId)
		{
			return (from l in _licensesRepository.GetAllLicenses()
							where l.LicenseId == licenseId
							select l).FirstOrDefault();
		}

		public List<License> GetLast10Licenses()
		{
			var newLics = (from l in _licensesRepository.GetAllLicenses()
										 where l.UpdatedOn == null
										 orderby l.CreatedOn descending
										 select l).ToList();


			var updatedLics = (from l in _licensesRepository.GetAllLicenses()
												 where l.UpdatedOn != null
												 orderby l.UpdatedOn descending
												 select l).ToList();

			SortedDictionary<DateTime, License> sortedLics = new SortedDictionary<DateTime, License>();

			foreach (var lic in newLics)
			{
				sortedLics.Add(lic.CreatedOn, lic);
			}

			foreach (var lic in updatedLics)
			{
				sortedLics.Add(lic.UpdatedOn.Value, lic);
			}

			return sortedLics.Take(8).Select(x => x.Value).ToList();
		}

		public Dictionary<License, List<LicenseSet>> GetAllLicensesAndSets()
		{
			Dictionary<License, List<LicenseSet>> data = new Dictionary<License, List<LicenseSet>>();

			List<License> licenses = GetAllLicenses();

			foreach (License l in licenses)
			{
				List<LicenseSet> sets = _licenseSetService.GetLiceseSetsByLicenseId(l.LicenseId);

				data.Add(l, sets);
			}

			return data;
		}
	}
}
