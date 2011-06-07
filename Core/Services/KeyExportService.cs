using System;
using System.Collections.Generic;
using System.IO;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Services
{
	internal class KeyExportService : IKeyExportService
	{
		public void ExportKeysToFile(string filePath, string projectName, string licenseSetName, List<string> keys)
		{
			if (File.Exists(filePath))
				File.Delete(filePath);

			using (StreamWriter writer = new StreamWriter(filePath))
			{
				WriteKeyGenerationFileHeader(writer, keys.Count, projectName, licenseSetName);

				foreach (var i in keys)
				{
					writer.WriteLine(i);
				}
			}
		}

		#region Write Key Generation File Header
		private static void WriteKeyGenerationFileHeader(StreamWriter writer, int keyCount, string projectName, string licenseSetName)
		{
			writer.WriteLine("=================================================================");
			writer.WriteLine("                             Scutex                              ");
			writer.WriteLine("=================================================================");
			writer.WriteLine();
			writer.WriteLine(string.Format("Generating System: {0}", Environment.MachineName));
			writer.WriteLine(string.Format("License Project: {0}", projectName));
			writer.WriteLine(string.Format("License Set: {0}", licenseSetName));
			writer.WriteLine(string.Format("Total Keys Generated: {0}", keyCount));
			writer.WriteLine(string.Format("Generation Timestamp: {0}", DateTime.Now));
			writer.WriteLine("-----------------------------------------------------------------");
		}
		#endregion Write Key Generation File Header
	}
}