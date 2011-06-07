using System;

namespace WaveTech.Scutex.Model
{
	public class LicenseActivation
	{
		public int LicenseActivationId { get; set; }
		public int LicenseKeyId { get; set; }
		public Guid ActivationToken { get; set; }
		public Guid? OriginalToken { get; set; }
		public DateTime ActivatedOn { get; set; }
		public string HardwareHash { get; set; }
		public ActivationStatus ActivationStatus { get; set; }
		public DateTime ActivationStatusUpdatedOn { get; set; }
	}
}