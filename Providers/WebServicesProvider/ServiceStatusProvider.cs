using System.ServiceModel;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Results;
using WaveTech.Scutex.Providers.WebServicesProvider.Properties;
using WaveTech.Scutex.Providers.WebServicesProvider.WcfServices.StatusService;

namespace WaveTech.Scutex.Providers.WebServicesProvider
{
	internal class ServiceStatusProvider : IServiceStatusProvider
	{
		private readonly ISymmetricEncryptionProvider _symmetricEncryptionProvider;
		private readonly IObjectSerializationProvider _objectSerializationProvider;
		private readonly IAsymmetricEncryptionProvider _asymmetricEncryptionProvider;

		public ServiceStatusProvider(ISymmetricEncryptionProvider symmetricEncryptionProvider,
			IObjectSerializationProvider objectSerializationProvider, IAsymmetricEncryptionProvider asymmetricEncryptionProvider)
		{
			_symmetricEncryptionProvider = symmetricEncryptionProvider;
			_objectSerializationProvider = objectSerializationProvider;
			_asymmetricEncryptionProvider = asymmetricEncryptionProvider;
		}

		private StatusServiceClient StatusServiceClientCreator(string url)
		{
			BasicHttpBinding binding = BindingFactory.CreateBasicHttpBinding();
			EndpointAddress endpointAddress = new EndpointAddress(string.Format(Resources.Mgmt_StatusServiceUrl, url));

			return new StatusServiceClient(binding, endpointAddress);
		}

		public StatusRequestResult GetServiceStatus(string url, string token, EncryptionInfo encryptionInfo)
		{
			StatusServiceClient client = StatusServiceClientCreator(url);

			string encryptedToken = _symmetricEncryptionProvider.Encrypt(token, encryptionInfo);
			string serviceResult = client.GetServiceStatus(encryptedToken);
			string result = _symmetricEncryptionProvider.Decrypt(serviceResult, encryptionInfo);

			StatusRequestResult statusRequestResult = _objectSerializationProvider.Deserialize<StatusRequestResult>(result);
			return statusRequestResult;
		}

		public InitializationResult InitializeService(string url, string token, MasterServiceData data, EncryptionInfo encryptionInfo)
		{
			StatusServiceClient client = StatusServiceClientCreator(url);

			string encryptedToken = _symmetricEncryptionProvider.Encrypt(token, encryptionInfo);
			string serializedData = _objectSerializationProvider.Serialize(data);
			string encryptedData = _symmetricEncryptionProvider.Encrypt(serializedData, encryptionInfo);

			string result = client.InitializeService(encryptedToken, encryptedData);
			string decryptedResult = _symmetricEncryptionProvider.Decrypt(result, encryptionInfo);

			InitializationResult initializationResult = _objectSerializationProvider.Deserialize<InitializationResult>(decryptedResult);
			return initializationResult;
		}

		public SetupTestProductResult SetupTestProduct(string url, string token, string key, EncryptionInfo encryptionInfo)
		{
			StatusServiceClient client = StatusServiceClientCreator(url);

			string encryptedToken = _symmetricEncryptionProvider.Encrypt(token, encryptionInfo);

			string result = client.SetupTestProduct(encryptedToken, key);
			string decryptedResult = _symmetricEncryptionProvider.Decrypt(result, encryptionInfo);

			SetupTestProductResult setupTestProductResult = _objectSerializationProvider.Deserialize<SetupTestProductResult>(decryptedResult);
			return setupTestProductResult;
		}

		public SetupTestProductResult CleanUpTestProductData(string url, string token, EncryptionInfo encryptionInfo)
		{
			StatusServiceClient client = StatusServiceClientCreator(url);

			string encryptedToken = _symmetricEncryptionProvider.Encrypt(token, encryptionInfo);

			string result = client.CleanTestProductData(encryptedToken);
			string decryptedResult = _symmetricEncryptionProvider.Decrypt(result, encryptionInfo);

			SetupTestProductResult setupTestProductResult = _objectSerializationProvider.Deserialize<SetupTestProductResult>(decryptedResult);
			return setupTestProductResult;
		}

		public QueryActiveServiceProductsResult GetActiveServiceProducts(string url, string token, EncryptionInfo encryptionInfo, KeyPair serviceKeys)
		{
			StatusServiceClient client = StatusServiceClientCreator(url);

			string encryptedToken = _symmetricEncryptionProvider.Encrypt(token, encryptionInfo);

			string encryptedResult = client.QueryActiveProductsAndLiceseSets(encryptedToken);
			string decryptedResult = _asymmetricEncryptionProvider.DecryptPublic(encryptedResult, serviceKeys);

			QueryActiveServiceProductsResult result = _objectSerializationProvider.Deserialize<QueryActiveServiceProductsResult>(decryptedResult);

			return result;
		}

		public string BasicServiceTest(string url)
		{
			StatusServiceClient client = StatusServiceClientCreator(url);
			string result = client.BasicServiceTest();

			return result;
		}
	}
}