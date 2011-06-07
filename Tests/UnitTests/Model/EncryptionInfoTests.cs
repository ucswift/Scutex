using NUnit.Framework;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Exceptions;

namespace WaveTech.Scutex.UnitTests.Model
{
	[TestFixture]
	public class EncryptionInfoTests
	{
		[Test]
		[ExpectedException(typeof(EncryptionInfoException), ExpectedMessage = "The EncryptionInfo key size is limited to values of 128, 192 or 256.")]
		public void SettingKeySizeToAnIncorrectValueShouldThrowAnError()
		{
			EncryptionInfo es = new EncryptionInfo();
			es.KeySize = 101;
		}

		[Test]
		public void SettingKeySizeTo128ValueShouldNotThrowAnError()
		{
			EncryptionInfo es = new EncryptionInfo();
			es.KeySize = 128;
		}

		[Test]
		public void SettingKeySizeTo192ValueShouldNotThrowAnError()
		{
			EncryptionInfo es = new EncryptionInfo();
			es.KeySize = 192;
		}

		[Test]
		public void SettingKeySizeTo256ValueShouldNotThrowAnError()
		{
			EncryptionInfo es = new EncryptionInfo();
			es.KeySize = 256;
		}

		[Test]
		[ExpectedException(typeof(EncryptionInfoException), ExpectedMessage = "The EncryptionInfo Hash Algorithm is limited to values of SHA1 or MD5.")]
		public void SettingHashAlgorithmToInvalidValueShouldThrowAnError()
		{
			EncryptionInfo es = new EncryptionInfo();
			es.HashAlgorithm = "AES";
		}

		[Test]
		public void SettingHashAlgorithmToSHA1ShouldNotThrowAnError()
		{
			EncryptionInfo es = new EncryptionInfo();
			es.HashAlgorithm = "SHA1";
		}

		[Test]
		public void SettingHashAlgorithmToMD5ShouldNotThrowAnError()
		{
			EncryptionInfo es = new EncryptionInfo();
			es.HashAlgorithm = "MD5";
		}

		[Test]
		[ExpectedException(typeof(EncryptionInfoException), ExpectedMessage = "The EncryptionInfo Init Vector must be 16 characters in length.")]
		public void SettingInitVectorToSmallValueShouldThrowAnError()
		{
			EncryptionInfo es = new EncryptionInfo();
			es.InitVector = "A4238&@@";
		}

		[Test]
		[ExpectedException(typeof(EncryptionInfoException), ExpectedMessage = "The EncryptionInfo Init Vector must be 16 characters in length.")]
		public void SettingInitVectorToLargeValueShouldThrowAnError()
		{
			EncryptionInfo es = new EncryptionInfo();
			es.InitVector = "A4238&@@AS!@@Dasd)_!jasdad1351D4!@#";
		}

		[Test]
		public void SettingInitVectorToValidValueShouldNotThrowAnError()
		{
			EncryptionInfo es = new EncryptionInfo();
			es.InitVector = "a1B2c3@4e5F6g7H^";
		}
	}
}
