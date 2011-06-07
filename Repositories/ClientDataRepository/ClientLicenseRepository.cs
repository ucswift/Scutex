using System.IO;
using System.Reflection;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Repositories;
using WaveTech.Scutex.Repositories.ClientDataRepository.Properties;

namespace WaveTech.Scutex.Repositories.ClientDataRepository
{
	internal class ClientLicenseRepository : IClientLicenseRepository
	{
		#region Private Readonly Members
		private readonly IObjectSerializationProvider objectSerializationProvider;
		private readonly ISymmetricEncryptionProvider encryptionProvider;
		private readonly string path;
		private readonly EncryptionInfo encryptionInfo;
		#endregion Private Readonly Members

		#region Constructor
		public ClientLicenseRepository(IObjectSerializationProvider objectSerializationProvider, ISymmetricEncryptionProvider encryptionProvider)
		{
			this.objectSerializationProvider = objectSerializationProvider;
			this.encryptionProvider = encryptionProvider;

			path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
			path = path.Replace("file:\\", "");

			encryptionInfo = new EncryptionInfo();
			encryptionInfo.KeySize = 256;
			encryptionInfo.HashAlgorithm = Resources.EncryptionHashValue;
			encryptionInfo.PassPhrase = Resources.EncryptionPassPhrase;
			encryptionInfo.SaltValue = Resources.EncryptionSaltValue;
			encryptionInfo.Iterations = 5;
			encryptionInfo.InitVector = Resources.EncryptionInitVector;
		}
		#endregion Constructor

		#region Private Properties
		private string Name
		{
			get { return path + "\\" + Resources.FileName; }
		}

		private bool FileExists()
		{
			return File.Exists(Name);
		}
		#endregion Private Properties

		#region Get Client License
		public ClientLicense GetClientLicense()
		{
			return GetClientLicense(Name);
		}

		public ClientLicense GetClientLicense(string filePath)
		{
			if (File.Exists(filePath) == false)
				return null;

			string rawFileData;
			using (StreamReader reader = new StreamReader(filePath))
			{
				rawFileData = reader.ReadToEnd();
			}

			string plainTextObjectData;
			plainTextObjectData = encryptionProvider.Decrypt(rawFileData, encryptionInfo);

			ClientLicense sl = objectSerializationProvider.Deserialize<ClientLicense>(plainTextObjectData);

			return sl;
		}
		#endregion Get Client License

		#region Save Client License
		public ClientLicense SaveClientLicense(ClientLicense clientLicense)
		{
			return SaveClientLicense(clientLicense, Name);
		}

		public ClientLicense SaveClientLicense(ClientLicense clientLicense, string filePath)
		{
			if (File.Exists(filePath))
				File.Delete(filePath);

			string plainTextObjectData;
			plainTextObjectData = objectSerializationProvider.Serialize(clientLicense);

			string encryptedObjectData;
			encryptedObjectData = encryptionProvider.Encrypt(plainTextObjectData, encryptionInfo);

			using (StreamWriter writer = new StreamWriter(filePath))
			{
				writer.Write(encryptedObjectData);
			}

			return GetClientLicense(filePath);
		}
		#endregion Save Client License
	}
}