using System.Linq;

namespace WaveTech.Scutex.Model.Interfaces.Repositories
{
	public interface IClientRepository
	{
		IQueryable<LicenseKey> GetAllLicenseKeys();
		IQueryable<LicenseSet> GetAllLicenseSets();
		IQueryable<LicenseActivation> GetAllLicenseActivations();
		LicenseKey GetLiceseKeyByKeyLicenseSetId(string liceseKey, int liceseSetId);
		LicenseSet GetLicenseSetById(int licenseSetId);
		LicenseActivation GetLicenseActivationByKeyAndSetId(string licenseKey, int licenseSetId);
		LicenseActivation InsertLicenseActivation(LicenseActivation licenseActivation);
	}
}