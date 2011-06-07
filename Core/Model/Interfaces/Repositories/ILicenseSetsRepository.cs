using System.Linq;

namespace WaveTech.Scutex.Model.Interfaces.Repositories
{
	public interface ILicenseSetsRepository
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		IQueryable<LicenseSet> GetAllLicenseSets();

		IQueryable<Model.LicenseSet> GetLicenseSetById(int licenseSetId);

		IQueryable<Model.LicenseSet> InsertLicenseSet(Model.LicenseSet licenseSet);

		IQueryable<Model.LicenseSet> UpdateLicenseSet(Model.LicenseSet licenseSet);

		void DeleteLicenseSetsByLicenseId(int licenseId);
	}
}