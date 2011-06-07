using System.Collections.Generic;

namespace WaveTech.Scutex.Model
{
	public class ServiceProduct
	{
		public int LicenseId { get; set; }
		public string LicenseName { get; set; }
		public List<ServiceLicenseSet> LicenseSets { get; set; }
	}
}