namespace WaveTech.Scutex.Model
{
	public class LicenseCapability
	{
		public LicenseKeyTypeFlag SupportedLicenseKeyTypes { get; set; }
		public int MaxLicenseKeysPerBatch { get; set; }
		public int MaxTotalLicenseKeys { get; set; }
	}
}