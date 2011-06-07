using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Repositories;
using WaveTech.Scutex.Model.Interfaces.Wcf;

namespace WaveTech.Scutex.WcfServices.ServicesLibrary.Services
{
	public class KeyPairService : IKeyPairService
	{
		private readonly ICommonService _commonService;
		private readonly ICommonRepository _commonRepository;

		public KeyPairService(ICommonService commonService, ICommonRepository commonRepository)
		{
			_commonService = commonService;
			_commonRepository = commonRepository;
		}

		public KeyPair GetClientKeyPair()
		{
			MasterServiceData masterData = _commonRepository.GetMasterServiceData();

			StringBuilder inboundKey = new StringBuilder();
			inboundKey.Append(masterData.ClientInboundKey);
			inboundKey.Append(GetInboundHalfFromFile());

			StringBuilder outboundKey = new StringBuilder();
			outboundKey.Append(masterData.ClientOutboundKey);
			outboundKey.Append(GetOutboundHalfFromFile());

			KeyPair keyPair = new KeyPair();
			keyPair.PublicKey = inboundKey.ToString();
			keyPair.PrivateKey = outboundKey.ToString();

			return keyPair;
		}

		public KeyPair GetManagementKeyPair()
		{
			MasterServiceData masterData = _commonRepository.GetMasterServiceData();

			StringBuilder inboundKey = new StringBuilder();
			inboundKey.Append(masterData.ManagementInboundKey);
			inboundKey.Append(GetInboundHalfFromFile());

			StringBuilder outboundKey = new StringBuilder();
			outboundKey.Append(masterData.ManagementOutboundKey);
			outboundKey.Append(GetOutboundHalfFromFile());

			KeyPair keyPair = new KeyPair();
			keyPair.PublicKey = inboundKey.ToString();
			keyPair.PrivateKey = outboundKey.ToString();

			return keyPair;
		}

		public string GetInboundHalfFromFile()
		{
			DirectoryInfo dir = new DirectoryInfo(_commonService.GetPath());

			List<FileInfo> inConfigFiles = new List<FileInfo>();
			inConfigFiles.AddRange(dir.GetFiles("*-IN.config"));

			Debug.WriteLine(inConfigFiles.First().FullName);

			FileStream file1 = new FileStream(inConfigFiles.First().FullName, FileMode.Open, FileAccess.Read);
			StreamReader reader1 = new StreamReader(file1);

			string keyPart = reader1.ReadToEnd().Trim();
			Debug.WriteLine(keyPart);

			return keyPart;
		}

		public string GetOutboundHalfFromFile()
		{
			DirectoryInfo dir = new DirectoryInfo(_commonService.GetPath());

			List<FileInfo> outConfigFiles = new List<FileInfo>();
			outConfigFiles.AddRange(dir.GetFiles("*-OUT.config"));

			Debug.WriteLine(outConfigFiles.First().FullName);

			FileStream file2 = new FileStream(outConfigFiles.First().FullName, FileMode.Open, FileAccess.Read);
			StreamReader reader2 = new StreamReader(file2);

			return reader2.ReadToEnd().Trim();
		}

		public string GetTokenFromFile()
		{
			DirectoryInfo dir = new DirectoryInfo(_commonService.GetPath());

			List<FileInfo> outConfigFiles = new List<FileInfo>();
			outConfigFiles.AddRange(dir.GetFiles("*-TK.config"));

			FileStream file2 = new FileStream(outConfigFiles.First().FullName, FileMode.Open, FileAccess.Read);
			StreamReader reader2 = new StreamReader(file2);

			string token = reader2.ReadToEnd().Trim();
			//Debug.WriteLine(token);

			return token;
		}
	}
}