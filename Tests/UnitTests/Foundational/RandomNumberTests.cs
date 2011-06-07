using System;
using System.Security.Cryptography;
using NUnit.Framework;

namespace WaveTech.Scutex.UnitTests.Foundational
{
	[TestFixture]
	public class RandomNumberTests
	{
		[Test]
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