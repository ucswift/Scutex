using System.ServiceModel;

namespace WaveTech.Scutex.WcfServices.ServicesLibrary.Interfaces.Management
{
	[ServiceContract]
	public interface IReportingService
	{
		[OperationContract]
		string GetAllActivationLogs(string token);

		[OperationContract]
		string GetAllLicenseActivations(string token);
	}
}