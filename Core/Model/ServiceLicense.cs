namespace WaveTech.Scutex.Model
{
	public class ServiceLicense : LicenseBase
	{
		public string HardwareHash { get; set; }

		public ServiceLicense()
		{

		}

		public ServiceLicense(LicenseBase licenseBase)
		{
			base.UniqueId = licenseBase.UniqueId;
			base.KeyPair = licenseBase.KeyPair;

			if (base.KeyPair != null)
				base.KeyPair.PrivateKey = base.KeyPair.PublicKey; // We can't include the private license on the client side.

			base.LicenseSets = licenseBase.LicenseSets;
			base.Product = licenseBase.Product;
			base.TrailNotificationSettings = licenseBase.TrailNotificationSettings;
			base.TrialNotificationType = licenseBase.TrialNotificationType;
			base.TrialSettings = licenseBase.TrialSettings;
			base.KeyGeneratorType = licenseBase.KeyGeneratorType;
		}
	}
}