using System.Linq;
using AutoMapper;
using WaveTech.Scutex.Model.Interfaces.Repositories;

namespace WaveTech.Scutex.Repositories.ManagerDataRepository
{
	internal class ProductsRepository : IProductsRepository
	{
		private readonly ScutexEntities db;

		public ProductsRepository(ScutexEntities db)
		{
			this.db = db;
		}

		public IQueryable<Model.Product> GetAllProducts()
		{
			return from prod in db.Products
						 select new Model.Product
											{
												ProductId = prod.ProductId,
												Description = prod.Description,
												Name = prod.Name,
												UniquePad = prod.UniquePad
											};
		}

		public IQueryable<Model.Product> GetProductById(int productId)
		{
			return from p in GetAllProducts()
						 where p.ProductId == productId
						 select p;
		}


		public IQueryable<Model.Product> InsertProduct(Model.Product product)
		{
			int newId;

			//using (ScutexEntities db1 = new ScutexEntities())
			//{
			Product prod = new Product();

			Mapper.CreateMap<Model.Product, Product>();
			prod = Mapper.Map<Model.Product, Product>(product);

			db.AddToProducts(prod);
			db.SaveChanges();

			newId = prod.ProductId;
			//}

			return GetProductById(newId);
		}

		public IQueryable<Model.Product> UpdateProduct(Model.Product product)
		{
			int newId;

			//using (ScutexEntities db1 = new ScutexEntities())
			//{
			Product prod = (from p in db.Products
											where p.ProductId == product.ProductId
											select p).First();

			prod.Name = product.Name;
			prod.Description = product.Description;

			db.SaveChanges();

			newId = prod.ProductId;
			//}

			return GetProductById(newId);
		}

		public void DeleteProductById(int productId)
		{
			//using (ScutexEntities db1 = new ScutexEntities())
			//{
			db.DeleteObject(db.Products.FirstOrDefault(x => x.ProductId == productId));
			db.SaveChanges();
			//}
		}
	}
}