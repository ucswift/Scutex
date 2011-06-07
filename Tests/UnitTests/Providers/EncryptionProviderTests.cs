using NUnit.Framework;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Providers.SymmetricEncryptionProvider;

namespace WaveTech.Scutex.UnitTests.Providers
{
	[TestFixture]
	public class EncryptionProviderTests
	{
		string plainText = "Hello, World!";    // original plaintext
		private string cipherText = "Pr4prQGpaQ/XADxgEaVSfA==";

		[Test]
		public void EncryptStringTest()
		{
			EncryptionInfo info = new EncryptionInfo();
			info.PassPhrase = "Pas5pr@se";        // can be any string
			info.SaltValue = "s@1tValue";        // can be any string
			info.HashAlgorithm = "SHA1";             // can be "MD5"
			info.Iterations = 2;                  // can be any number
			info.InitVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
			info.KeySize = 256;                // can be 192 or 128



			SymmetricEncryptionProvider provider = new SymmetricEncryptionProvider();
			string cipherText2 = provider.Encrypt(plainText, info);

			Assert.IsNotNull(cipherText2);
			Assert.AreEqual(cipherText2, cipherText);
		}

		[Test]
		public void DecryptStringTest()
		{
			EncryptionInfo info = new EncryptionInfo();
			info.PassPhrase = "Pas5pr@se";        // can be any string
			info.SaltValue = "s@1tValue";        // can be any string
			info.HashAlgorithm = "SHA1";             // can be "MD5"
			info.Iterations = 2;                  // can be any number
			info.InitVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
			info.KeySize = 256;                // can be 192 or 128

			SymmetricEncryptionProvider provider = new SymmetricEncryptionProvider();

			string plainText2 = provider.Decrypt(cipherText, info);

			Assert.IsNotNull(plainText2);
			Assert.AreEqual(plainText2, plainText);
		}
	}
}