namespace WaveTech.Scutex.Model.Interfaces.Providers
{
	public interface IAsymmetricEncryptionProvider
	{
		KeyPair GenerateKeyPair(BitStrengths bitStrength);
		string EncryptPrivate(string plainText, KeyPair xml);
		string DecryptPrivate(string cipherText, KeyPair xml);
		string EncryptPublic(string plainText, KeyPair xml);
		string DecryptPublic(string cipherText, KeyPair xml);
	}
}