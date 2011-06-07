using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace WaveTech.Scutex.WcfServices.ServicesLibrary
{
	public class ErrorHandler : IErrorHandler, IServiceBehavior
	{
		public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
		{
			// Nothing Needed Here
		}

		public bool HandleError(Exception error)
		{
			try
			{
				LoggingHelper.LogException(error);
			}
			catch { }

			return false;
		}

		public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
			// Nothing Needed Here
		}

		public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
		{
			// Nothing Needed Here
		}

		public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
		{
			IErrorHandler errorHandler = new ErrorHandler();

			foreach (ChannelDispatcherBase channelDispatcherBase in serviceHostBase.ChannelDispatchers)
			{
				ChannelDispatcher channelDispatcher = channelDispatcherBase as ChannelDispatcher;

				if (channelDispatcher != null)
				{
					channelDispatcher.ErrorHandlers.Add(errorHandler);
				}
			}
		}
	}
}