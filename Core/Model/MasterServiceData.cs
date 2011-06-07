using System;

namespace WaveTech.Scutex.Model
{
	public class MasterServiceData
	{
		public Guid ServiceId { get; set; }
		public string ClientInboundKey { get; set; }
		public string ClientOutboundKey { get; set; }
		public string ManagementInboundKey { get; set; }
		public string ManagementOutboundKey { get; set; }
		public string Token { get; set; }
		public bool Initialized { get; set; }
	}
}