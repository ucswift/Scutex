using System;
using WaveTech.Scutex.Licensing;
using WaveTech.Scutex.Model;

namespace CSharpConsoleExample
{
	class Program
	{
		static void Main(string[] args)
		{
			LicensingManager licensingManager = new LicensingManager(null);
			ScutexLicense license = licensingManager.Validate();


			bool wasPressed = license.InterfaceInteraction.TryButtonClicked;
			Console.WriteLine(Guid.NewGuid().ToString());
			Console.WriteLine("Test Product");
			Console.WriteLine();

			Console.WriteLine(String.Format("Is Product Licensed: {0}", license.IsLicensed));
			Console.WriteLine(String.Format("Is Trial Valid: {0}", license.IsTrialValid));
			Console.WriteLine(String.Format("Is Trial Expired: {0}", license.IsTrialExpired));
			Console.WriteLine(String.Format("Is Trial Fault Reason: {0}", license.TrialFaultReason));
			Console.WriteLine(String.Format("Is Trial Remaining: {0}", license.TrialRemaining));
			Console.WriteLine(String.Format("Is Trial Elapsed: {0}", license.TrialUsed));
			Console.WriteLine(String.Format("Is Trial First Run: {0}", license.WasTrialFristRun));

			Console.WriteLine();
			Console.WriteLine("Press ENTER to exit.");
			Console.ReadLine();
		}
	}
}