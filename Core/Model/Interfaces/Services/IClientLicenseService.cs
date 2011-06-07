namespace WaveTech.Scutex.Model.Interfaces.Services
{
	public interface IClientLicenseService
	{
		ClientLicense GetClientLicense();
		ClientLicense GetClientLicense(string filePath);
		ClientLicense SaveClientLicense(ClientLicense scutexLicense);
		ClientLicense SaveClientLicense(ClientLicense clientLicense, string filePath);
	}
}