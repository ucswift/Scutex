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

		internal HardwareFingerprintService(IWmiDataProvider wmiDataProvider, IHashingProvider hashingProvider)
		{
			_wmiDataProvider = wmiDataProvider;
			_hashingProvider = hashingProvider;
		}

		public string GetHardwareFingerprint(FingerprintTypes type)
		{
			switch (type)
			{
				case FingerprintTypes.Default:
					return _wmiDataProvider.GetPrimaryHardDriveSerial();
				case FingerprintTypes.HardDriveSerial:
					return _wmiDataProvider.GetPrimaryHardDriveSerial();
				default:
					throw new ArgumentOutOfRangeException("type");
			}
		}
	}
}