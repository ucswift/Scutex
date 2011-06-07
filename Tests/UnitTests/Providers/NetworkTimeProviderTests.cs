using System;
using NUnit.Framework;
using WaveTech.Scutex.Providers.NetworkTimeProvider;

namespace WaveTech.Scutex.UnitTests.Providers
{
	[TestFixture]
	public class NetworkTimeProviderTests
	{
		[Test]
		public void TestNetworkTime()
		{
			NtpProvider ntpProvider = new NtpProvider();
			DateTime date1 = ntpProvider.GetNetworkTime();
			DateTime date2 = ntpProvider.GetNetworkTime();
			
			Assert.AreNotEqual(date1, date2);
		}
	}
}