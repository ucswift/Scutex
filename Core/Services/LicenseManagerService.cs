using System;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Services
{
	internal class LicenseManagerService : ILicenseManagerService
	{
		private readonly IClientLicenseService _clientLicenseService;
		private readonly INetworkTimeProvider _networkTimeProvider;

		public LicenseManagerService(IClientLicenseService clientLicenseService, INetworkTimeProvider networkTimeProvider)
		{
			_clientLicenseService = clientLicenseService;
			_networkTimeProvider = networkTimeProvider;
		}

		public ScutexLicense GetScutexLicense()
		{
			ClientLicense clientLicense = _clientLicenseService.GetClientLicense();

			if (clientLicense == null)
				return null;

			return GetScutexLicense(clientLicense);
		}

		public ScutexLicense GetScutexLicense(string path)
		{
			ClientLicense clientLicense = _clientLicenseService.GetClientLicense(path);

			if (clientLicense == null)
				return null;

			return GetScutexLicense(clientLicense);
		}

		public ScutexLicense GetScutexLicense(ClientLicense clientLicense)
		{
			ScutexLicense scutexLicense = new ScutexLicense();
			scutexLicense.TrialFaultReason = TrialFaultReasons.None;
			scutexLicense.IsTrialValid = true;
			scutexLicense.LastRun = clientLicense.LastRun;
			scutexLicense.TrialSettings = clientLicense.TrialSettings;
			scutexLicense.TrailNotificationSettings = clientLicense.TrailNotificationSettings;
			scutexLicense.IsCommunityEdition = ApplicationConstants.IsCommunityEdition;

			if (!clientLicense.FirstRun.HasValue)
			{
				Logging.LogDebug(string.Format("Is First Run!"));
				clientLicense.FirstRun = _networkTimeProvider.GetNetworkTime();
				scutexLicense.WasTrialFristRun = true;

				Logging.LogDebug(string.Format("FirstRun SetTo: {0}", clientLicense.FirstRun.Value));
			}
			else
			{
				Logging.LogDebug(string.Format("Is Existing Run!"));
				Logging.LogDebug(string.Format("FirstRun Date: {0}", clientLicense.FirstRun.Value));
				Logging.LogDebug(string.Format("NetTime Date: {0}", _networkTimeProvider.GetNetworkTime()));
			}

			if (clientLicense.LastRun.HasValue && _networkTimeProvider.GetNetworkTime() < clientLicense.LastRun)
			{
				Logging.LogDebug(string.Format("TIME FAULT"));
				Logging.LogDebug(string.Format("LastRun Date: {0}", clientLicense.LastRun.Value));
				Logging.LogDebug(string.Format("NetRun Date: {0}", _networkTimeProvider.GetNetworkTime()));
				Logging.LogDebug(string.Format("Delta: {0}", (_networkTimeProvider.GetNetworkTime() - clientLicense.LastRun.Value)));

				scutexLicense.IsTrialValid = false;
				scutexLicense.TrialFaultReason = TrialFaultReasons.TimeFault;
			}

			clientLicense.LastRun = _networkTimeProvider.GetNetworkTime();

			Logging.LogDebug(string.Format("Old Run Count: {0}", clientLicense.RunCount));
			clientLicense.RunCount++;
			Logging.LogDebug(string.Format("New Run Count: {0}", clientLicense.RunCount));

			scutexLicense.FirstRun = clientLicense.FirstRun;

			switch (clientLicense.TrialSettings.ExpirationOptions)
			{
				case TrialExpirationOptions.Days:
					TimeSpan ts = clientLicense.FirstRun.Value -
												_networkTimeProvider.GetNetworkTime().AddDays(-int.Parse(clientLicense.TrialSettings.ExpirationData));

					scutexLicense.TrialRemaining = ts.Days;

					if (ts.Days <= 0)
					{
						scutexLicense.IsTrialExpired = true;
						Logging.LogDebug(string.Format("Trial Expired"));
					}
					else
					{
						scutexLicense.IsTrialExpired = false;
						Logging.LogDebug(string.Format("Trial Not Expired"));
					}

					break;
			}

			// TODO: Need to set a fair amount of data here, like licensed edition, level, etc
			if (clientLicense.IsLicensed)
			{
				scutexLicense.IsLicensed = clientLicense.IsLicensed;
				Logging.LogDebug(string.Format("Product is licensed"));
			}
			else
			{
				Logging.LogDebug(string.Format("Product is not licensed"));
			}

			Logging.LogDebug(string.Format("Saving client license"));
			_clientLicenseService.SaveClientLicense(clientLicense);

			return scutexLicense;
		}

		public ScutexLicense Validate(string licenseKey, ScutexLicense scutexLicense, ClientLicense clientLicense)
		{

			return scutexLicense;
		}

	}
}