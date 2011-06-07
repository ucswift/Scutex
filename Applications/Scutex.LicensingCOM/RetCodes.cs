
namespace ScutexLicensingCCW
{
	internal class RetCodes
	{
		internal const int SystemNormal = 0;
		internal const int SystemInvalid = 42;
		internal const int TrialExpired = 100;

		internal const int TrialInvalid = 200;
		internal const int TrialInvalidTimeFault = 201;

		internal const int IsLicensedAndActivated = 101;
		internal const int IsLicensedAndNotActivated = 102;
	}

	internal class RegisterRetCodes
	{
		internal const int SystemNormal = 0;
		internal const int SystemInvalid = 42;

		internal const int NotLicensed = 50;

		internal const int IsLicensed = 100;
		internal const int IsLicensedAndActivated = 101;
		internal const int IsLicensedAndNotActivated = 102;
	}
}