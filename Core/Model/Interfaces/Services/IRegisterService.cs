
namespace WaveTech.Scutex.Model.Interfaces.Services
{
	public interface IRegisterService
	{
		RegisterResult Register(string licenseKey, LicenseBase scutexLicense, ScutexLicense license);
	}
}