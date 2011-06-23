namespace WaveTech.Scutex.Model
{
	/// <summary>
	/// Interaction Modes determine how Scutex will present 
	/// information to the user.
	/// </summary>
	public enum InteractionModes
	{
		/// <summary>
		/// Scutex will output data to a log file and the Windows Event log
		/// </summary>
		None = 0,

		/// <summary>
		/// Scutex will display the normal graphical window
		/// </summary>
		Gui = 1,

		/// <summary>
		/// Scutex will output data to the STDOUT console writer
		/// </summary>
		Console = 2,

		/// <summary>
		/// Scutex will not report any data
		/// </summary>
		Silent = 3,

		/// <summary>
		/// Scutex will display a minimal GUI for Control/Component interaction
		/// </summary>
		Component = 4
	}
}