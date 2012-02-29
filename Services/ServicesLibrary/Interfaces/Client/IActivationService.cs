using System.ServiceModel;

namespace WaveTech.Scutex.WcfServices.ServicesLibrary.Interfaces.Client
{
	[ServiceContract]
	public interface IActivationService
	{
		[OperationContract]
		string ActivateLicense(string token, string payload);

		[OperationContract]
		string BasicServiceTest();
	}
}