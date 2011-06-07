namespace WaveTech.Scutex.Manager.Classes
{
	public class UploadProductDisplayData
	{
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public int LicenseId { get; set; }
		public string LicenseName { get; set; }
		public int LicenseSetId { get; set; }
		public string LicenseSetName { get; set; }

		public string NameForDisplay
		{
			get { return string.Format("{0} - {1} - {2}", LicenseSetName, LicenseName, ProductName); }
		}
	}
}