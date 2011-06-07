using System;

namespace WaveTech.Scutex.Model
{
	public class LicenseKey
	{
		public long LicenseKeyId { get; set; }
		public int LicenseSetId { get; set; }
		public string Key { get; set; }
		public string HashedLicenseKey { get; set; }
		public DateTime CreatedOn { get; set; }
	}
}