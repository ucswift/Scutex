namespace WaveTech.Scutex.Model.Interfaces.Generators
{
	/// <summary>
	/// Interface defining the required operations for a key generation system.
	/// </summary>
	public interface IKeyGenerator
	{
		/// <summary>
		/// Generates a license key using the following options
		/// </summary>
		/// <param name="rsaXmlString"></param>
		/// <param name="scutexLicense"></param>
		/// <param name="generationOptions"></param>
		/// <returns></returns>
		string GenerateLicenseKey(string rsaXmlString, LicenseBase scutexLicense, LicenseGenerationOptions generationOptions);

		/// <summary>
		/// Validates a supplied license key against the ScutexLicense
		/// </summary>
		/// <param name="licenseKey"></param>
		/// <param name="rsaXmlString"></param>
		/// <param name="scutexLicense"></param>
		/// <returns></returns>
		bool ValidateLicenseKey(string licenseKey, LicenseBase scutexLicense);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="licenseKey"></param>
		/// <param name="scutexLicense"></param>
		/// <returns></returns>
		KeyData GetLicenseKeyData(string licenseKey, LicenseBase scutexLicense);

		/// <summary>
		/// Asks the underlying license generation system to report it's
		/// license capabilities. These are used to know if the license system
		/// can support certain features.
		/// </summary>
		/// <returns>
		/// Populated LicenseCapability object which denotes the functionality 
		/// of the underlying license generation system
		/// </returns>
		LicenseCapability GetLicenseCapability();
	}
}