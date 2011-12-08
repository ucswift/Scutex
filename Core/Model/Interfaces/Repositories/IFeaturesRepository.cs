using System.Linq;

namespace WaveTech.Scutex.Model.Interfaces.Repositories
{
	public interface IFeaturesRepository
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		IQueryable<Feature> GetAllFeatures();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		IQueryable<Model.Feature> InsertFeature(Model.Feature feature);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		IQueryable<Model.Feature> UpdateFeature(Model.Feature feature);

		void DeleteFeatureById(int featureId);
		bool IsFeatureInUse(int featureId);
	}
}