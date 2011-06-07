using System;
using System.Diagnostics;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Services;
using WaveTech.Scutex.Model.Interfaces.Wcf;
using WaveTech.Scutex.WcfServices.ServicesLibrary.Properties;

namespace WaveTech.Scutex.WcfServices.ServicesLibrary.Services
{
	public class ControlService : IControlService
	{
		private readonly ISymmetricEncryptionProvider _symmetricEncryptionProvider;
		private readonly IPackingService _packingService;
		private readonly IMasterService _masterService;
		private readonly IKeyPairService _keyPairService;
		private readonly IObjectSerializationProvider _objectSerializationProvider;
		private readonly IAsymmetricEncryptionProvider _asymmetricEncryptionProvider;

		public ControlService(ISymmetricEncryptionProvider symmetricEncryptionProvider, IKeyPairService keyPairService,
			IPackingService packingService, IMasterService masterService, IObjectSerializationProvider objectSerializationProvider,
			IAsymmetricEncryptionProvider asymmetricEncryptionProvider)
		{
			_symmetricEncryptionProvider = symmetricEncryptionProvider;
			_packingService = packingService;
			_masterService = masterService;
			_keyPairService = keyPairService;
			_objectSerializationProvider = objectSerializationProvider;
			_asymmetricEncryptionProvider = asymmetricEncryptionProvider;
		}

		public EncryptionInfo GetStandardEncryptionInfo()
		{
			EncryptionInfo ei = new EncryptionInfo();
			ei.HashAlgorithm = "SHA1";
			ei.InitVector = Resources.IV;
			ei.Iterations = 2;
			ei.KeySize = 192;
			ei.PassPhrase = _keyPairService.GetOutboundHalfFromFile();
			ei.SaltValue = _keyPairService.GetInboundHalfFromFile();

			return ei;
		}

		public bool ValidateClientToken(string token)
		{
			if (String.IsNullOrEmpty(token))
				return false;

			string decryptedToken = _symmetricEncryptionProvider.Decrypt(token, GetStandardEncryptionInfo());
			Token t = _packingService.UnpackToken(decryptedToken);

			if (DateTime.Now - t.Timestamp > new TimeSpan(0, 0, 30, 0))
				return false;

			if (!t.Data.Equals(_keyPairService.GetTokenFromFile()))
				return false;

			return true;
		}

		public bool ValidateManagementToken(string token)
		{
			if (String.IsNullOrEmpty(token))
				return false;

			string decryptedToken = _symmetricEncryptionProvider.Decrypt(token, GetStandardEncryptionInfo());
			Token t = _packingService.UnpackToken(decryptedToken);

			if (DateTime.Now - t.Timestamp > new TimeSpan(0, 0, 30, 0))
				return false;

			if (!t.Data.Equals(_keyPairService.GetTokenFromFile()))
				return false;

			return true;
		}

		public string EncryptSymmetricResponse(string data)
		{
			return _symmetricEncryptionProvider.Encrypt(data, GetStandardEncryptionInfo());
		}

		public string DecryptSymmetricResponse(string data)
		{
			return _symmetricEncryptionProvider.Decrypt(data, GetStandardEncryptionInfo());
		}

		#region Management Communication Helpers
		public string SerializeAndEncryptMgmtOutboundData(object o)
		{
			KeyPair keyPair = _keyPairService.GetManagementKeyPair();

			string serializedData = _objectSerializationProvider.Serialize(o);
			string encryptedData = _asymmetricEncryptionProvider.EncryptPrivate(serializedData, keyPair);

			return encryptedData;
		}

		public T DeserializeAndDencryptMgmtInboundData<T>(string data)
		{
			KeyPair keyPair = _keyPairService.GetManagementKeyPair();

			string unencryptedData = _asymmetricEncryptionProvider.DecryptPublic(data, keyPair);

			return _objectSerializationProvider.Deserialize<T>(unencryptedData);
		}
		#endregion Management Communication Helpers

		#region Client Communication Helpers
		public string SerializeAndEncryptClientOutboundData(object o)
		{
			KeyPair keyPair = _keyPairService.GetClientKeyPair();
			Debug.WriteLine(keyPair.PublicKey);
			Debug.WriteLine(keyPair.PrivateKey);

			string serializedData = _objectSerializationProvider.Serialize(o);
			string encryptedData = _asymmetricEncryptionProvider.EncryptPrivate(serializedData, keyPair);

			return encryptedData;
		}

		public T DeserializeAndDencryptClientInboundData<T>(string data)
		{
			KeyPair keyPair = _keyPairService.GetClientKeyPair();
			Debug.WriteLine(keyPair.PublicKey);
			Debug.WriteLine(keyPair.PrivateKey);

			string unencryptedData = _asymmetricEncryptionProvider.DecryptPublic(data, keyPair);

			return _objectSerializationProvider.Deserialize<T>(unencryptedData);
		}
		#endregion Client Communication Helpers
	}
}