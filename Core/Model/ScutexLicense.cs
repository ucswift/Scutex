using System;

namespace WaveTech.Scutex.Model
{
	/// <summary>
	/// Result of a program verification check. The ScutexLicense will
	/// contain the current status, licensed or trial, and critical information
	/// regarding the state of the trial or licensed product.
	/// </summary>
	/// <remarks>
	/// The Scutex License object can be useful for letting your customers
	/// know inside your application the current state of the trial, and
	/// displaying reminder screens for them to register when the the trial
	/// is almost over. 
	/// </remarks>
	public class ScutexLicense
	{
		public string LicenseKey { get; set; }

		public bool IsLicensed { get; set; }
		public bool IsActivated { get; set; }

		public DateTime? ActivatedOn { get; set; }
		public DateTime? FirstRun { get; set; }
		public DateTime? LastRun { get; set; }
		public int RunCount { get; set; }
		public bool WasTrialFristRun { get; set; }

		public bool IsCommunityEdition { get; set; }

		public TrialSettings TrialSettings { get; set; }
		public TrailNotificationSettings TrailNotificationSettings { get; set; }

		public bool IsTrialValid { get; set; }
		public TrialFaultReasons TrialFaultReason { get; set; }

		public bool IsTrialExpired { get; set; }
		public int TrialRemaining { get; set; }


		public int TrialUsed
		{
			get
			{
				return int.Parse(TrialSettings.ExpirationData) - TrialRemaining;
			}
		}

		public TrialInterfaceInteraction InterfaceInteraction { get; set; }

		public bool IsLicenseValid()
		{
			bool isValid = true;

			if (IsLicensed == false && IsTrialExpired == true)
				isValid = false;	// Not licensed and the trial is expired

			if (IsLicensed == false && IsTrialExpired == false && IsTrialValid == false)
				isValid = false;	// Not licenses, trial not expired but it isn't valid

			return isValid;
		}
	}
}