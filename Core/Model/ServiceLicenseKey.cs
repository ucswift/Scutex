using System;

namespace WaveTech.Scutex.Model
{
	public class ServiceLicenseKey
	{
		public int LicenseKeyId { get; set; }
		public int LicenseSetId { get; set; }
		public string Key { get; set; }
		public DateTime CreatedOn { get; set; }
		public int ActivationCount { get; set; }
		public bool Deactivated { get; set; }
		public int? DeactivatedReason { get; set; }
		public DateTime? DeactivatedOn { get; set; }
	}
}