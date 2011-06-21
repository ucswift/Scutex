using System;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WaveTech.Scutex.UnitTests.Foundational
{
	[TestClass]
	public class MathTests
	{
		[TestMethod]
		public void LicenseKeyValueObfuscatorMathTest()
		{
			byte[] randomNumber = new byte[1];
			RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();

			for (int i = 0; i < 100000; i++)
			{
				randomNumber = new byte[1];
				Gen.GetBytes(randomNumber);
				int origValue = Convert.ToInt32(randomNumber[0]) % 35;
				
				randomNumber = new byte[1];
				Gen.GetBytes(randomNumber);
				int key = Convert.ToInt32(randomNumber[0]) % 15;

				randomNumber = new byte[1];
				Gen.GetBytes(randomNumber);
				int position = Convert.ToInt32(randomNumber[0]) % 25;

				if (position == 0)
					position = 1;

				Assert.IsFalse(origValue > 35);
				Assert.IsFalse(key > 15);

				// Encode the Key value
				//decimal modValue = Math.Truncate(origValue / 2m);
				//decimal modValue2 = origValue - position;

				double c0;

				if (origValue == 0)
					c0 = Math.PI;
				else
					c0 = origValue;

				double c1 = Math.Log((c0 * (position * 100)));
				double c2 = Math.Sin(c1) + Math.Cos(c1);
				double c3 = Math.Abs(c2 * c1 + (position / 2));
				double c4 = Math.Round(c3, MidpointRounding.ToEven);

				//double modValue = Math.Truncate(Math.Log((origValue * (position * 100))));

				double modValue = c4;

				if (modValue > 15)
				{
					modValue = modValue - (double)Math.Truncate((position/2m));

					if (modValue > 15)
						modValue = 15;
				}

				if (modValue < 0)
				{
					modValue = modValue + (double)Math.Truncate((position / 2m));

					if (modValue < 0)
						modValue = 1;
				}

				Assert.IsFalse(modValue > 15);
				Assert.IsFalse(modValue < 0);

				double obfKey = key + modValue;
				Assert.IsFalse(obfKey > 35);

				// Decode the key value
				//decimal modValue2 = Math.Truncate(origValue / 2m);
				//decimal modValue2 = origValue - position;

				double c0a;

				if (origValue == 0)
					c0a = Math.PI;
				else
					c0a = origValue;

				double c1a = Math.Log((c0a * (position * 100)));
				double c2a = Math.Sin(c1a) + Math.Cos(c1);
				double c3a = Math.Abs(c2a * c1a + (position / 2));
				double c4a = Math.Round(c3a, MidpointRounding.ToEven);

				//double modValue2 = Math.Truncate(Math.Log((origValue * (position * 100))));

				double modValue2 = c4;

				if (modValue2 > 15)
				{
					modValue2 = modValue2 - (double)Math.Truncate((position/2m));

					if (modValue2 > 15)
						modValue2 = 15;
				}

				if (modValue2 < 0)
				{
					modValue2 = modValue2 + (double)Math.Truncate((position / 2m));

					if (modValue2 < 0)
						modValue2 = 1;
				}

				double origKey = obfKey - modValue2;

				Assert.AreEqual(key, origKey);
			}
			
		}
	}
}