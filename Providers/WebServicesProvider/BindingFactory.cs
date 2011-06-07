using System.ServiceModel;
using System.Xml;

namespace WaveTech.Scutex.Providers.WebServicesProvider
{
	internal static class BindingFactory
	{
		internal static BasicHttpBinding CreateBasicHttpBinding()
		{
			BasicHttpBinding binding = new BasicHttpBinding();
			binding.MaxReceivedMessageSize = 256 * 1024;
			binding.ReaderQuotas = new XmlDictionaryReaderQuotas
			{
				MaxStringContentLength = 64 * 1024
			};

			return binding;
		}
	}
}