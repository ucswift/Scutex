using System.Linq;
using AutoMapper;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Repositories;

namespace WaveTech.Scutex.Repositories.ManagerDataRepository
{
	internal class ProductsRepository : IProductsRepository
	{
		private readonly ScutexEntities db;
		private readonly IFeaturesRepository _featuresRepository;

		public ProductsRepository(ScutexEntities db, IFeaturesRepository featuresRepository)
		{
			this.db = db;
			_featuresRepository = featuresRepository;
		}

		public IQueryable<Model.Product> GetAllProducts()
		{
			var query = from prod in db.Products.AsEnumerable()
						 select new Model.Product
											{
												ProductId = prod.ProductId,
												Description = prod.Description,
												Name = prod.Name,
												UniquePad = prod.UniquePad,
												Features = new NotifyList<Model.Feature>(_featuresRepository.GetAllFeatures().Where(x => x.ProductId == prod.ProductId).ToList())
											};

			return query.AsQueryable();
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

			Mapper.CreateMap<Model.Product, Product>().ForMember(dest => dest.Features, opt => opt.Ignore()); ;
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