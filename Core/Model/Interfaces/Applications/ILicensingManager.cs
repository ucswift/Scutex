namespace WaveTech.Scutex.Model.Interfaces.Applications
{
	public interface ILicensingManager
	{
		ScutexLicense Validate(InteractionModes interactionMode);
		object[] ValidateEx(int interactionMode);
		ScutexLicense Register(string licenseKey);
		object[] RegisterEx(string licenseKey);
	}
}