namespace WaveTech.Scutex.Model.Interfaces.Repositories
{
	/// <summary>
	/// The repoistory for the client license data which will
	/// manages the lifetime of the client license.
	/// </summary>
	public interface IClientLicenseRepository
	{
		ClientLicense GetClientLicense();
		ClientLicense GetClientLicense(string filePath);
		ClientLicense SaveClientLicense(ClientLicense scutexLicense);
		ClientLicense SaveClientLicense(ClientLicense clientLicense, string filePath);
	}
}