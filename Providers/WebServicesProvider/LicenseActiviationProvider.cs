using System.ServiceModel;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Results;
using WaveTech.Scutex.Providers.WebServicesProvider.Properties;
using WaveTech.Scutex.Providers.WebServicesProvider.WcfServices.ActivationService;

namespace WaveTech.Scutex.Providers.WebServicesProvider
{
	internal class LicenseActiviationProvider : ILicenseActiviationProvider
	{
		private readonly IAsymmetricEncryptionProvider _asymmetricEncryptionProvider;
		private readonly IObjectSerializationProvider _objectSerializationProvider;
		private readonly ISymmetricEncryptionProvider _symmetricEncryptionProvider;

		public LicenseActiviationProvider(IAsymmetricEncryptionProvider asymmetricEncryptionProvider, ISymmetricEncryptionProvider symmetricEncryptionProvider,
			IObjectSerializationProvider objectSerializationProvider)
		{
			_asymmetricEncryptionProvider = asymmetricEncryptionProvider;
			_objectSerializationProvider = objectSerializationProvider;
			_symmetricEncryptionProvider = symmetricEncryptionProvider;
		}

		private ActivationServiceClient ActivationServiceClientCreator(string url)
		{
			BasicHttpBinding binding = BindingFactory.CreateBasicHttpBinding();
			EndpointAddress endpointAddress = new EndpointAddress(string.Format(Resources.Client_ActivationServiceUrl, url));

			return new ActivationServiceClient(binding, endpointAddress);
		}

		public ActivationResult ActivateLicense(string url, string token, EncryptionInfo encryptionInfo,
			LicenseActivationPayload payload, ClientLicense clientLicense)
		{
			ActivationServiceClient client = ActivationServiceClientCreator(url);

			string encryptedToken = _symmetricEncryptionProvider.Encrypt(token, encryptionInfo);
			string serializedPayload = _objectSerializationProvider.Serialize(payload);
			string encryptedData = _asymmetricEncryptionProvider.EncryptPrivate(serializedPayload, clientLicense.ServicesKeys);


			string serviceResult = client.ActivateLicense(encryptedToken, encryptedData);
			string result = _asymmetricEncryptionProvider.DecryptPublic(serviceResult, clientLicense.ServicesKeys);

			ActivationResult activationResult = _objectSerializationProvider.Deserialize<ActivationResult>(result);
			return activationResult;
		}
	}
}