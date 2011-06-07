using System.Collections.Generic;
using System.Linq;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Repositories;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Services
{
	internal class LicenseSetService : ILicenseSetService
	{
		private readonly ILicenseSetsRepository _licenseSetsRepository;

		public LicenseSetService(ILicenseSetsRepository licenseSetsRepository)
		{
			_licenseSetsRepository = licenseSetsRepository;
		}

		public LicenseSet GetLiceseSetById(int licenseSetId)
		{
			return _licenseSetsRepository.GetLicenseSetById(licenseSetId).FirstOrDefault();
		}

		public List<LicenseSet> GetLiceseSetsByLicenseId(int licenseId)
		{
			return (from ls in _licenseSetsRepository.GetAllLicenseSets()
							where ls.LicenseId == licenseId
							select ls).ToList();
		}
	}
}