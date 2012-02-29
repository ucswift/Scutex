using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Wcf;
using WaveTech.Scutex.Model.Results;
using WaveTech.Scutex.WcfServices.ServicesLibrary.Interfaces.Client;

namespace WaveTech.Scutex.WcfServices.ServicesLibrary.Services.Client
{
	public class ActivationService : IActivationService
	{
		#region Private Readonly Members
		private readonly IControlService _controlService;
		private readonly IKeyManagementService _keyService;
		private readonly IKeyPairService _keyPairService;
		private readonly IObjectSerializationProvider _serializationProvider;
		private readonly IAsymmetricEncryptionProvider _asymmetricEncryptionProvider;
		private readonly IActivationLogService _activationLogService;
		private readonly IMasterService _masterService;
		#endregion Private Readonly Members

		#region Constructors
		public ActivationService()
			: this(ObjectLocator.GetInstance<IControlService>(), ObjectLocator.GetInstance<IKeyManagementService>(), ObjectLocator.GetInstance<IKeyPairService>(),
			ObjectLocator.GetInstance<IObjectSerializationProvider>(), ObjectLocator.GetInstance<IAsymmetricEncryptionProvider>(),
			ObjectLocator.GetInstance<IActivationLogService>(), ObjectLocator.GetInstance<IMasterService>())
		{

		}

		public ActivationService(IControlService controlService, IKeyManagementService keyService, IKeyPairService keyPairService,
			IObjectSerializationProvider serializationProvider, IAsymmetricEncryptionProvider asymmetricEncryptionProvider,
			IActivationLogService activationLogService, IMasterService masterService)
		{
			_controlService = controlService;
			_keyService = keyService;
			_keyPairService = keyPairService;
			_serializationProvider = serializationProvider;
			_asymmetricEncryptionProvider = asymmetricEncryptionProvider;
			_activationLogService = activationLogService;
			_masterService = masterService;
		}
		#endregion Constructors

		public string ActivateLicense(string token, string payload)
		{
			ActivationResult result = new ActivationResult();
			KeyPair keyPair = _keyPairService.GetClientKeyPair();
			Debug.WriteLine(keyPair.PrivateKey);
			Debug.WriteLine(keyPair.PublicKey);

			if (!_controlService.ValidateClientToken(token))
			{
				result.WasOperationSuccessful = false;
				result.WasRequestValid = false;
				result.ActivationSuccessful = false;

				return SerializeAndEncryptResult(result, keyPair);
			}

			result.WasRequestValid = true;

			result.ServiceId = _masterService.GetMasterServiceData().ServiceId;

			string decryptedPayload = _asymmetricEncryptionProvider.DecryptPublic(payload, keyPair);
			LicenseActivationPayload activationPayload = _serializationProvider.Deserialize<LicenseActivationPayload>(decryptedPayload);

			if (!_keyService.AuthorizeLicenseForActivation(activationPayload.LicenseKey, activationPayload.ServiceLicense))
			{
				result.WasOperationSuccessful = false;
				result.ActivationSuccessful = false;
			}
			else
			{
				Guid? activiationtoken = _keyService.ActivateLicenseKey(activationPayload.LicenseKey, activationPayload.Token,
																										 activationPayload.ServiceLicense);
				if (activiationtoken.HasValue)
				{
					_activationLogService.LogActiviation(activationPayload.LicenseKey, ActivationResults.Success, GetIpAddress());

					result.ActivationToken = activiationtoken;
					result.WasOperationSuccessful = true;
					result.ActivationSuccessful = true;
				}
				else
				{
					_activationLogService.LogActiviation(activationPayload.LicenseKey, ActivationResults.ActivationFailure, GetIpAddress());

					result.WasOperationSuccessful = false;
					result.ActivationSuccessful = false;
				}
			}

			return SerializeAndEncryptResult(result, keyPair);
		}

		public string BasicServiceTest()
		{
			return "Ok";
		}

		private string SerializeAndEncryptResult(ActivationResult result, KeyPair keyPair)
		{
			string serializedResult = _serializationProvider.Serialize(result);
			string encryptedResult = _asymmetricEncryptionProvider.EncryptPrivate(serializedResult, keyPair);

			return encryptedResult;
		}

		private string GetIpAddress()
		{
			OperationContext context = OperationContext.Current;
			MessageProperties messageProperties = context.IncomingMessageProperties;
			RemoteEndpointMessageProperty endpointProperty =
					messageProperties[RemoteEndpointMessageProperty.Name]
					as RemoteEndpointMessageProperty;

			if (endpointProperty != null)
				return endpointProperty.Address;

			return null;
		}
	}
}