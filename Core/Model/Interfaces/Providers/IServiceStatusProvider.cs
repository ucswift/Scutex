using WaveTech.Scutex.Model.Results;

namespace WaveTech.Scutex.Model.Interfaces.Providers
{
	public interface IServiceStatusProvider
	{
		StatusRequestResult GetServiceStatus(string url, string token, EncryptionInfo encryptionInfo);
		InitializationResult InitializeService(string url, string token, MasterServiceData data, EncryptionInfo encryptionInfo);
		SetupTestProductResult SetupTestProduct(string url, string token, string key, EncryptionInfo encryptionInfo);
		SetupTestProductResult CleanUpTestProductData(string url, string token, EncryptionInfo encryptionInfo);
		QueryActiveServiceProductsResult GetActiveServiceProducts(string url, string token, EncryptionInfo encryptionInfo, KeyPair serviceKeys);
	}
}