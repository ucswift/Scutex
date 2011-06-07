using System.Linq;

namespace WaveTech.Scutex.Model.Interfaces.Repositories
{
	/// <summary>
	/// Defines the repository access for Products
	/// </summary>
	public interface IProductsRepository
	{
		/// <summary>
		/// Gets all the products in the underlying repository
		/// </summary>
		/// <returns></returns>
		IQueryable<Product> GetAllProducts();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="productId"></param>
		/// <returns></returns>
		IQueryable<Product> GetProductById(int productId);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="p"></param>
		IQueryable<Product> InsertProduct(Product product);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="product"></param>
		IQueryable<Product> UpdateProduct(Product product);
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="productId"></param>
		void DeleteProductById(int productId);
	}
}