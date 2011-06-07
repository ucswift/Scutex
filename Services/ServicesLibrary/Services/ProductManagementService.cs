
using System;
using System.Collections.Generic;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Repositories;
using WaveTech.Scutex.Model.Interfaces.Wcf;

namespace WaveTech.Scutex.WcfServices.ServicesLibrary.Services
{
	public class ProductManagementService : IProductManagementService
	{
		#region Private Readonly Members
		private readonly IServiceProductsRepository _serviceProductsRepository;
		private readonly IHashingProvider _hashingProvider;
		private readonly IActivationLogRepoistory _activationLogRepoistory;
		#endregion Private Readonly Members

		#region Constructor
		public ProductManagementService(IServiceProductsRepository serviceProductsRepository,
			IHashingProvider hashingProvider, IActivationLogRepoistory activationLogRepoistory)
		{
			_serviceProductsRepository = serviceProductsRepository;
			_hashingProvider = hashingProvider;
			_activationLogRepoistory = activationLogRepoistory;
		}
		#endregion Constructor

		public ServiceProduct GetServiceProductById(int licenseId)
		{
			return _serviceProductsRepository.GetProduct(licenseId);
		}

		public ServiceProduct SaveServiceProduct(ServiceProduct product)
		{
			return _serviceProductsRepository.SaveServiceProduct(product);
		}

		public ServiceProduct CreateTestProduct(string licenseKey)
		{
			DeleteTestServiceProduct();

			ServiceProduct prod = new ServiceProduct();
			prod.LicenseId = int.MaxValue;
			prod.LicenseName = "Test Product License";
			prod.LicenseSets = new List<ServiceLicenseSet>();

			ServiceLicenseSet ls = new ServiceLicenseSet();
			ls.LicenseSetId = int.MaxValue;
			ls.LicenseId = int.MaxValue;
			ls.Keys = new List<ServiceLicenseKey>();
			ls.LicenseSetName = "Test Product License Set";
			ls.LicenseType = LicenseKeyTypeFlag.MultiUser;
			ls.MaxUsers = 2;

			ServiceLicenseKey key = new ServiceLicenseKey();
			key.CreatedOn = DateTime.Now;
			key.Key = _hashingProvider.ComputeHash(licenseKey, "SHA256");
			key.LicenseSetId = int.MaxValue;
			key.ActivationCount = 0;
			ls.Keys.Add(key);

			prod.LicenseSets.Add(ls);

			_serviceProductsRepository.SaveServiceProduct(prod);

			return _serviceProductsRepository.GetProduct(int.MaxValue);
		}

		public void DeleteTestServiceProduct()
		{
			ServiceProduct testProduct = _serviceProductsRepository.GetProduct(int.MaxValue);

			if (testProduct != null)
			{
				List<ServiceLicenseKey> keys = _serviceProductsRepository.GetServiceLicenseKeysByLicenseSet(int.MaxValue);

				foreach (ServiceLicenseKey k in keys)
				{
					_serviceProductsRepository.DeleteLicenseActivationsByKeyId(k.LicenseKeyId);
				}

				foreach (ServiceLicenseKey k in keys)
				{
					_activationLogRepoistory.DeleteActivationLogByKey(k.Key);
				}

				_serviceProductsRepository.DeleteLicenseKeyByLicenseSetId(int.MaxValue);
				_serviceProductsRepository.DeleteLicenseSetId(int.MaxValue);
				_serviceProductsRepository.DeleteLicenseId(int.MaxValue);
			}
		}

		public List<ServiceProduct> GetAllServicePorducts()
		{
			return _serviceProductsRepository.GetAllProducts();
		}
	}
}