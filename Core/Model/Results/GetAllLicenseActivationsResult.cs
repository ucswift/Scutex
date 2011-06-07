using System.Collections.Generic;

namespace WaveTech.Scutex.Model.Results
{
	public class GetAllLicenseActivationsResult : BaseServiceResult
	{
		public List<LicenseActivation> LicenseActivations { get; set; }
	}
}