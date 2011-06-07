namespace WaveTech.Scutex.Model.Interfaces.Providers
{
	public interface IHashingProvider
	{
		string HashFile(string filePath);
		int Checksum16(string data);
		uint Checksum32(string data);

		char GetChecksumChar(string data);
		bool ValidateChecksumChar(string data);

		string ComputeHashWithSalt(string plainText,
											 string hashAlgorithm,
											 byte[] saltBytes);

		bool VerifyHashWithSalt(string plainText,
										string hashAlgorithm,
										string hashValue);

		string ComputeHash(string plainText, string hashAlgorithm);
	}
}