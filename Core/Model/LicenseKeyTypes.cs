using System;

namespace WaveTech.Scutex.Model
{
	/// <summary>
	/// Enumeration of the possible license key types that can be used.
	/// This enumeration applys for both static and dynamic licenses and
	/// as such can only have a maximum value of 15.
	/// </summary>
	public enum LicenseKeyTypes
	{
		// Used (Static Keys)
		SingleUser		= 0,		// For a single user/install (Talks to License Server)
		MultiUser			= 1,		// Multiple users/installs (Talks to License Server)
		HardwareLock	= 2,		// Locks to a specific hardware profile (Talks to License Server)
		Unlimited			= 3,		// Unlimited installs, with License Server (Talks to License Server)
		Enterprise		= 4,		// Unlimited installs, no License Server

		// Unused (Dynamic Keys)
		Upgrade				= 5,		// Upgrade license
		UseLimit			= 6,		// # of times the app may be used
		TimeLimit = 7,				// Total Culmative running time
		EndDate				= 8, 		// Max date (i.e. a beta period)
		Unlock				= 9,		// Activates/Unlocks for a set time period

		// Used
		HardwareLockLocal = 10 // Locks a key with a specific hardware fingerprint, does not talk to the server.

		// WARNING: The maximum enumeration value is locked at 15! (F in Hex)
	}

	/// <summary>
	/// Flag Enumeration of the possible license key types that can be used.
	/// This enumeration applies for both static and dynamic licenses and is
	/// used to track capabilities of license types to possible <see cref="LicenseKeyTypes"/>
	/// types.
	/// </summary>
	[Flags]
	public enum LicenseKeyTypeFlag
	{
		None					= 0,			// Disabled
		SingleUser		= 1,			// For a single user/install (Talks to License Server)
		MultiUser			= 2,			// Multiple users/installs (Talks to License Server)
		HardwareLock	= 4,			// Locks to a specific hardware profile (Talks to License Server)
		Unlimited			= 8,			// Unlimited installs, with License Server (Talks to License Server)
		Enterprise		= 16,			// Unlimited installs, no License Server
	}
}