using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace WaveTech.Scutex.WcfServices.ServicesLibrary
{
	public class CoreWebServiceHostFactory : ServiceHostFactory
	{
		protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
		{
			Bootstrapper.Configure();

			return base.CreateServiceHost(serviceType, baseAddresses);
		}
	}
}
