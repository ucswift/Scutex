using System.Collections.Generic;

namespace WaveTech.Scutex.Model
{
	public class ServiceLicenseSet
	{
		public int LicenseSetId { get; set; }
		public int LicenseId { get; set; }
		public string LicenseSetName { get; set; }
		public LicenseKeyTypeFlag LicenseType { get; set; }
		public int? MaxUsers { get; set; }

		public List<ServiceLicenseKey> Keys { get; set; }
	}
}