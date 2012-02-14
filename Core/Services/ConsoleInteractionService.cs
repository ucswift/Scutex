
using System;
using System.Threading;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Services;
using WaveTech.Scutex.Services.Properties;

namespace WaveTech.Scutex.Services
{
	internal class ConsoleInteractionService : IConsoleInteractionService
	{
		public ScutexLicense Validate(ScutexLicense scutexLicense)
		{
			WriteScutexHeader();

			if (scutexLicense.IsTrialValid)
			{
				Console.WriteLine(Resources.ConsoleInteractionService_Validate_TrialInvalid);
				Thread.Sleep(2000);
				Environment.Exit(1001);
			}

			if (scutexLicense.WasTrialFristRun)
				Console.WriteLine(Resources.ConsoleInteractionService_Validate_TrialFirstRun);

			if (scutexLicense.IsTrialExpired)
			{
				Console.WriteLine(Resources.ConsoleInteractionService_Validate_TrialExpired);
				Thread.Sleep(2000);
				Environment.Exit(1001);
			}

			return scutexLicense;
		}

		private void WriteScutexHeader()
		{
			Console.WriteLine("===========================================");
			Console.WriteLine("=                 SCUTEX                  =");
			Console.WriteLine("===========================================");
		}
	}
}
