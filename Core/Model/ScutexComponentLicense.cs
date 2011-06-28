using System;

namespace WaveTech.Scutex.Model
{
	public class ScutexComponentLicense : System.ComponentModel.License
	{
		public ScutexLicense License { get; set; }

		public ScutexComponentLicense(ScutexLicense license)
		{
			License = license;
		}

		public override void Dispose()
		{
		}

		public override string LicenseKey
		{
			get { return License.LicenseKey; }
		}
	}
}