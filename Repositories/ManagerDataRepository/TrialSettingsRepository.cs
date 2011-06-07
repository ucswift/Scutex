using System.Linq;
using AutoMapper;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Repositories;

namespace WaveTech.Scutex.Repositories.ManagerDataRepository
{
	internal class TrialSettingsRepository : ITrialSettingsRepository
	{
		private readonly ScutexEntities db;

		public TrialSettingsRepository(ScutexEntities db)
		{
			this.db = db;
		}

		public IQueryable<LicenseTrialSettings> GetAllTrialSettings()
		{
			var query = from ts in db.TrialSettings.AsEnumerable()
									select new LicenseTrialSettings
								 {
									 TrialSettingId = ts.TrialSettingId,
									 LicenseId = ts.LicenseId,
									 ExpirationOptions = (TrialExpirationOptions)ts.TrialExpirationOptionId,
									 ExpirationData = ts.ExpirationData
								 };

			return query.AsQueryable();
		}

		public IQueryable<LicenseTrialSettings> GetTrialSettingsById(int trialSettingsById)
		{
			return from ts in GetAllTrialSettings()
						 where ts.TrialSettingId == trialSettingsById
						 select ts;
		}

		public IQueryable<LicenseTrialSettings> InsertTrialSettings(LicenseTrialSettings trialSettings)
		{
			int newId;

			//using (ScutexEntities db1 = new ScutexEntities())
			//{
			TrialSetting ts = new TrialSetting();

			Mapper.CreateMap<Model.LicenseTrialSettings, TrialSetting>();
			ts = Mapper.Map<Model.LicenseTrialSettings, TrialSetting>(trialSettings);

			ts.TrialExpirationOptionId = (int)trialSettings.ExpirationOptions;

			db.AddToTrialSettings(ts);
			db.SaveChanges();

			newId = ts.TrialSettingId;
			//}

			return GetTrialSettingsById(newId);
		}

		public IQueryable<LicenseTrialSettings> UpdateTrialSettings(LicenseTrialSettings trialSettings)
		{
			int newId;

			//using (ScutexEntities db1 = new ScutexEntities())
			//{
			var trialSet = (from ts in db.TrialSettings
											where ts.TrialSettingId == trialSettings.TrialSettingId
											select ts).First();

			trialSet.TrialExpirationOptionId = (int)trialSettings.ExpirationOptions;
			trialSet.ExpirationData = trialSettings.ExpirationData;

			db.SaveChanges();

			newId = trialSet.TrialSettingId;
			//}

			return GetTrialSettingsById(newId);
		}

		public void DeleteTrialSettingsByLicenseId(int licenseId)
		{
			//using (ScutexEntities db1 = new ScutexEntities())
			//{
			var trialSets = from ts in db.TrialSettings
											where ts.LicenseId == licenseId
											select ts;

			foreach (var i in trialSets)
				db.TrialSettings.DeleteObject(i);

			db.SaveChanges();
			//}
		}
	}
}