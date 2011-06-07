
using System;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Services
{
	internal class ValidationService : IValidationService
	{
		public ValidationResult IsLicenseValidForSaving(License license)
		{
			ValidationResult result = new ValidationResult();
			result.IsValid = true;

			if (String.IsNullOrEmpty(license.Name))
			{
				result.IsValid = false;
				result.ValidationErrors.Add("Project Name cannot be null.");
			}

			if (license.Product == null)
			{
				result.IsValid = false;
				result.ValidationErrors.Add("License project must contain a Product.");
			}

			if (license.KeyGeneratorType == KeyGeneratorTypes.None)
			{
				result.IsValid = false;
				result.ValidationErrors.Add("You must select a valid License Key Generator type.");
			}

			if (license.TrialSettings == null ||
				license.TrialSettings.ExpirationOptions == TrialExpirationOptions.None ||
				String.IsNullOrEmpty(license.TrialSettings.ExpirationData))
			{
				result.IsValid = false;
				result.ValidationErrors.Add("You must select a Trial Expiration type.");
			}

			return result;
		}

		public ValidationResult IsLicenseStateValid(License license)
		{
			ValidationResult result = new ValidationResult();
			result.IsValid = true;

			if (license.LicenseSets != null && license.LicenseSets.Count > 0)
			{
				bool isNonEnterprise = false;

				foreach (LicenseSet ls in license.LicenseSets)
				{
					if (ls.SupportedLicenseTypes.IsSet(LicenseKeyTypeFlag.SingleUser) ||
							ls.SupportedLicenseTypes.IsSet(LicenseKeyTypeFlag.MultiUser) ||
							ls.SupportedLicenseTypes.IsSet(LicenseKeyTypeFlag.HardwareLock) ||
							ls.SupportedLicenseTypes.IsSet(LicenseKeyTypeFlag.Unlimited))
					{
						isNonEnterprise = true;
					}
				}

				if (isNonEnterprise)
				{
					if (license.Service == null)
					{
						result.IsValid = false;
						result.ValidationErrors.Add("A LicenseSet exists that is non-Enterprise without a service tied to the license.");
					}
				}
			}

			return result;
		}
	}
}
