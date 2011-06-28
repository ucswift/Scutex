namespace WaveTech.Scutex.Model
{
	/// <summary>
	/// Options that can override default LicensingManager behavior
	/// and tune it for specific interactions.
	/// </summary>
	public class LicensingManagerOptions
	{
		/// <summary>
		/// Location to the data file needed by Scutex to perform operations.
		/// Needs to be the full path (including file name an extension)
		/// </summary>
		public string DataFileLocation { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string PublicKey { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string DllHash { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public bool KillOnError { get; set; }

		public LicensingManagerOptions()
		{
			KillOnError = true;
		}
	}
}