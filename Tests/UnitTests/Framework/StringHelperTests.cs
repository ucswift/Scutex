using NUnit.Framework;
using WaveTech.Scutex.Framework;

namespace WaveTech.Scutex.UnitTests.Framework
{
	[TestFixture]
	public class StringHelperTests
	{
		[Test]
		public void ShouldValidateForLocalhostUrl()
		{
			Assert.IsTrue(StringHelpers.IsValidUrl("http://localhost/Client/"));
		}

		[Test]
		public void ShouldValidateForLocalhostSSLUrl()
		{
			Assert.IsTrue(StringHelpers.IsValidUrl("https://localhost/Client/"));
		}

		[Test]
		public void ShouldNotValidateForLocalhostUrlWihtoutSlash()
		{
			Assert.IsFalse(StringHelpers.IsValidUrl("http://localhost/Client"));
		}

		[Test]
		public void ShouldNotValidateForLocalhostSSLUrlWihtoutSlash()
		{
			Assert.IsFalse(StringHelpers.IsValidUrl("https://localhost/Client"));
		}

		[Test]
		public void ShouldValidateForValidUrl()
		{
			Assert.IsTrue(StringHelpers.IsValidUrl("http://www.google.com/Client/"));
		}

		[Test]
		public void ShouldValidateForValidSSLUrl()
		{
			Assert.IsTrue(StringHelpers.IsValidUrl("https://www.google.com/Client/"));
		}

		[Test]
		public void ShouldNotValidateForInvalidUrl()
		{
			Assert.IsFalse(StringHelpers.IsValidUrl("http://google/Client/"));
		}

		[Test]
		public void ShouldNotValidateForInvalidSSLUrl()
		{
			Assert.IsFalse(StringHelpers.IsValidUrl("https://google/Client/"));
		}

		[Test]
		public void ShouldValidateForValidUrlWithoutWww()
		{
			Assert.IsTrue(StringHelpers.IsValidUrl("http://google.com/Client/"));
		}

		[Test]
		public void ShouldValidateForValidSSLUrlWithoutWww()
		{
			Assert.IsTrue(StringHelpers.IsValidUrl("https://google.com/Client/"));
		}
	}
}