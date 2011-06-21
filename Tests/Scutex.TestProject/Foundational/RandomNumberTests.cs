using System;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WaveTech.Scutex.UnitTests.Foundational
{
	[TestClass]
	public class RandomNumberTests
	{
		[TestMethod]
		public void TestRandomNumberLessThen35()
		{
			for (int i = 0; i < 5000; i++)
			{
				byte[] randomNumber = new byte[1];
				RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();
				Gen.GetBytes(randomNumber);
				int rand = Convert.ToInt32(randomNumber[0]);

				Assert.IsFalse(rand % 35 > 35);
			}
		}
	}
}