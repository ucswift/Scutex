using System.Collections.Generic;

namespace WaveTech.Scutex.Model.Interfaces.Wcf
{
	public interface IActivationLogService
	{
		void LogActiviation(string licenseKey, ActivationResults activationResult, string ipAddress);
		List<ActivationLog> GetAllActivationLogs();
	}
}