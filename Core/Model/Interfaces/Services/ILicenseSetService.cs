
using System.Collections.Generic;

namespace WaveTech.Scutex.Model.Interfaces.Services
{
	public interface ILicenseSetService
	{
		LicenseSet GetLiceseSetById(int licenseSetId);
		List<LicenseSet> GetLiceseSetsByLicenseId(int licenseId);
	}
}