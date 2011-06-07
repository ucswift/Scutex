using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;

namespace WaveTech.Scutex.Providers.AsymmetricEncryptionProvider
{
	internal class AsymmetricEncryptionProvider : IAsymmetricEncryptionProvider
	{
		public KeyPair GenerateKeyPair(BitStrengths bitStrength)
		{
			KeyPair kp = new KeyPair();

			RsaKeyPairGenerator r = new RsaKeyPairGenerator();
			r.Init(new KeyGenerationParameters(new SecureRandom(), 768));
			AsymmetricCipherKeyPair keys = r.GenerateKeyPair();

			kp.PublicKey = RsaPublicKeyToString((RsaKeyParameters)keys.Public);
			kp.PrivateKey = RsaPrivateKeyToString((RsaPrivateCrtKeyParameters)keys.Private);

			return kp;
		}

		public string EncryptPrivate(string plainText, KeyPair xml)
		{
			return Encrypt(plainText, xml.PrivateKey, true);
		}

		public string DecryptPrivate(string cipherText, KeyPair xml)
		{
			return Decrypt(cipherText, xml.PrivateKey, true);
		}

		public string EncryptPublic(string plainText, KeyPair xml)
		{
			return Encrypt(plainText, xml.PublicKey, false);
		}

		public string DecryptPublic(string cipherText, KeyPair xml)
		{
			return Decrypt(cipherText, xml.PublicKey, false);
		}


		internal string Encrypt(string plainText, string encryptionKey, bool isPrivate)
		{
			RsaKeyParameters key;
			byte[] data = null;
			List<byte> output = new List<byte>();
			string result = null;

			try
			{
				if (isPrivate)
					key = RsaPrivateStringToRsaKey(encryptionKey);
				else
					key = RsaPublicStringToRsaKey(encryptionKey);

				data = StringToByteArray(plainText);

				IAsymmetricBlockCipher e = new Pkcs1Encoding(new RsaEngine()).GetUnderlyingCipher();
				e.Init(true, key);

				int blockSize = e.GetInputBlockSize();

				if (data != null)
				{
					for (int chunkPosition = 0; chunkPosition < data.Length; chunkPosition += blockSize)
					{
						int chunkSize;

						if (data.Length <= blockSize)
							chunkSize = data.Length; // If we have less data then the blockSize, do it all
						else if ((chunkPosition + blockSize) > data.Length) // Do we have any remainder data
							chunkSize = data.Length - chunkPosition;
						else
							chunkSize = blockSize; // Normal process, chunk to blockSize

						// No more blocks to process
						if (chunkSize <= 0)
							break;

						output.AddRange(e.ProcessBlock(data, chunkPosition, chunkSize));
					}

					result = Encoding.ASCII.GetString(Hex.Encode(output.ToArray()));
				}
			}
			catch (Exception)
			{

			}

			return result;
		}

		internal string Decrypt(string cryptText, string encryptionKey, bool isPrivate)
		{
			RsaKeyParameters key;
			byte[] data = null;
			List<byte> output = new List<byte>();
			string result = null;

			try
			{
				if (isPrivate)
					key = RsaPrivateStringToRsaKey(encryptionKey);
				else
					key = RsaPublicStringToRsaKey(encryptionKey);

				data = Hex.Decode(cryptText);

				IAsymmetricBlockCipher e = new Pkcs1Encoding(new RsaEngine()).GetUnderlyingCipher();
				e.Init(false, key);

				int blockSize = e.GetInputBlockSize();

				if (data != null)
				{
					for (int chunkPosition = 0; chunkPosition < data.Length; chunkPosition += blockSize)
					{
						int chunkSize;

						if (data.Length <= blockSize)
							chunkSize = data.Length;
						else if ((chunkPosition + blockSize) > data.Length)
							chunkSize = data.Length - chunkPosition;
						else
							chunkSize = blockSize;

						if (chunkSize <= 0)
							break;

						output.AddRange(e.ProcessBlock(data, chunkPosition, chunkSize));
					}

					result = ByteArrayToString(output.ToArray());
				}
			}
			catch (Exception ex)
			{
				Debug.Write(ex.ToString());
			}

			return result;
		}


		#region Private RSA Helper Methods
		internal static string RsaPublicKeyToString(RsaKeyParameters publicKey)
		{
			return publicKey.Modulus + "|" + publicKey.Exponent;
		}

		internal static RsaKeyParameters RsaPublicStringToRsaKey(string publicKey)
		{
			string[] keyParts = publicKey.Split(char.Parse("|"));
			return new RsaKeyParameters(false, new BigInteger(keyParts[0]), new BigInteger(keyParts[1]));
		}

		internal static string RsaPrivateKeyToString(RsaPrivateCrtKeyParameters privateKey)
		{
			return privateKey.Modulus + "|" + privateKey.PublicExponent + "|" + privateKey.Exponent + "|" +
						 privateKey.P + "|" + privateKey.Q + "|" + privateKey.DP + "|" + privateKey.DQ + "|" + privateKey.QInv;
		}

		internal static RsaPrivateCrtKeyParameters RsaPrivateStringToRsaKey(string privateKey)
		{
			string[] keyParts = privateKey.Split(char.Parse("|"));



			return new RsaPrivateCrtKeyParameters(new BigInteger(keyParts[0]),
																						new BigInteger(keyParts[1]),
																						new BigInteger(keyParts[2]),
																						new BigInteger(keyParts[3]),
																						new BigInteger(keyParts[4]),
																						new BigInteger(keyParts[5]),
																						new BigInteger(keyParts[6]),
																						new BigInteger(keyParts[7]));
		}

		internal static byte[] StringToByteArray(string data)
		{
			BinaryFormatter bf = new BinaryFormatter();
			byte[] bytes;
			MemoryStream ms = new MemoryStream();

			bf.Serialize(ms, data);
			//ms.Seek(0, 0);
			ms.Position = 0;
			bytes = ms.ToArray();

			for (int i = 0; i < bytes.Length; ++i) bytes[i] ^= 168; // pseudo encrypt
			//for (int i = 0; i < bytes.Length; ++i) bytes[i] ^= 168; // pseudo decrypt

			//BinaryFormatter bfx = new BinaryFormatter();
			//MemoryStream msx = new MemoryStream();
			//msx.Write(bytes, 0, bytes.Length);
			//msx.Seek(0, 0);
			//string sx = (string)bfx.Deserialize(msx);

			//BinaryFormatter bfy = new BinaryFormatter();
			//MemoryStream msy = new MemoryStream();
			//bfy.Serialize(msy, sx);
			//msy.Seek(0, 0);
			//byte[] bytesy = msy.ToArray();

			return bytes;
		}

		internal static string ByteArrayToString(byte[] data)
		{
			byte[] bytes = data;

			for (int i = 0; i < bytes.Length; ++i) bytes[i] ^= 168; // pseudo decrypt

			BinaryFormatter bfx = new BinaryFormatter();
			MemoryStream msx = new MemoryStream();
			msx.Write(bytes, 0, bytes.Length);
			//msx.Seek(0, 0);
			msx.Position = 0;
			string sx = (string)bfx.Deserialize(msx);

			return sx;
		}
		#endregion Private RSA Helper Methods
	}
}