namespace WaveTech.Scutex.Model
{
	/// <summary>
	/// Reasons why a trial status may be in fault. Fault reasons can vary
	/// from transient systems issues to more serious issues that may be
	/// an indication that someone is trying to bypass the trial
	/// </summary>
	public enum TrialFaultReasons
	{
		/// <summary>
		/// No trial faults were detected. This is the
		/// default no fault state
		/// </summary>
		None = 0,

		/// <summary>
		/// The last time the trial was verified is greater then the current
		/// time, which means the last run time is invalid due to a system
		/// issue or tampering.
		/// </summary>
		TimeFault = 1
	}
}