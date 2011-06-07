using System;
using System.ServiceModel.Configuration;

namespace WaveTech.Scutex.WcfServices.ServicesLibrary
{
	public class ErrorHandlerElement : BehaviorExtensionElement
	{
		protected override object CreateBehavior()
		{
			return new ErrorHandler();
		}

		public override Type BehaviorType
		{
			get
			{
				return typeof(ErrorHandler);
			}
		}
	}
}