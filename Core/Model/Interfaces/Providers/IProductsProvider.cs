using WaveTech.Scutex.Model.Results;
using WaveTech.Scutex.Model.ServiceData;

namespace WaveTech.Scutex.Model.Interfaces.Providers
{
	public interface IProductsProvider
	{
		AddProductResult AddProduct(string url, string token, EncryptionInfo encryptionInfo, KeyPair serviceKeys, ServiceProduct product);

		GetLicenseKeysForProductResult GetLicenseKeysForLicenseSet(string url, string token,
																															 EncryptionInfo encryptionInfo, KeyPair serviceKeys,
																															 GetLicenseKeysForProductData data);

		AddLicenseKeysForProductResult AddLicenseKeysForLicenseSet(string url, string token,
																															 EncryptionInfo encryptionInfo, KeyPair serviceKeys,
																															 AddLicenseKeysForProductData data);
	}
}