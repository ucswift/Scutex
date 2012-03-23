using System.Diagnostics;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Services;
using WaveTech.Scutex.Services.Properties;

namespace WaveTech.Scutex.Services
{
	internal class SilentInteractionService : ISilentInteractionService
	{
		public ScutexLicense Validate(ScutexLicense scutexLicense)
		{
			if (scutexLicense.IsTrialValid)
			{
				Debug.WriteLine(Resources.ConsoleInteractionService_Validate_TrialInvalid);
			}

			if (scutexLicense.WasTrialFristRun)
				Debug.WriteLine(Resources.ConsoleInteractionService_Validate_TrialFirstRun);

			if (scutexLicense.IsTrialExpired)
			{
				Debug.WriteLine(Resources.ConsoleInteractionService_Validate_TrialExpired);
			}

			return scutexLicense;
		}
	}
}
