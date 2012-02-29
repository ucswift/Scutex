using System.ServiceModel;

namespace WaveTech.Scutex.WcfServices.ServicesLibrary.Interfaces.Management
{
	[ServiceContract]
	public interface IStatusService
	{
		[OperationContract]
		string GetServiceStatus(string token);

		[OperationContract]
		string InitializeService(string token, string data);

		[OperationContract]
		string SetupTestProduct(string token, string key);

		[OperationContract]
		string CleanTestProductData(string token);

		[OperationContract]
		string QueryActiveProductsAndLiceseSets(string token);

		[OperationContract]
		string BasicServiceTest();
	}
}
