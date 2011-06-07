using System.Linq;

namespace WaveTech.Scutex.Model.Interfaces.Repositories
{
	public interface ILicenseKeyRepository
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		IQueryable<LicenseKey> GetAllLicenseKeys();

		void InsertLicenseKey(LicenseKey key);
	}
}
