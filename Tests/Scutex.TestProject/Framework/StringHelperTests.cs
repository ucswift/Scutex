using Microsoft.VisualStudio.TestTools.UnitTesting;
using WaveTech.Scutex.Framework;

namespace WaveTech.Scutex.UnitTests.Framework
{
	[TestClass]
	public class StringHelperTests
	{
		[TestMethod]
		public void ShouldValidateForLocalhostUrl()
		{
			Assert.IsTrue(StringHelpers.IsValidUrl("http://localhost/Client/"));
		}

		[TestMethod]
		public void ShouldValidateForLocalhostSSLUrl()
		{
			Assert.IsTrue(StringHelpers.IsValidUrl("https://localhost/Client/"));
		}

		[TestMethod]
		public void ShouldNotValidateForLocalhostUrlWihtoutSlash()
		{
			Assert.IsFalse(StringHelpers.IsValidUrl("http://localhost/Client"));
		}

		[TestMethod]
		public void ShouldNotValidateForLocalhostSSLUrlWihtoutSlash()
		{
			Assert.IsFalse(StringHelpers.IsValidUrl("https://localhost/Client"));
		}

		[TestMethod]
		public void ShouldValidateForValidUrl()
		{
			Assert.IsTrue(StringHelpers.IsValidUrl("http://www.google.com/Client/"));
		}

		[TestMethod]
		public void ShouldValidateForValidSSLUrl()
		{
			Assert.IsTrue(StringHelpers.IsValidUrl("https://www.google.com/Client/"));
		}

		[TestMethod]
		public void ShouldNotValidateForInvalidUrl()
		{
			Assert.IsFalse(StringHelpers.IsValidUrl("http://google/Client/"));
		}

		[TestMethod]
		public void ShouldNotValidateForInvalidSSLUrl()
		{
			Assert.IsFalse(StringHelpers.IsValidUrl("https://google/Client/"));
		}

		[TestMethod]
		public void ShouldValidateForValidUrlWithoutWww()
		{
			Assert.IsTrue(StringHelpers.IsValidUrl("http://google.com/Client/"));
		}

		[TestMethod]
		public void ShouldValidateForValidSSLUrlWithoutWww()
		{
			Assert.IsTrue(StringHelpers.IsValidUrl("https://google.com/Client/"));
		}
	}
}