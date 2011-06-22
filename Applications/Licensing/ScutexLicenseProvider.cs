using System;
using System.ComponentModel;
using WaveTech.Scutex.Model;
using License = System.ComponentModel.License;

namespace WaveTech.Scutex.Licensing
{
	public class ScutexLicenseProvider : LicenseProvider
	{
		public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
		{
			bool isLicenseValid = true;

			if (!isLicenseValid)
			{
				throw new LicenseException(type, instance, "Invalid license.");
			}
			else
			{
				return new ScutexComponentLicense();
			}
		}
	}
}