using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WaveTech.Scutex.Providers.NetworkTimeProvider;

namespace WaveTech.Scutex.UnitTests.Providers
{
	[TestClass]
	public class NetworkTimeProviderTests
	{
		[TestMethod]
		public void TestNetworkTime()
		{
			NtpProvider ntpProvider = new NtpProvider();
			DateTime date1 = ntpProvider.GetNetworkTime();
			DateTime date2 = ntpProvider.GetNetworkTime();
			
			Assert.AreNotEqual(date1, date2);
		}
	}
}