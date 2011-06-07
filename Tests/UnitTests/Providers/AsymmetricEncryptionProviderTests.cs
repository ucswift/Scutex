using NUnit.Framework;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Providers.AsymmetricEncryptionProvider;

namespace WaveTech.Scutex.UnitTests.Providers
{
	[TestFixture]
	public class AsymmetricEncryptionProviderTests
	{
		private IAsymmetricEncryptionProvider asymmetricEncryptionProvider;
		private KeyPair _key;
		private string plainText = "This is some test string that will be used to encrupt using RSA!!!";

		[SetUp]
		public void WireUp()
		{
			asymmetricEncryptionProvider = new AsymmetricEncryptionProvider();
			_key = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);
		}

		[Test]
		public void PrivateEncryptionPublicDecryptTest()
		{
			string data = asymmetricEncryptionProvider.EncryptPrivate(plainText, _key);
			string data2 = asymmetricEncryptionProvider.DecryptPublic(data, _key);

			Assert.AreEqual(plainText, data2);
		}

		[Test]
		public void PublicEncryptionPrivateDecryptTest()
		{
			string data = asymmetricEncryptionProvider.EncryptPublic(plainText, _key);
			string data2 = asymmetricEncryptionProvider.DecryptPrivate(data, _key);

			Assert.AreEqual(plainText, data2);
		}

		[Test]
		public void PrivateEncryptionPrivateDecryptTest()
		{
			string data = asymmetricEncryptionProvider.EncryptPrivate(plainText, _key);
			string data2 = asymmetricEncryptionProvider.DecryptPrivate(data, _key);

			Assert.AreNotEqual(plainText, data2);
		}

		[Test]
		public void PublicEncryptionPublicDecryptTest()
		{
			string data = asymmetricEncryptionProvider.EncryptPublic(plainText, _key);
			string data2 = asymmetricEncryptionProvider.DecryptPublic(data, _key);

			Assert.AreNotEqual(plainText, data2);
		}
	}
}