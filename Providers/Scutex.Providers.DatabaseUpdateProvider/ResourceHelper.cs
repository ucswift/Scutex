using System.IO;
using System.Reflection;

namespace WaveTech.Scutex.Providers.DatabaseUpdateProvider
{
	internal class ResourceHelper
	{
		private string GetResourceString(string resrouceName)
		{
			Assembly assem = GetType().Assembly;
			string data;

			using (Stream stream = assem.GetManifestResourceStream(resrouceName))
			{
				stream.Position = 0;

				// Now read stream into a byte buffer.
				byte[] bytes = new byte[stream.Length];
				int numBytesToRead = (int)stream.Length;
				int numBytesRead = 0;
				while (numBytesToRead > 0)
				{
					// Read may return anything from 0 to 10.
					int n = stream.Read(bytes, numBytesRead, numBytesToRead);

					// The end of the file is reached.
					if (n == 0)
					{
						break;
					}
					numBytesRead += n;
					numBytesToRead -= n;
				}
				stream.Close();

				data = System.Text.ASCIIEncoding.ASCII.GetString(bytes);
			}

			return data;
		}

		public string GetDatabaseSchema()
		{
			return GetResourceString("WaveTech.Scutex.Providers.DatabaseUpdateProvider.Scutex-CreateDb.sql");
		}

		public string GetDatabaseData()
		{
			return GetResourceString("WaveTech.Scutex.Providers.DatabaseUpdateProvider.Scutex-InsertData.sql");
		}
	}
}