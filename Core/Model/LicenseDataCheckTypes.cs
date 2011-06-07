
namespace WaveTech.Scutex.Model
{
	public enum LicenseDataCheckTypes
	{
		None						= 0,		// No data is Checksum'ed, values used as key fields
		Standard				= 1,		// LicenseId + Product Name
		Customer				= 2,		// Standard + Customer Name and Company Name
		Version					= 3,		// Standard + Product Version
		VersionCompany	= 4		  // Version + Customer
	}
}