using System.ServiceModel;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Results;
using WaveTech.Scutex.Model.ServiceData;
using WaveTech.Scutex.Providers.WebServicesProvider.Properties;
using WaveTech.Scutex.Providers.WebServicesProvider.WcfServices.ProductsService;

namespace WaveTech.Scutex.Providers.WebServicesProvider
{
	internal class ProductsProvider : IProductsProvider
	{
		private readonly ISymmetricEncryptionProvider _symmetricEncryptionProvider;
		private readonly IObjectSerializationProvider _objectSerializationProvider;
		private readonly IAsymmetricEncryptionProvider _asymmetricEncryptionProvider;

		public ProductsProvider(ISymmetricEncryptionProvider symmetricEncryptionProvider,
			IObjectSerializationProvider objectSerializationProvider, IAsymmetricEncryptionProvider asymmetricEncryptionProvider)
		{
			_symmetricEncryptionProvider = symmetricEncryptionProvider;
			_objectSerializationProvider = objectSerializationProvider;
			_asymmetricEncryptionProvider = asymmetricEncryptionProvider;
		}

		private ProductsServiceClient ProductClientCreator(string url)
		{
			BasicHttpBinding binding = BindingFactory.CreateBasicHttpBinding();
			EndpointAddress endpointAddress = new EndpointAddress(string.Format(Resources.Mgmt_ProductsServiceUrl, url));

			return new ProductsServiceClient(binding, endpointAddress);
		}

		public AddProductResult AddProduct(string url, string token,
			EncryptionInfo encryptionInfo, KeyPair serviceKeys, ServiceProduct product)
		{
			ProductsServiceClient client = ProductClientCreator(url);

			string encryptedToken = _symmetricEncryptionProvider.Encrypt(token, encryptionInfo);
			string serializedPayload = _objectSerializationProvider.Serialize(product);
			string encryptedData = _asymmetricEncryptionProvider.EncryptPrivate(serializedPayload, serviceKeys);


			string encryptedResult = client.AddProduct(encryptedToken, encryptedData);
			string decryptedResult = _asymmetricEncryptionProvider.DecryptPublic(encryptedResult, serviceKeys);

			AddProductResult result = _objectSerializationProvider.Deserialize<AddProductResult>(decryptedResult);

			return result;
		}

		public GetLicenseKeysForProductResult GetLicenseKeysForLicenseSet(string url, string token,
			EncryptionInfo encryptionInfo, KeyPair serviceKeys, GetLicenseKeysForProductData data)
		{
			ProductsServiceClient client = ProductClientCreator(url);

			string encryptedToken = _symmetricEncryptionProvider.Encrypt(token, encryptionInfo);
			string serializedPayload = _objectSerializationProvider.Serialize(data);
			string encryptedData = _asymmetricEncryptionProvider.EncryptPrivate(serializedPayload, serviceKeys);


			string encryptedResult = client.GetLicenseKeysForProduct(encryptedToken, encryptedData);
			string decryptedResult = _asymmetricEncryptionProvider.DecryptPublic(encryptedResult, serviceKeys);

			GetLicenseKeysForProductResult result = _objectSerializationProvider.Deserialize<GetLicenseKeysForProductResult>(decryptedResult);

			return result;
		}

		public AddLicenseKeysForProductResult AddLicenseKeysForLicenseSet(string url, string token,
			EncryptionInfo encryptionInfo, KeyPair serviceKeys, AddLicenseKeysForProductData data)
		{
			ProductsServiceClient client = ProductClientCreator(url);

			string encryptedToken = _symmetricEncryptionProvider.Encrypt(token, encryptionInfo);
			string serializedPayload = _objectSerializationProvider.Serialize(data);
			string encryptedData = _asymmetricEncryptionProvider.EncryptPrivate(serializedPayload, serviceKeys);


			string encryptedResult = client.AddLicenseKeysForProduct(encryptedToken, encryptedData);
			string decryptedResult = _asymmetricEncryptionProvider.DecryptPublic(encryptedResult, serviceKeys);

			AddLicenseKeysForProductResult result = _objectSerializationProvider.Deserialize<AddLicenseKeysForProductResult>(decryptedResult);

			return result;
		}
	}
}