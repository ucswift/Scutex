using System;

namespace WaveTech.Scutex.Model.Interfaces.Providers
{
	public interface INetworkTimeProvider
	{
		DateTime GetNetworkTime();
	}
}