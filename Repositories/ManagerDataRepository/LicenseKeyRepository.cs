using System.Linq;
using AutoMapper;
using WaveTech.Scutex.Model.Interfaces.Repositories;

namespace WaveTech.Scutex.Repositories.ManagerDataRepository
{
	internal class LicenseKeyRepository : ILicenseKeyRepository
	{
		private readonly ScutexEntities db;

		public LicenseKeyRepository(ScutexEntities db)
		{
			this.db = db;
		}

		public IQueryable<Model.LicenseKey> GetAllLicenseKeys()
		{
			return from lk in db.LicenseKeys
						 select new Model.LicenseKey
											{
												CreatedOn = lk.CreatedOn,
												Key = lk.LicenseKey1,
												HashedLicenseKey = lk.HashedLicenseKey,
												LicenseKeyId = lk.LicenseKeyId,
												LicenseSetId = lk.LicenseSetId
											};
		}

		public void InsertLicenseKey(Model.LicenseKey key)
		{
			long newId;

			//using (ScutexEntities db1 = new ScutexEntities())
			//{
			LicenseKey lk = new LicenseKey();

			Mapper.CreateMap<Model.LicenseKey, LicenseKey>();
			lk = Mapper.Map<Model.LicenseKey, LicenseKey>(key);
			lk.LicenseKey1 = key.Key;
			lk.HashedLicenseKey = key.HashedLicenseKey;

			db.AddToLicenseKeys(lk);
			db.SaveChanges();

			newId = lk.LicenseKeyId;
			//}
		}
	}
}
