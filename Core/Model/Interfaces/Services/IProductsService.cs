using System.Collections.Generic;

namespace WaveTech.Scutex.Model.Interfaces.Services
{
	/// <summary>
	/// Definition for interacting with the Products service
	/// </summary>
	public interface IProductsService
	{
		/// <summary>
		/// Get all products in the system
		/// </summary>
		/// <returns></returns>
		List<Product> GetAllProducts();

		/// <summary>
		/// Gets a single product by it's Id
		/// </summary>
		/// <param name="productId"></param>
		/// <returns></returns>
		Product GetProductById(int productId);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="product"></param>
		Product SaveProduct(Product product);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="productId"></param>
		void DeleteProductById(int productId);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		bool IsProductNameInUse(string name);
	}
}