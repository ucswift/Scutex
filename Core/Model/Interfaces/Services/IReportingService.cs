using System.Collections.Generic;

namespace WaveTech.Scutex.Model.Interfaces.Services
{
	public interface IReportingService
	{
		List<ActivationLog> GetAllServiceActivationLogs(Service service);
		List<LicenseActivation> GetAllServiceLicenseActivations(Service service);
	}
}