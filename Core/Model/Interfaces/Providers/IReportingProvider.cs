
using WaveTech.Scutex.Model.Results;

namespace WaveTech.Scutex.Model.Interfaces.Providers
{
	public interface IReportingProvider
	{
		GetAllActivationLogsResult GetAllServiceActivationLogs(string url, string token, EncryptionInfo encryptionInfo,
																													 KeyPair serviceKeys);

		GetAllLicenseActivationsResult GetAllServiceLicenseActivations(string url, string token, EncryptionInfo encryptionInfo,
																																	 KeyPair serviceKeys);
	}
}