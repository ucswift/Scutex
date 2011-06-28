using System;
using System.ComponentModel;
using WaveTech.Scutex.Model;
using License = System.ComponentModel.License;

namespace WaveTech.Scutex.Licensing
{
	public class ScutexLicenseProvider : LicenseProvider
	{
		private static LicensingManager _licensingManager;

		public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions)
		{
			ScutexLicense license = null;

			try
			{
				if (context.UsageMode == LicenseUsageMode.Designtime)
				{
					if (_licensingManager != null)
						license = _licensingManager.Validate(InteractionModes.Silent);

					if (license != null)
					{
						if (!license.IsLicenseValid())
						{
							throw new LicenseException(type, instance, "Invalid license or trial expired");
						}
						else
						{
							return new ScutexComponentLicense(license);
						}
					}
				}

				return null;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public static void SetLicensingManager(LicensingManager licenseManager)
		{
			_licensingManager = licenseManager;
		}
	}
}