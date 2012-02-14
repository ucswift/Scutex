namespace WaveTech.Scutex.Model.Interfaces.Services
{
	public interface IHardwareFingerprintService
	{
		string GetHardwareFingerprint(FingerprintTypes type);
	}
}