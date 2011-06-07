using System.ServiceModel;

namespace WaveTech.Scutex.WcfServices.ServicesLibrary.Interfaces.Management
{
	[ServiceContract]
	public interface IProductsService
	{
		[OperationContract]
		string AddProduct(string token, string data);

		[OperationContract]
		string GetLicenseKeysForProduct(string token, string data);

		[OperationContract]
		string AddLicenseKeysForProduct(string token, string data);
	}
}