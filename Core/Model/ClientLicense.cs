using System;

namespace WaveTech.Scutex.Model
{
	/// <summary>
	/// The client license contains core data used by the generation systems
	/// and client activity.
	/// </summary>
	public class ClientLicense : LicenseBase
	{
		public bool IsLicensed { get; set; }
		public bool IsActivated { get; set; }

		public Guid? ActivationToken { get; set; }
		public DateTime? ActivatedOn { get; set; }
		public Guid? ActivatingServiceId { get; set; }
		public DateTime? ActivationLastCheckedOn { get; set; }

		public DateTime? FirstRun { get; set; }
		public DateTime? LastRun { get; set; }
		public int RunCount { get; set; }

		public string LicenseCode { get; set; }
		public bool IsCommunityEdition { get; set; }

		public LicenseSet LicenseLevel { get; set; }

		public string BuyNowUrl { get; set; }
		public string ProductUrl { get; set; }
		public string EulaUrl { get; set; }
		public string SupportEmail { get; set; }
		public string SalesEmail { get; set; }

		public string CustomFilename { get; set; }

		public KeyPair ServicesKeys { get; set; }
		public string Ces1 { get; set; }
		public string Ces2 { get; set; }
		public string ServiceAddress { get; set; }
		public string ServiceToken { get; set; }

		public ClientLicense()
		{

		}

		public ClientLicense(LicenseBase licenseBase)
		{
			base.UniqueId = licenseBase.UniqueId;
			base.KeyPair = licenseBase.KeyPair;
			base.KeyPair.PrivateKey = base.KeyPair.PublicKey; // We can't include the private license on the client side.

			base.LicenseSets = licenseBase.LicenseSets;
			base.Product = licenseBase.Product;
			base.TrailNotificationSettings = licenseBase.TrailNotificationSettings;
			base.TrialNotificationType = licenseBase.TrialNotificationType;
			base.TrialSettings = licenseBase.TrialSettings;
			base.KeyGeneratorType = licenseBase.KeyGeneratorType;
		}

		public ClientLicense(License license)
			: this((LicenseBase)license)
		{
			BuyNowUrl = license.BuyNowUrl;
			ProductUrl = license.ProductUrl;
			EulaUrl = license.EulaUrl;
			SupportEmail = license.SupportEmail;
			SalesEmail = license.SalesEmail;

			if (license.Service != null)
			{
				ServicesKeys = license.Service.GetClientServiceKeyPair();
				Ces1 = license.Service.GetClientOutboundKeyPart2();
				Ces2 = license.Service.GetClientInboundKeyPart2();

				ServiceAddress = license.Service.ClientUrl;
				ServiceToken = license.Service.ClientRequestToken;
			}
		}
	}
}