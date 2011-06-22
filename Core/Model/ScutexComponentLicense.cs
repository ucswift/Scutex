using System;

namespace WaveTech.Scutex.Model
{
	public class ScutexComponentLicense : System.ComponentModel.License
	{
		public override void Dispose()
		{
		}

		public override string LicenseKey
		{
			get
			{
				return "TEMP";
			}
		}
	}
}