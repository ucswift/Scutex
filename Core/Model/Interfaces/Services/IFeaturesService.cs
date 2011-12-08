using System.Collections.Generic;

namespace WaveTech.Scutex.Model.Interfaces.Services
{
	/// <summary>
	/// Definition for interacting with the Features service
	/// </summary>
	public interface IFeaturesService
	{
		/// <summary>
		/// Get all Features in the system
		/// </summary>
		/// <returns></returns>
		List<Feature> GetAllFeatures();

		/// <summary>
		/// Gets a single Feature by it's Id
		/// </summary>
		/// <param name="featureId"></param>
		/// <returns></returns>
		Feature GetFeaturetById(int featureId);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="feature"></param>
		Feature SaveFeature(Feature feature);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="featureId"></param>
		void DeleteFeatureById(int featureId);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="productId"></param>
		/// <param name="name"></param>
		bool IsFeatureNameInUse(int productId, string name);

		bool IsFeatureInUse(int featureId);

		List<Feature> GetFeaturesForProduct(int productId);
	}
}