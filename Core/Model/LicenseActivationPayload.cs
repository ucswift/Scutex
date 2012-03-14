using System;

namespace WaveTech.Scutex.Model
{
	public class LicenseActivationPayload
	{
		public string LicenseKey { get; set; }
		public Guid? Token { get; set; }
		public string HardwareFingerprint { get; set; }
		public ServiceLicense ServiceLicense { get; set; }
	}
}