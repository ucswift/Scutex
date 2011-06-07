using System.Collections.Generic;
using System.IO;
using System.Reflection;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Services
{
	internal class WcfPackagingService : IWcfPackagingService
	{
		private readonly IZipCompressionProvider _zipCompressionProvider;

		public WcfPackagingService(IZipCompressionProvider zipCompressionProvider)
		{
			_zipCompressionProvider = zipCompressionProvider;
		}

		public void PackageService(string saveLocation, Service service)
		{
			if (File.Exists(saveLocation))
				File.Delete(saveLocation);

			string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
			path = path.Replace("file:\\", "");
			path = path + @"\lib\Services\";

			DirectoryInfo csDir = new DirectoryInfo(path + @"ClientService\");
			DirectoryInfo mgmtDir = new DirectoryInfo(path + @"ManagementService\");

			List<FileInfo> configFiles = new List<FileInfo>();
			configFiles.AddRange(csDir.GetFiles("*-IN.config"));
			configFiles.AddRange(csDir.GetFiles("*-OUT.config"));
			configFiles.AddRange(csDir.GetFiles("*-TK.config"));
			configFiles.AddRange(mgmtDir.GetFiles("*-IN.config"));
			configFiles.AddRange(mgmtDir.GetFiles("*-OUT.config"));
			configFiles.AddRange(mgmtDir.GetFiles("*-TK.config"));

			foreach (FileInfo fi in configFiles)
			{
				File.Delete(fi.FullName);
			}

			// Write the Client Service KeyPair's
			using (StreamWriter writer = new StreamWriter(path + @"ClientService\" + string.Format("{0}-IN.config", service.Token)))
			{
				writer.WriteLine(service.GetClientInboundKeyPart2());
			}

			using (StreamWriter writer = new StreamWriter(path + @"ClientService\" + string.Format("{0}-OUT.config", service.Token)))
			{
				writer.WriteLine(service.GetClientOutboundKeyPart2());
			}

			using (StreamWriter writer = new StreamWriter(path + @"ClientService\" + string.Format("{0}-TK.config", service.Token)))
			{
				writer.WriteLine(service.ClientRequestToken);
			}

			// Write the Management Service KeyPair's
			using (StreamWriter writer = new StreamWriter(path + @"ManagementService\" + string.Format("{0}-IN.config", service.Token)))
			{
				writer.WriteLine(service.GetManagementInboundKeyPart2());
			}

			using (StreamWriter writer = new StreamWriter(path + @"ManagementService\" + string.Format("{0}-OUT.config", service.Token)))
			{
				writer.WriteLine(service.GetManagementOutboundKeyPart2());
			}

			using (StreamWriter writer = new StreamWriter(path + @"ManagementService\" + string.Format("{0}-TK.config", service.Token)))
			{
				writer.WriteLine(service.ManagementRequestToken);
			}

			_zipCompressionProvider.CreateZip(saveLocation, path);
		}

		internal void WriteClientKeys(string path, Service service)
		{
			DirectoryInfo dir = new DirectoryInfo(path);

			List<FileInfo> configFiles = new List<FileInfo>();
			configFiles.AddRange(dir.GetFiles("*-IN.config"));
			configFiles.AddRange(dir.GetFiles("*-OUT.config"));
			configFiles.AddRange(dir.GetFiles("*-TK.config"));

			foreach (FileInfo fi in configFiles)
			{
				File.Delete(fi.FullName);
			}

			using (StreamWriter writer = new StreamWriter(path + string.Format("\\{0}-IN.config", service.Token)))
			{
				writer.WriteLine(service.GetClientInboundKeyPart2());
			}

			using (StreamWriter writer = new StreamWriter(path + string.Format("\\{0}-OUT.config", service.Token)))
			{
				writer.WriteLine(service.GetClientOutboundKeyPart2());
			}

			using (StreamWriter writer = new StreamWriter(path + string.Format("\\{0}-TK.config", service.Token)))
			{
				writer.WriteLine(service.ClientRequestToken);
			}
		}

		internal void WriteManagementKeys(string path, Service service)
		{
			DirectoryInfo dir = new DirectoryInfo(path);

			List<FileInfo> configFiles = new List<FileInfo>();
			configFiles.AddRange(dir.GetFiles("*-IN.config"));
			configFiles.AddRange(dir.GetFiles("*-OUT.config"));
			configFiles.AddRange(dir.GetFiles("*-TK.config"));

			foreach (FileInfo fi in configFiles)
			{
				File.Delete(fi.FullName);
			}

			using (StreamWriter writer = new StreamWriter(path + string.Format("\\{0}-IN.config", service.Token)))
			{
				writer.WriteLine(service.GetManagementInboundKeyPart2());
			}

			using (StreamWriter writer = new StreamWriter(path + string.Format("\\{0}-OUT.config", service.Token)))
			{
				writer.WriteLine(service.GetManagementOutboundKeyPart2());
			}

			using (StreamWriter writer = new StreamWriter(path + string.Format("\\{0}-TK.config", service.Token)))
			{
				writer.WriteLine(service.ManagementRequestToken);
			}
		}
	}
}
