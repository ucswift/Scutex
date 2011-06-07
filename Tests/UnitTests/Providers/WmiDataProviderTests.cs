using NUnit.Framework;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Providers.WmiDataProvider;

namespace WaveTech.Scutex.UnitTests.Providers
{
	[TestFixture]
	public class WmiDataProviderTests
	{
		[Test]
		public void GetProcessorData()
		{
			IWmiDataProvider provider = new WmiDataProvider();
			string data = provider.GetProcessorData();

			Assert.IsNotNull(data);
		}

		[Test]
		public void GetMotherboardData()
		{
			IWmiDataProvider provider = new WmiDataProvider();
			string data = provider.GetMotherboardData();

			Assert.IsNotNull(data);
		}

		[Test]
		public void GetBiosData()
		{
			IWmiDataProvider provider = new WmiDataProvider();
			string data = provider.GetBiosData();

			Assert.IsNotNull(data);			
		}
	}
}