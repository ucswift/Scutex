namespace WaveTech.Scutex.Model.Interfaces.Services
{
	public interface IValidationService
	{
		ValidationResult IsLicenseValidForSaving(License license);
		ValidationResult IsLicenseStateValid(License license);
	}
}