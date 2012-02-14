using Microsoft.VisualStudio.TestTools.UnitTesting;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Providers.WmiDataProvider;

namespace WaveTech.Scutex.UnitTests.Providers
{
	[TestClass]
	public class WmiDataProviderTests
	{
		[TestMethod]
		public void GetProcessorData()
		{
			IWmiDataProvider provider = new WmiDataProvider();
			string data = provider.GetProcessorData();

			Assert.IsNotNull(data);
		}

		[TestMethod]
		public void GetMotherboardData()
		{
			IWmiDataProvider provider = new WmiDataProvider();
			string data = provider.GetMotherboardData();

			Assert.IsNotNull(data);
		}

		[TestMethod]
		public void GetBiosData()
		{
			IWmiDataProvider provider = new WmiDataProvider();
			string data = provider.GetBiosData();

			Assert.IsNotNull(data);			
		}

		[TestMethod]
		public void GetPrimaryHardDriveSerial()
		{
			WmiDataProvider provider = new WmiDataProvider();
			string data = provider.GetPrimaryHardDriveSerial();

			Assert.IsNotNull(data);
		}
	}
}