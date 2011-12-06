using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Repositories;

namespace WaveTech.Scutex.Repositories.ManagerDataRepository
{
	internal class LicenseSetsRepository : ILicenseSetsRepository
	{
		private readonly ScutexEntities db;
		private readonly IFeaturesRepository _featuresRepository;

		public LicenseSetsRepository(ScutexEntities db, IFeaturesRepository featuresRepository)
		{
			this.db = db;
			_featuresRepository = featuresRepository;
		}

		public IQueryable<Model.LicenseSet> GetAllLicenseSets()
		{
			return from licSet in db.LicenseSets
						 select new Model.LicenseSet
						 {
							 LicenseSetId = licSet.LicenseSetId,
							 LicenseId = licSet.License.LicenseId,
							 Name = licSet.Name,
							 SupportedLicenseTypes = (LicenseKeyTypeFlag)licSet.LicenseType,
							 UniquePad = licSet.UniquePad,
							 MaxUsers = licSet.MaxUsers
							 //Features = new NotifyList<Model.Feature>(GetAllLicenseSetFeatures(licSet.LicenseSetId).ToList())
						 };
		}

		private IQueryable<Model.Feature> GetAllLicenseSetFeatures(int licenseSetId)
		{
			var licSets = from licSetFeatures in db.LicenseSetFeatures
										where licSetFeatures.LicenseSet.LicenseSetId == licenseSetId
										select licSetFeatures.Feature.FeatureId;

			return from f in _featuresRepository.GetAllFeatures()
						 where licSets.Contains(f.ProductFeatureId)
						 select f;
		}

		public IQueryable<Model.LicenseSet> GetLicenseSetById(int licenseSetId)
		{
			return from ls in GetAllLicenseSets()
						 where ls.LicenseSetId == licenseSetId
						 select ls;
		}

		public IQueryable<Model.LicenseSet> InsertLicenseSet(Model.LicenseSet licenseSet)
		{
			int newId;

			//using (ScutexEntities db1 = new ScutexEntities())
			//{
			LicenseSet ls = new LicenseSet();

			Mapper.CreateMap<Model.LicenseSet, LicenseSet>();
			ls = Mapper.Map<Model.LicenseSet, LicenseSet>(licenseSet);

			ls.LicenseType = (int)licenseSet.SupportedLicenseTypes;

			db.AddToLicenseSets(ls);
			db.SaveChanges();

			newId = ls.LicenseSetId;
			//}

			CreateLicsenSetFeatures(newId, licenseSet.Features);

			return GetLicenseSetById(newId);
		}

		public IQueryable<Model.LicenseSet> UpdateLicenseSet(Model.LicenseSet licenseSet)
		{
			int newId;

			//using (ScutexEntities db1 = new ScutexEntities())
			//{
			var licSet = (from ls in db.LicenseSets
										where ls.LicenseSetId == licenseSet.LicenseSetId
										select ls).First();

			licSet.Name = licenseSet.Name;
			licSet.LicenseId = licenseSet.LicenseId;
			licSet.UniquePad = licenseSet.UniquePad;
			licSet.LicenseType = (int)licenseSet.SupportedLicenseTypes;
			licSet.MaxUsers = licenseSet.MaxUsers;

			db.SaveChanges();

			newId = licSet.LicenseSetId;

			DeleteLicenseSetFeaturesByFeatureId(newId);
			CreateLicsenSetFeatures(newId, licenseSet.Features);
			//}

			return GetLicenseSetById(newId);
		}

		public void DeleteLicenseSetsByLicenseId(int licenseId)
		{
			//using (ScutexEntities db1 = new ScutexEntities())
			//{
			var licSets = from ls in db.LicenseSets
										where ls.LicenseId == licenseId
										select ls;

			foreach (var i in licSets)
				db.LicenseSets.DeleteObject(i);

			db.SaveChanges();
			//}
		}

		private void DeleteLicenseSetFeaturesByFeatureId(int licenseSetId)
		{
			//using (ScutexEntities db1 = new ScutexEntities())
			//{
			var licSetFeatures = from lcf in db.LicenseSetFeatures
													 where lcf.LicenseSetId == licenseSetId
													 select lcf;

			foreach (var i in licSetFeatures)
				db.LicenseSetFeatures.DeleteObject(i);

			db.SaveChanges();
			//}
		}

		private void CreateLicsenSetFeatures(int licenseSetId, IEnumerable<Model.Feature> features)
		{
			//using (ScutexEntities db1 = new ScutexEntities())
			//{
			foreach (var feat in features)
			{
				LicenseSetFeature feature = new LicenseSetFeature();
				feature.LicenseSetId = licenseSetId;
				feature.FeatureId = feat.ProductFeatureId;

				db.AddToLicenseSetFeatures(feature);
			}

			db.SaveChanges();
			//}
		}
	}
}