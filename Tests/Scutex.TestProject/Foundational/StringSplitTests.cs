using Microsoft.VisualStudio.TestTools.UnitTesting;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Providers.AsymmetricEncryptionProvider;

namespace WaveTech.Scutex.UnitTests.Foundational
{
	[TestClass]
	public class StringSplitTests
	{
		[TestMethod]
		public void SplitDataSouldEqualOriginal()
		{
			AsymmetricEncryptionProvider provider = new AsymmetricEncryptionProvider();
			KeyPair kp = provider.GenerateKeyPair(BitStrengths.High);

			string inKey1 = kp.PublicKey.Substring(0, (kp.PublicKey.Length/2));
			string inKey2 = kp.PublicKey.Substring(inKey1.Length, (kp.PublicKey.Length - inKey1.Length));

			string outKey1 = kp.PrivateKey.Substring(0, (kp.PrivateKey.Length / 2));
			string outKey2 = kp.PrivateKey.Substring(inKey1.Length, (kp.PrivateKey.Length - outKey1.Length));

			Assert.AreEqual(kp.PublicKey.Length, inKey1.Length + inKey2.Length);
			Assert.AreEqual(kp.PrivateKey.Length, outKey1.Length + outKey2.Length);
		}
	}
}
