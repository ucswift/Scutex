
namespace WaveTech.Scutex.Model
{
	public class RegisterResult
	{
		public LicenseBase ClientLicense { get; set; }
		public ScutexLicense ScutexLicense { get; set; }
		public ProcessCodes Result { get; set; }
	}
}