using System;
using System.Collections.Generic;
using System.Linq;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Repositories;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Services
{
	public class FeaturesService : IFeaturesService
	{
		private readonly IFeaturesRepository _featuresRepository;

		public FeaturesService(IFeaturesRepository featuresRepository)
		{
			_featuresRepository = featuresRepository;
		}

		public List<Feature> GetAllFeatures()
		{
			return _featuresRepository.GetAllFeatures().ToList();
		}

		public Feature GetFeaturetById(int featureId)
		{
			return (from f in _featuresRepository.GetAllFeatures()
			        where f.FeatureId == featureId
			        select f).FirstOrDefault();
		}

		public Feature SaveFeature(Feature feature)
		{
			if (feature.FeatureId == 0)
				return _featuresRepository.InsertFeature(feature).First();
			else
				return _featuresRepository.UpdateFeature(feature).First();
		}

		public void DeleteFeatureById(int featureId)
		{
			_featuresRepository.DeleteFeatureById(featureId);
		}

		public bool IsFeatureNameInUse(int productId, string name)
		{
			var prods = from p in _featuresRepository.GetAllFeatures()
									where p.Name == name
									select p;

			if (prods.Count() > 0)
				return true;

			return false;
		}

		public bool IsFeatureInUse(int featureId)
		{
			return _featuresRepository.IsFeatureInUse(featureId);
		}

		public List<Feature> GetFeaturesForProduct(int productId)
		{
			return (from f in _featuresRepository.GetAllFeatures()
			        where f.ProductId == productId
			        select f).ToList();
		}
	}
}
