namespace WaveTech.Scutex.Model
{
	public class TrialInterfaceInteraction
	{
		public bool MoreInfoButtonClicked { get; set; }
		public bool RegisterButtonClicked { get; set; }
		public bool TryButtonClicked { get; set; }
		public bool ExitButtonClicked { get; set; }
		public bool BuyNowUrlClicked { get; set; }
		public bool ProductUrlClicked { get; set; }
		public bool EulaUrlClicked { get; set; }
		public bool ActiviateLicenseClicked { get; set; }
		public int TimesActiviationAttempted { get; set; }
	}
}