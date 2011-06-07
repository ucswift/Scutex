using System.Linq;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Repositories;

namespace WaveTech.Scutex.Repositories.ServicesDataRepository
{
	internal class ClientRepository : IClientRepository
	{
		private readonly ScutexServiceEntities db;

		public ClientRepository(ScutexServiceEntities db)
		{
			this.db = db;
		}

		public IQueryable<Model.LicenseKey> GetAllLicenseKeys()
		{
			return from lk in db.LicenseKeys
						 select new Model.LicenseKey
						 {
							 LicenseKeyId = lk.LicenseKeyId,
							 LicenseSetId = lk.LicenseSetId,
							 Key = lk.Key,
							 CreatedOn = lk.CreatedOn
						 };
		}

		public IQueryable<Model.LicenseSet> GetAllLicenseSets()
		{
			return from ls in db.LicenseSets
						 select new Model.LicenseSet
						 {
							 LicenseSetId = ls.LicenseSetId,
							 LicenseId = ls.LicenseId,
							 SupportedLicenseTypes = (LicenseKeyTypeFlag)ls.LicenseType,
							 MaxUsers = ls.MaxUsers
						 };
		}

		public IQueryable<Model.LicenseActivation> GetAllLicenseActivations()
		{
			return from la in db.LicenseActivations
						 select new Model.LicenseActivation
						 {
							 LicenseActivationId = la.LicenseActivationId,
							 LicenseKeyId = la.LicenseKeyId,
							 ActivationToken = la.ActivationToken,
							 ActivatedOn = la.ActivatedOn,
							 OriginalToken = la.OriginalToken,
							 ActivationStatus = (ActivationStatus)la.ActivationStatus,
							 ActivationStatusUpdatedOn = la.ActivationStatusUpdatedOn,
							 HardwareHash = la.HardwareHash
						 };
		}

		public Model.LicenseKey GetLiceseKeyByKeyLicenseSetId(string liceseKey, int liceseSetId)
		{
			return (from lk in GetAllLicenseKeys()
							where lk.Key == liceseKey && lk.LicenseSetId == liceseSetId
							select lk).FirstOrDefault();
		}

		public Model.LicenseSet GetLicenseSetById(int licenseSetId)
		{
			return (from ls in GetAllLicenseSets()
							where ls.LicenseSetId == licenseSetId
							select ls).FirstOrDefault();
		}

		public Model.LicenseActivation GetLicenseActivationByKeyAndSetId(string licenseKey, int licenseSetId)
		{
			Model.LicenseKey lk = GetLiceseKeyByKeyLicenseSetId(licenseKey, licenseSetId);

			if (lk != null)
				return (from la in GetAllLicenseActivations()
								where la.LicenseKeyId == lk.LicenseKeyId
								select la).FirstOrDefault();

			return null;
		}

		public Model.LicenseActivation GetLicenseActivationById(int licenseActivationId)
		{
			return (from la in GetAllLicenseActivations()
							where la.LicenseActivationId == licenseActivationId
							select la).FirstOrDefault();
		}

		public Model.LicenseActivation InsertLicenseActivation(Model.LicenseActivation licenseActivation)
		{
			int newId;

			using (ScutexServiceEntities db1 = new ScutexServiceEntities())
			{
				LicenseActivation la = new LicenseActivation();

				la.LicenseKeyId = licenseActivation.LicenseKeyId;
				la.ActivationToken = licenseActivation.ActivationToken;
				la.ActivatedOn = licenseActivation.ActivatedOn;
				la.OriginalToken = licenseActivation.OriginalToken;
				la.ActivationStatus = (int)licenseActivation.ActivationStatus;
				la.ActivationStatusUpdatedOn = licenseActivation.ActivationStatusUpdatedOn;
				la.HardwareHash = licenseActivation.HardwareHash;

				db1.AddToLicenseActivations(la);
				db1.SaveChanges();

				newId = la.LicenseActivationId;
			}

			return GetLicenseActivationById(newId);
		}
	}
}