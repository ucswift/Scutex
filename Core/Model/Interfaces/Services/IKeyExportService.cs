using System.Collections.Generic;

namespace WaveTech.Scutex.Model.Interfaces.Services
{
	public interface IKeyExportService
	{
		void ExportKeysToFile(string filePath, string projectName, string licenseSetName, List<string> keys);
	}
}