using System.Linq;

namespace WaveTech.Scutex.Model.Interfaces.Repositories
{
	public interface ILicensesRepository
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		IQueryable<License> GetAllLicenses();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="licenseId"></param>
		/// <returns></returns>
		IQueryable<License> GetLicenseById(int licenseId);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		IQueryable<License> InsertLicense(License license);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="license"></param>
		IQueryable<License> UpdateLicense(License license);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="licenseId"></param>
		void DeleteLicenseById(int licenseId);

		bool IsProductIdUsed(int productId);
	}
}