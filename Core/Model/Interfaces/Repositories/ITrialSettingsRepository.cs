using System.Linq;

namespace WaveTech.Scutex.Model.Interfaces.Repositories
{
	public interface ITrialSettingsRepository
	{
		IQueryable<LicenseTrialSettings> GetAllTrialSettings();
		IQueryable<LicenseTrialSettings> GetTrialSettingsById(int trialSettingsById);
		IQueryable<LicenseTrialSettings> InsertTrialSettings(LicenseTrialSettings trialSettings);
		IQueryable<LicenseTrialSettings> UpdateTrialSettings(LicenseTrialSettings trialSettings);
		void DeleteTrialSettingsByLicenseId(int licenseId);
	}
}
