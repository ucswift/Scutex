using System.ServiceModel;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Results;
using WaveTech.Scutex.Providers.WebServicesProvider.Properties;
using WaveTech.Scutex.Providers.WebServicesProvider.WcfServices.ReportingService;

namespace WaveTech.Scutex.Providers.WebServicesProvider
{
	internal class ReportingProvider : IReportingProvider
	{
		private readonly ISymmetricEncryptionProvider _symmetricEncryptionProvider;
		private readonly IObjectSerializationProvider _objectSerializationProvider;
		private readonly IAsymmetricEncryptionProvider _asymmetricEncryptionProvider;

		public ReportingProvider(ISymmetricEncryptionProvider symmetricEncryptionProvider,
			IObjectSerializationProvider objectSerializationProvider, IAsymmetricEncryptionProvider asymmetricEncryptionProvider)
		{
			_symmetricEncryptionProvider = symmetricEncryptionProvider;
			_objectSerializationProvider = objectSerializationProvider;
			_asymmetricEncryptionProvider = asymmetricEncryptionProvider;
		}

		private ReportingServiceClient ReportingClientCreator(string url)
		{
			BasicHttpBinding binding = BindingFactory.CreateBasicHttpBinding();
			EndpointAddress endpointAddress = new EndpointAddress(string.Format(Resources.Mgmt_ReportingServiceUrl, url));

			return new ReportingServiceClient(binding, endpointAddress);
		}

		public GetAllActivationLogsResult GetAllServiceActivationLogs(string url, string token, EncryptionInfo encryptionInfo, KeyPair serviceKeys)
		{
			ReportingServiceClient client = ReportingClientCreator(url);

			string encryptedToken = _symmetricEncryptionProvider.Encrypt(token, encryptionInfo);

			string encryptedResult = client.GetAllActivationLogs(encryptedToken);
			string decryptedResult = _asymmetricEncryptionProvider.DecryptPublic(encryptedResult, serviceKeys);

			GetAllActivationLogsResult result = _objectSerializationProvider.Deserialize<GetAllActivationLogsResult>(decryptedResult);

			return result;
		}

		public GetAllLicenseActivationsResult GetAllServiceLicenseActivations(string url, string token, EncryptionInfo encryptionInfo, KeyPair serviceKeys)
		{
			ReportingServiceClient client = ReportingClientCreator(url);

			string encryptedToken = _symmetricEncryptionProvider.Encrypt(token, encryptionInfo);

			string encryptedResult = client.GetAllLicenseActivations(encryptedToken);
			string decryptedResult = _asymmetricEncryptionProvider.DecryptPublic(encryptedResult, serviceKeys);

			GetAllLicenseActivationsResult result = _objectSerializationProvider.Deserialize<GetAllLicenseActivationsResult>(decryptedResult);

			return result;
		}
	}
}