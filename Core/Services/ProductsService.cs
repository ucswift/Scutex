using System.Collections.Generic;
using System.Linq;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Repositories;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Services
{
	internal class ProductsService : IProductsService
	{
		#region Private Readonly Members
		private readonly IProductsRepository productsRepository;
		#endregion Private Readonly Members

		#region Constructor
		public ProductsService(IProductsRepository productsRepository)
		{
			this.productsRepository = productsRepository;
		}
		#endregion Constructor

		#region Get All Products
		public List<Product> GetAllProducts()
		{
			return productsRepository.GetAllProducts().ToList();
		}
		#endregion Get All Products

		#region Get Product By Id
		public Product GetProductById(int productId)
		{
			return productsRepository.GetProductById(productId).First();
		}
		#endregion Get Product By Id

		#region Save Product
		public Product SaveProduct(Product product)
		{
			if (product.ProductId == 0)
				return productsRepository.InsertProduct(product).First();
			else
				return productsRepository.UpdateProduct(product).First();
		}
		#endregion Save Product

		#region Delete Product By Id
		public void DeleteProductById(int productId)
		{
			productsRepository.DeleteProductById(productId);
		}
		#endregion Delete Product By Id

		#region Is Product Name In Use
		public bool IsProductNameInUse(string name)
		{
			var prods = from p in productsRepository.GetAllProducts()
									where p.Name == name
									select p;

			if (prods.Count() > 0)
				return true;

			return false;
		}
		#endregion Is Product Name In Use
	}
}