using System;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Services
{
	internal class HardwareFingerprintService : IHardwareFingerprintService
	{
		private readonly IWmiDataProvider _wmiDataProvider;
		private readonly IHashingProvider _hashingProvider;

		public HardwareFingerprintService(IWmiDataProvider wmiDataProvider, IHashingProvider hashingProvider)
		{
			_wmiDataProvider = wmiDataProvider;
			_hashingProvider = hashingProvider;
		}

		/// <summary>
		/// Returns the hardware fingerprint for the current system. Requires WMI enabled.
		/// </summary>
		/// <param name="type">Type of hardware to use in the Fingerprint</param>
		/// <returns>Hardware Fingerprint as a String</returns>
		public string GetHardwareFingerprint(FingerprintTypes type)
		{
			string fingerprint = null;

			try
			{
					switch (type)
					{
						case FingerprintTypes.Default:
							fingerprint = _wmiDataProvider.GetPrimaryHardDriveSerial();
							break;
						case FingerprintTypes.HardDriveSerial:
							fingerprint = _wmiDataProvider.GetPrimaryHardDriveSerial();
							break;
						default:
							throw new ArgumentOutOfRangeException("type");
					}
			}
			catch { }

			if (String.IsNullOrEmpty(fingerprint))
			{
				try
				{
					fingerprint = _wmiDataProvider.GetProcessorData();
				}
				catch { }
				
			}

			return _hashingProvider.ComputeHash(fingerprint, "MD5");
		}
	}
}