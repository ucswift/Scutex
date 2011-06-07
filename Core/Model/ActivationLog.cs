
using System;

namespace WaveTech.Scutex.Model
{
	public class ActivationLog
	{
		public string LicenseKey { get; set; }
		public ActivationResults ActivationResult { get; set; }
		public string IpAddress { get; set; }
		public DateTime Timestamp { get; set; }
	}
}
