using System;
using System.Diagnostics;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Services
{
	internal class EventLogInteractionService : IEventLogInteractionService
	{
		public ScutexLicense Validate(ScutexLicense scutexLicense)
		{
			try
			{
				if (!EventLog.SourceExists("Scutex"))
					EventLog.CreateEventSource("Scutex", "Application");

				if (scutexLicense.IsTrialValid)
				{
					EventLog.WriteEntry("Scutex", "SCUTEX: Your trial is invalid, this can be due to tampering or system changes/corruption.", EventLogEntryType.Warning, 500);
					Environment.Exit(1001);
				}

				if (scutexLicense.WasTrialFristRun)
					EventLog.WriteEntry("Scutex", "SCUTEX: This is your first time running the product, your trail starts now.", EventLogEntryType.Warning, 500);

				if (scutexLicense.IsTrialExpired)
				{
					EventLog.WriteEntry("Scutex", "SCUTEX: Your trial has expired, please buy the product if you wish to continue using it.", EventLogEntryType.Warning, 500);
					Environment.Exit(1001);
				}


			}
			catch { }

			return scutexLicense;
		}
	}
}
