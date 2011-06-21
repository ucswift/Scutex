using Microsoft.VisualStudio.TestTools.UnitTesting;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Providers.AsymmetricEncryptionProvider;

namespace WaveTech.Scutex.UnitTests.Providers
{
	[TestClass]
	public class AsymmetricEncryptionProviderTests
	{
		private IAsymmetricEncryptionProvider asymmetricEncryptionProvider;
		private KeyPair _key;
		private string plainText = "This is some test string that will be used to encrupt using RSA!!!";

		[TestInitialize]
		public void WireUp()
		{
			asymmetricEncryptionProvider = new AsymmetricEncryptionProvider();
			_key = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);
		}

		[TestMethod]
		public void PrivateEncryptionPublicDecryptTest()
		{
			string data = asymmetricEncryptionProvider.EncryptPrivate(plainText, _key);
			string data2 = asymmetricEncryptionProvider.DecryptPublic(data, _key);

			Assert.AreEqual(plainText, data2);
		}

		[TestMethod]
		public void PublicEncryptionPrivateDecryptTest()
		{
			string data = asymmetricEncryptionProvider.EncryptPublic(plainText, _key);
			string data2 = asymmetricEncryptionProvider.DecryptPrivate(data, _key);

			Assert.AreEqual(plainText, data2);
		}

		[TestMethod]
		public void PrivateEncryptionPrivateDecryptTest()
		{
			string data = asymmetricEncryptionProvider.EncryptPrivate(plainText, _key);
			string data2 = asymmetricEncryptionProvider.DecryptPrivate(data, _key);

			Assert.AreNotEqual(plainText, data2);
		}

		[TestMethod]
		public void PublicEncryptionPublicDecryptTest()
		{
			string data = asymmetricEncryptionProvider.EncryptPublic(plainText, _key);
			string data2 = asymmetricEncryptionProvider.DecryptPublic(data, _key);

			Assert.AreNotEqual(plainText, data2);
		}
	}
}