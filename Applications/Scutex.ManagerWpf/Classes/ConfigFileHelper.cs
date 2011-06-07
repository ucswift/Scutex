
using System.Configuration;
using System.IO;
using System.Reflection;

namespace WaveTech.Scutex.Manager.Classes
{
	internal class ConfigFileHelper
	{
		internal void SaveConfigFile(string connectionString)
		{
			Assembly assem = GetType().Assembly;

			using (Stream stream = assem.GetManifestResourceStream("WaveTech.Scutex.Manager.Resources.ConfigFile.txt"))
			{
				stream.Position = 0;

				// Now read stream into a byte buffer.
				byte[] bytes = new byte[stream.Length];
				int numBytesToRead = (int)stream.Length;
				int numBytesRead = 0;
				while (numBytesToRead > 0)
				{
					int n = 0;

					if (numBytesRead + numBytesToRead < bytes.Length + 1)
						n = stream.Read(bytes, numBytesRead, numBytesToRead);

					// The end of the file is reached.
					if (n == 0)
					{
						break;
					}
					numBytesRead += n;
					numBytesToRead -= n;
				}
				stream.Close();

				string data = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
				data = data.Replace("???", "");
				data = data.Replace("{ConnectionString}", connectionString);

				string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
				path = path.Replace("file:\\", "");

				if (File.Exists(path + "\\LicensingManager.exe.config"))
					File.Delete(path + "\\LicensingManager.exe.config");

				using (StreamWriter outStream = new StreamWriter(path + "\\LicensingManager.exe.config"))
				{
					outStream.Write(data);
				}
			}
		}

		internal void SaveConfigFileForSqlExpress2005(string server)
		{
			Assembly assem = GetType().Assembly;

			using (Stream stream = assem.GetManifestResourceStream("WaveTech.Scutex.Manager.Resources.ConfigFileSQE2005.txt"))
			{
				stream.Position = 0;

				// Now read stream into a byte buffer.
				byte[] bytes = new byte[stream.Length];
				int numBytesToRead = (int)stream.Length;
				int numBytesRead = 0;
				while (numBytesToRead > 0)
				{
					int n = 0;

					if (numBytesRead + numBytesToRead < bytes.Length + 1)
						n = stream.Read(bytes, numBytesRead, numBytesToRead);

					// The end of the file is reached.
					if (n == 0)
					{
						break;
					}
					numBytesRead += n;
					numBytesToRead -= n;
				}
				stream.Close();

				string data = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
				data = data.Replace("???", "");
				data = data.Replace("{Server}", server);

				string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
				path = path.Replace("file:\\", "");

				if (File.Exists(path + "\\LicensingManager.exe.config"))
					File.Delete(path + "\\LicensingManager.exe.config");

				using (StreamWriter outStream = new StreamWriter(path + "\\LicensingManager.exe.config"))
				{
					outStream.Write(data);
				}
			}
		}

		internal void SaveConfigFileForSqlExpress2008(string server)
		{
			Assembly assem = GetType().Assembly;

			using (Stream stream = assem.GetManifestResourceStream("WaveTech.Scutex.Manager.Resources.ConfigFileSQE2008.txt"))
			{
				stream.Position = 0;

				// Now read stream into a byte buffer.
				byte[] bytes = new byte[stream.Length];
				int numBytesToRead = (int)stream.Length;
				int numBytesRead = 0;
				while (numBytesToRead > 0)
				{
					int n = 0;

					if (numBytesRead + numBytesToRead < bytes.Length + 1)
						n = stream.Read(bytes, numBytesRead, numBytesToRead);

					// The end of the file is reached.
					if (n == 0)
					{
						break;
					}
					numBytesRead += n;
					numBytesToRead -= n;
				}
				stream.Close();

				string data = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
				data = data.Replace("???", "");
				data = data.Replace("{Server}", server);

				string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
				path = path.Replace("file:\\", "");

				if (File.Exists(path + "\\LicensingManager.exe.config"))
					File.Delete(path + "\\LicensingManager.exe.config");

				using (StreamWriter outStream = new StreamWriter(path + "\\LicensingManager.exe.config"))
				{
					outStream.Write(data);
				}
			}
		}

		internal static string GetConnectionString()
		{
			string temp = ConfigurationManager.ConnectionStrings["ScutexEntities"].ConnectionString;
			int index = temp.IndexOf("\"");
			int length = temp.Length;

			return temp.Substring(index + 1, 91).Trim();
		}
	}
}