using System.Collections.Generic;

namespace WaveTech.Scutex.Model.ServiceData
{
	public class AddLicenseKeysForProductData
	{
		public int LicenseSetId { get; set; }
		public List<string> Keys { get; set; }
	}
}