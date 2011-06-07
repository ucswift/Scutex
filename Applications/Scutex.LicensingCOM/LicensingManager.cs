using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ScutexLicensingCCW
{
	[Guid("42B9DD83-1A24-4CFD-A996-E3D40889BAD3")]
	[ClassInterface(ClassInterfaceType.None)]
	[ProgId("ScutexLicensingCCW.LicensingManager")]
	[ComVisible(true)]
	public class LicensingManager : _LicensingManager
	{
		public LicensingManager() { }

		public bool Prepare(string p1, string p2)
		{
			WaveTech.Scutex.Providers.COM.ComBypassProvider.ComBypass bypass = new WaveTech.Scutex.Providers.COM.ComBypassProvider.ComBypass();

			if (bypass.IsComBypassEnabled())
				bypass.RemoveComBypass();

			bypass.SetComBypass(p1, p2);

			WaveTech.Scutex.Licensing.LicensingManager _licensingMgr = null;

			try
			{
				_licensingMgr = new WaveTech.Scutex.Licensing.LicensingManager();
			}
			catch (Exception ex)
			{
				try
				{
					if (!EventLog.SourceExists("Scutex"))
						EventLog.CreateEventSource("Scutex", "Application");

					EventLog.WriteEntry("Scutex", ex.ToString(), EventLogEntryType.Warning, 500);
				}
				catch { }

				return false;
			}

			if (_licensingMgr == null)
				return false;

			return true;
		}

		public int Validate(int interactionMode)
		{
			object[] data;

			try
			{
				WaveTech.Scutex.Licensing.LicensingManager _licensingMgr = new WaveTech.Scutex.Licensing.LicensingManager();
				data = _licensingMgr.ValidateEx(interactionMode);
			}
			catch
			{
				return RetCodes.SystemInvalid;
			}

			if ((bool)data[0] == false)
				return RetCodes.SystemInvalid;

			if ((bool)data[1])
				return RetCodes.TrialExpired;

			if ((bool)data[2] == false)
			{
				switch ((int)data[3])
				{
					case 1:
						return RetCodes.TrialInvalidTimeFault;
				}
			}

			if ((bool)data[4] == true)
			{
				if ((bool)data[5] == true)
					return RetCodes.IsLicensedAndActivated;

				return RetCodes.IsLicensedAndNotActivated;
			}

			return RetCodes.SystemNormal;
		}

		public int Register(string key)
		{
			object[] data;

			try
			{
				WaveTech.Scutex.Licensing.LicensingManager licensingManager = new WaveTech.Scutex.Licensing.LicensingManager();
				data = licensingManager.RegisterEx(key);
			}
			catch
			{
				return RegisterRetCodes.SystemInvalid;
			}

			if ((bool)data[1] == false)
				return RegisterRetCodes.SystemInvalid;

			if ((bool)data[1])
			{
				if ((bool)data[2])
					return RegisterRetCodes.IsLicensedAndActivated;

				return RegisterRetCodes.IsLicensedAndNotActivated;
			}

			return RegisterRetCodes.NotLicensed;
		}
	}
}