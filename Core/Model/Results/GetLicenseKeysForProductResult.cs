using System.Collections.Generic;

namespace WaveTech.Scutex.Model.Results
{
	public class GetLicenseKeysForProductResult : BaseServiceResult
	{
		public List<string> LicenseKeys { get; set; }
	}
}