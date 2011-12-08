using System.Linq;
using AutoMapper;
using WaveTech.Scutex.Model.Interfaces.Repositories;

namespace WaveTech.Scutex.Repositories.ManagerDataRepository
{
	internal class FeaturesRepository : IFeaturesRepository
	{
		private readonly ScutexEntities db;

		public FeaturesRepository(ScutexEntities db)
		{
			this.db = db;
		}

		public IQueryable<Model.Feature> GetAllFeatures()
		{
			return from feature in db.Features
						 select new Model.Feature
						 {
							 FeatureId = feature.FeatureId,
							 ProductId = feature.Product.ProductId,
							 Name = feature.Name,
							 Description = feature.Description,
							 UniquePad = feature.UniquePad
						 };
		}

		public IQueryable<Model.Feature> GetFeatureById(int featureId)
		{
			return from feature in GetAllFeatures()
						 where feature.FeatureId == featureId
						 select feature;
		}

		public IQueryable<Model.Feature> InsertFeature(Model.Feature feature)
		{
			int newId;

			//using (ScutexEntities db1 = new ScutexEntities())
			//{
			Feature feat = new Feature();

			Mapper.CreateMap<Model.Feature, Feature>();
			feat = Mapper.Map<Model.Feature, Feature>(feature);

			db.AddToFeatures(feat);
			db.SaveChanges();

			newId = feat.FeatureId;
			//}

			return GetFeatureById(newId);
		}

		public IQueryable<Model.Feature> UpdateFeature(Model.Feature feature)
		{
			int newId;

			//using (ScutexEntities db1 = new ScutexEntities())
			//{
			var feat = (from f in db.Features
									where f.FeatureId == feature.FeatureId
									select f).First();

			feat.Name = feature.Name;
			feat.Description = feature.Description;
			feat.ProductId = feature.ProductId;
			feat.UniquePad = feature.UniquePad;
			
			db.SaveChanges();

			newId = feat.FeatureId;
			//}

			return GetFeatureById(newId);
		}

		public void DeleteFeatureById(int featureId)
		{
			db.DeleteObject(db.Features.FirstOrDefault(x => x.FeatureId == featureId));
			db.SaveChanges();
		}

		public bool IsFeatureInUse(int featureId)
		{
			var sets = from lsf in db.LicenseSetFeatures
								 where lsf.FeatureId == featureId
								 select lsf;

			if (sets.Count() > 0)
				return true;

			return false;
		}
	}
}
