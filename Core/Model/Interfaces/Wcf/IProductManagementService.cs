
using System.Collections.Generic;

namespace WaveTech.Scutex.Model.Interfaces.Wcf
{
	public interface IProductManagementService
	{
		ServiceProduct GetServiceProductById(int licenseId);
		ServiceProduct SaveServiceProduct(ServiceProduct product);
		ServiceProduct CreateTestProduct(string licenseKey);
		void DeleteTestServiceProduct();
		List<ServiceProduct> GetAllServicePorducts();
	}
}
