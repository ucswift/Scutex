using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Generators.StaticKeyGeneratorSmall;
using WaveTech.Scutex.Licensing;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Generators;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Repositories;
using WaveTech.Scutex.Model.Interfaces.Services;
using WaveTech.Scutex.Providers.AsymmetricEncryptionProvider;
using WaveTech.Scutex.Providers.DataGenerationProvider;
using WaveTech.Scutex.Providers.HashingProvider;
using WaveTech.Scutex.Providers.ObjectSerialization;
using WaveTech.Scutex.Providers.SymmetricEncryptionProvider;
using WaveTech.Scutex.Providers.WebServicesProvider;
using WaveTech.Scutex.Repositories.ClientDataRepository;
using WaveTech.Scutex.Services;
using WaveTech.Scutex.TestHarness.WcfServices.ActivationService;

namespace TestHarness
{
	class Program
	{
		static void Main(string[] args)
		{
			//WmiDataProvider provider = new WmiDataProvider();

			//Console.WriteLine(provider.GetCpuManufacturer());
			//Console.WriteLine(provider.GetCpuSocket());
			//Console.WriteLine(provider.GetCpuId());
			//Console.WriteLine(provider.GetCpuVersion());

			//Console.WriteLine(provider.GetMotherboardManufacturer());
			//Console.WriteLine(provider.GetMotherboardSku());
			//Console.WriteLine(provider.GetMotherboardVersion());
			//Console.WriteLine(provider.GetMotherboardTag());

			//Console.WriteLine(provider.GetBiosName());
			//Console.WriteLine(provider.GetBiosVersion());

			//WriteAllProcessorWmi();
			//WriteAllMotherboardWmi();
			//WriteAllBusWmi();
			//WriteAllMemoryWmi();
			//WriteAllBiosWmi();
			//WriteAllDriveWmi();

			//Program p = new Program();
			//p.TestLicensingManager();

			//WriteLicensingDllHash();
			//WriteEncodedLicensingDllHash();

			//WriteTestMD5andSHA1Hashs();
			//WriteCompressedString();
			//RsaEncryptionTest();
			//RsaPrivateStripTest();
			//ChilkatRsaTest();
			//ByteConversionTests();

			//RandomNumberGeneratorTest();

			//LicenseKeyGenerationTest();
			//SmallLicenseKeyGenrationTest();
			//BatchSmallLicenseKeyGenrationTest();
			//BatchLargeLicenseKeyGenrationTest();

			//SmallLicenseKeyWithLessThen15CharsTest();

			//SerializeServiceData();
			//SerializeMasterServiceData();

			//HashTokenWithSalt();

			TestLicensingManagerWithOptions();

			//Console.WriteLine();
			//Console.WriteLine("Press enter to exit.");
			//Console.ReadLine();
		}

		public void TestLicensingManager()
		{
			LicensingManager manager = new LicensingManager(this);
			manager.Validate(InteractionModes.Gui);
		}

		public static void WriteAllProcessorWmi()
		{
			ManagementClass mgmt = new ManagementClass("Win32_Processor");
			ManagementObjectCollection objCol = mgmt.GetInstances();

			foreach (ManagementObject obj in objCol)
			{
				foreach (var p in obj.Properties)
				{
					Console.WriteLine(p.Name);
				}
			}
		}

		public static void WriteAllMotherboardWmi()
		{
			ManagementClass mgmt = new ManagementClass("Win32_BaseBoard");
			ManagementObjectCollection objCol = mgmt.GetInstances();

			foreach (ManagementObject obj in objCol)
			{
				foreach (var p in obj.Properties)
				{
					Console.WriteLine(p.Name);
					Console.WriteLine("\t:" + p.Value);
				}
			}
		}

		public static void WriteAllBusWmi()
		{
			ManagementClass mgmt = new ManagementClass("Win32_Bus");
			ManagementObjectCollection objCol = mgmt.GetInstances();

			foreach (ManagementObject obj in objCol)
			{
				foreach (var p in obj.Properties)
				{
					Console.WriteLine(p.Name);
				}
			}
		}

		public static void WriteAllMemoryWmi()
		{
			ManagementClass mgmt = new ManagementClass("Win32_MemoryDevice");
			ManagementObjectCollection objCol = mgmt.GetInstances();

			foreach (ManagementObject obj in objCol)
			{
				foreach (var p in obj.Properties)
				{
					Console.WriteLine(p.Name);
				}
			}
		}

		public static void WriteAllBiosWmi()
		{
			ManagementClass mgmt = new ManagementClass("Win32_BIOS");
			ManagementObjectCollection objCol = mgmt.GetInstances();

			foreach (ManagementObject obj in objCol)
			{
				foreach (var p in obj.Properties)
				{
					Console.WriteLine(p.Name);
					Console.WriteLine(p.Value);
				}
			}
		}

		public static void WriteAllDriveWmi()
		{
			ManagementClass mgmt = new ManagementClass("Win32_DiskDrive");
			ManagementObjectCollection objCol = mgmt.GetInstances();

			foreach (ManagementObject obj in objCol)
			{
				foreach (var p in obj.Properties)
				{
					Console.WriteLine(p.Name);
					Console.WriteLine(p.Value);
				}
			}
		}

		public static void WriteLicensingDllHash()
		{
			IHashingProvider hashingProvider = new HashingProvider();
			Console.WriteLine(hashingProvider.HashFile(Directory.GetCurrentDirectory() + "\\WaveTech.Scutex.Licensing.dll"));
		}

		public static void WriteEncodedLicensingDllHash()
		{
			IHashingProvider hashingProvider = new HashingProvider();
			IEncodingService encodingService = new EncodingService();

			string hash = hashingProvider.HashFile(Directory.GetCurrentDirectory() + "\\WaveTech.Scutex.Licensing.dll");
			Console.WriteLine(encodingService.Encode(hash));
		}

		public static void WriteTestMD5andSHA1Hashs()
		{
			string input = "Test string!!!";

			MD5CryptoServiceProvider x = new MD5CryptoServiceProvider();
			byte[] bs = System.Text.Encoding.UTF8.GetBytes(input);
			bs = x.ComputeHash(bs);
			System.Text.StringBuilder s = new System.Text.StringBuilder();
			foreach (byte b in bs)
			{
				s.Append(b.ToString("x2").ToLower());
			}
			string password = s.ToString();
			Console.WriteLine(string.Format("MD5: {0}", password));

			byte[] buffer = System.Text.Encoding.UTF8.GetBytes(input);
			SHA1CryptoServiceProvider cryptoTransformSHA1 =
			new SHA1CryptoServiceProvider();

			string hash = BitConverter.ToString(
					cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");

			Console.WriteLine(string.Format("SHA1: {0}", hash));
		}

		public static void WriteCompressedString()
		{
			string input = "E7BD6259DE81E8431828E2D4A33969FAB5083147";

			byte[] buffer = Encoding.UTF8.GetBytes(input);
			MemoryStream ms = new MemoryStream();
			using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
			{
				zip.Write(buffer, 0, buffer.Length);
			}

			ms.Position = 0;
			MemoryStream outStream = new MemoryStream();

			byte[] compressed = new byte[ms.Length];
			ms.Read(compressed, 0, compressed.Length);

			byte[] gzBuffer = new byte[compressed.Length + 4];
			System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
			System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
			Console.WriteLine(Convert.ToBase64String(gzBuffer));
		}

		public static void RsaEncryptionTest()
		{
			RSACryptoServiceProvider RSAProvider = new RSACryptoServiceProvider(1024);
			string privateKey = RSAProvider.ToXmlString(true);
			string publicKey = RSAProvider.ToXmlString(false);

			RSACryptoServiceProvider RSAPrivate = new RSACryptoServiceProvider(1024);
			RSAPrivate.FromXmlString(privateKey);

			RSACryptoServiceProvider RSAPublic = new RSACryptoServiceProvider(1024);
			RSAPublic.FromXmlString(publicKey);

			ASCIIEncoding encoding = new ASCIIEncoding();
			string data = "RsA EnCryPTion is cool!!!";
			Byte[] newdata = encoding.GetBytes(data);

			Byte[] encrypted = RSAPublic.Encrypt(newdata, false);
			Console.WriteLine("Encrypted test with RSA...");
			Console.WriteLine(Convert.ToBase64String(encrypted));
			Console.WriteLine();

			Byte[] decrypted = RSAPrivate.Decrypt(encrypted, false);
			Console.WriteLine("Decrypted text with RSA...");
			Console.WriteLine(encoding.GetString(decrypted));
			Console.WriteLine();
		}

		public static void RsaPrivateStripTest()
		{
			string publicKey =
				"<RSAKeyValue><Modulus>2GUXteh32SZ/zP9g9T0kWgavS4JhKc5AdLZGIFdJg0X4w5y6lzewHkT5VI20P1WcEcC/wfIl+JV5NPgxH5C/7qHnqsvFKNasSoN/l9MP1J0ONEnAunDvX6hVwtJUYpd/J2aGpkONvbaIqZtkG8wdVw7sxIZz3QTTlBexEYB4CkU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

			string privateKey = "<RSAKeyValue><P>7ceXglF1VqMxptJ1SHbE+BoKTJa1ynUQINi7BUi4byMYaXGuidKDU2GY+9cTEm+N/66nNykvnwHWBzFFTyGrQw==</P><Q>6PoBmxvupy/RwRf8mlbKV7RWN0osNuzsF5rgjb8E1CROA7kp/sxJK9QSH1sz369NRIGuUaHV3fCXluypot4n1w==</Q><DP>Wg78jXk4zgWlap/PmBCT7bw/Jl72n6XS4/3yZ7/xSvap6lYKW10GLHCMtuXw7UyfJbYK01OgG8NgQv0gWSZRbQ==</DP><DQ>uSC4l/WZLZbtGYAjBM2Emi69989j1P1tGdDDMT+h6aUzrPe9LDBO0JoDEJGbZdraDl7yEwIDfQnKm25R2g6oHw==</DQ><InverseQ>vY4utKwNI2be2W+k6EOf2eE6wTNQaHUX5F8r8LI0KVLCfsToRB3qzpo3LxjcxDz7Zc1N99nQmnBWJKMeQb7KDg==</InverseQ><D>zNGKyrwZTCkbiB4kWJshoDB5lP/4FYjKC25HG+9ifmUpW4UqO10TTuM8F6L0e4n9afRFzD31YL+h9NgCLv1kG64A/3VET+ZsadiaqEkrgMroYfsFr6eObRMGPN22HzqCdx3OngCUjy4tWQRfpjcbn3I6PtfLCw3qtu8Vq/yrnpU=</D></RSAKeyValue>";

			RSACryptoServiceProvider RSAPrivate = new RSACryptoServiceProvider(1024);
			RSAPrivate.FromXmlString(privateKey);

			RSACryptoServiceProvider RSAPublic = new RSACryptoServiceProvider(1024);
			RSAPublic.FromXmlString(publicKey);

			ASCIIEncoding encoding = new ASCIIEncoding();
			string data = "RsA EnCryPTion is cool!!!";
			Byte[] newdata = encoding.GetBytes(data);

			Byte[] encrypted = RSAPublic.Encrypt(newdata, false);
			Console.WriteLine("Encrypted test with RSA...");
			Console.WriteLine(Convert.ToBase64String(encrypted));
			Console.WriteLine();

			Byte[] decrypted = RSAPrivate.Decrypt(encrypted, false);
			Console.WriteLine("Decrypted text with RSA...");
			Console.WriteLine(encoding.GetString(decrypted));
			Console.WriteLine();
		}

		public static void ChilkatRsaTest()
		{
			//Chilkat.Rsa rsa = new Chilkat.Rsa();
			//rsa.UnlockComponent("RSA$TEAM$BEAN_17C734B35RoH");

			//string data = "RsA EnCryPTion is cool!!!";
			//rsa.GenerateKey(1024);

			////  Keys are exported in XML format:
			//string publicKey;
			//publicKey = rsa.ExportPublicKey();
			//Console.WriteLine(publicKey);

			//string privateKey;
			//privateKey = rsa.ExportPrivateKey();
			//Console.WriteLine(privateKey);

			//Console.WriteLine();
			//Console.WriteLine();

			//Chilkat.Rsa rsaEncryptor = new Chilkat.Rsa();

			//rsaEncryptor.EncodingMode = "hex";
			//rsaEncryptor.ImportPublicKey(privateKey);

			//string encryptedStr;
			//encryptedStr = rsaEncryptor.EncryptStringENC(data, true);
			//Console.WriteLine(encryptedStr);
			//Console.WriteLine();

			//Chilkat.Rsa rsaDecryptor = new Chilkat.Rsa();

			//rsaDecryptor.EncodingMode = "hex";
			//rsaDecryptor.ImportPrivateKey(publicKey);

			//string decryptedStr;
			//decryptedStr = rsaDecryptor.DecryptStringENC(encryptedStr, false);
			//Console.WriteLine(decryptedStr);
			//Console.WriteLine();

			//Chilkat.Rsa rsaDecryptor2 = new Chilkat.Rsa();

			//rsaDecryptor2.EncodingMode = "hex";
			//rsaDecryptor2.ImportPrivateKey(privateKey);

			//string decryptedStr2;
			//decryptedStr2 = rsaDecryptor2.DecryptStringENC(encryptedStr, true);
			//Console.WriteLine(decryptedStr2);

			//Chilkat.Rsa rsaDecryptor3 = new Chilkat.Rsa();

			//rsaDecryptor3.EncodingMode = "hex";
			//rsaDecryptor3.ImportPrivateKey(privateKey);

			//string decryptedStr3;
			//decryptedStr3 = rsaDecryptor3.DecryptStringENC(encryptedStr, false);
			//Console.WriteLine(decryptedStr3);
		}

		public static void ByteConversionTests()
		{
			string longVersionNumber = "10.152.369.497845";
			Console.WriteLine(string.Format("Long Version Number Bytes: {0}", BitConverter.ToString(Encoding.UTF8.GetBytes(longVersionNumber))));
		}

		public static void LicenseKeyGenerationTest()
		{
			IAsymmetricEncryptionProvider asymmetricEncryptionProvider = new AsymmetricEncryptionProvider();
			ISymmetricEncryptionProvider symmetricEncryptionProvider = new SymmetricEncryptionProvider();
			IHashingProvider hashingProvider = new HashingProvider();
			IObjectSerializationProvider objectSerializationProvider = new ObjectSerializationProvider();
			ILicenseActiviationProvider licenseActiviationProvider = new LicenseActiviationProvider(
				asymmetricEncryptionProvider, symmetricEncryptionProvider, objectSerializationProvider);
			INumberDataGeneratorProvider numberDataGeneratorProvider = new NumberDataGenerator();
			IPackingService packingService = new PackingService(numberDataGeneratorProvider);

			IClientLicenseRepository clientLicenseRepository = new ClientLicenseRepository(objectSerializationProvider,
																																										 symmetricEncryptionProvider);
			IClientLicenseService clientLicenseService = new ClientLicenseService(clientLicenseRepository);

			KeyGenerator staticKeyGenerator = new KeyGenerator(symmetricEncryptionProvider, asymmetricEncryptionProvider, hashingProvider);
			LicenseKeyService licenseKeyService = new LicenseKeyService(staticKeyGenerator, packingService, clientLicenseService);

			ClientLicense license = new ClientLicense();
			LicenseGenerationOptions generationOptions = new LicenseGenerationOptions();

			license.UniqueId = Guid.NewGuid();

			license.Product = new Product();
			license.Product.Name = "My Great Uber Cool Product, with new juice!";
			license.Product.ProductId = 1;

			string productHash = hashingProvider.Checksum32(license.GetLicenseProductIdentifier()).ToString("X");

			Dictionary<string, string> licenseKeys = new Dictionary<string, string>();

			DateTime start = DateTime.Now;
			for (int i = 0; i < 100000; i++)
			{
				string key = licenseKeyService.GenerateLicenseKey("TEST", license, generationOptions);
				licenseKeys.Add(key, key.GetHashCode().ToString());
				Console.WriteLine(key);
			}
			DateTime end = DateTime.Now;

			Console.WriteLine(start - end);
		}

		public static void RandomNumberGeneratorTest()
		{
			for (int i = 0; i < 100; i++)
			{
				byte[] randomNumber = new byte[1];
				RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();
				Gen.GetBytes(randomNumber);
				int rand = Convert.ToInt32(randomNumber[0]);

				Console.WriteLine(string.Format("Orignal random value {0}, mod {1}", rand, rand % 35));
			}
		}

		public static void SmallLicenseKeyGenrationTest()
		{
			IAsymmetricEncryptionProvider asymmetricEncryptionProvider = new AsymmetricEncryptionProvider();
			ISymmetricEncryptionProvider symmetricEncryptionProvider = new SymmetricEncryptionProvider();
			IHashingProvider hashingProvider = new HashingProvider();
			IObjectSerializationProvider objectSerializationProvider = new ObjectSerializationProvider();
			ILicenseActiviationProvider licenseActiviationProvider = new LicenseActiviationProvider(
				asymmetricEncryptionProvider, symmetricEncryptionProvider, objectSerializationProvider);
			INumberDataGeneratorProvider numberDataGeneratorProvider = new NumberDataGenerator();
			IPackingService packingService = new PackingService(numberDataGeneratorProvider);

			IClientLicenseRepository clientLicenseRepository = new ClientLicenseRepository(objectSerializationProvider,
																																										 symmetricEncryptionProvider);
			IClientLicenseService clientLicenseService = new ClientLicenseService(clientLicenseRepository);

			IKeyGenerator smallKeyGenerator = new WaveTech.Scutex.Generators.StaticKeyGeneratorSmall.KeyGenerator(symmetricEncryptionProvider, asymmetricEncryptionProvider, hashingProvider);
			LicenseKeyService licenseKeyService = new LicenseKeyService(smallKeyGenerator, packingService, clientLicenseService);

			ClientLicense license = new ClientLicense();
			LicenseGenerationOptions generationOptions = new LicenseGenerationOptions();

			license.UniqueId = Guid.NewGuid();
			license.Product = new Product();
			license.Product.Name = "My Great Uber Cool Product, with new juice!";
			license.Product.ProductId = 1;
			string productHash = hashingProvider.Checksum32(license.GetLicenseProductIdentifier()).ToString("X");

			Dictionary<string, string> licenseKeys = new Dictionary<string, string>();

			DateTime start = DateTime.Now;
			try
			{
				List<string> keys = licenseKeyService.GenerateLicenseKeys("TEST", license, generationOptions, 100000);

				foreach (string key in keys)
				{
					string hash = hashingProvider.ComputeHash(key, "SHA256");
					licenseKeys.Add(hash, key);
					Console.WriteLine(key);


					Console.WriteLine(hash + " " + hash.Equals(hashingProvider.ComputeHash(key, "SHA256")));
				}
			}
			catch (Exception ex)
			{ Console.WriteLine(ex.Message); }
			finally
			{

				Console.WriteLine();
				Console.WriteLine("=================================");

				DateTime end = DateTime.Now;
				Console.WriteLine(string.Format("Key Generation took {0}", end - start));

				Console.WriteLine(string.Format("Generated {0} unique license keys", licenseKeys.Count));

				Console.WriteLine();
				Console.WriteLine("Press enter to exit.");
				Console.ReadLine();
			}

		}

		public static void BatchSmallLicenseKeyGenrationTest()
		{
			IAsymmetricEncryptionProvider asymmetricEncryptionProvider = new AsymmetricEncryptionProvider();
			ISymmetricEncryptionProvider symmetricEncryptionProvider = new SymmetricEncryptionProvider();
			IHashingProvider hashingProvider = new HashingProvider();
			IObjectSerializationProvider objectSerializationProvider = new ObjectSerializationProvider();
			ILicenseActiviationProvider licenseActiviationProvider = new LicenseActiviationProvider(
				asymmetricEncryptionProvider, symmetricEncryptionProvider, objectSerializationProvider);
			INumberDataGeneratorProvider numberDataGeneratorProvider = new NumberDataGenerator();
			IPackingService packingService = new PackingService(numberDataGeneratorProvider);

			IClientLicenseRepository clientLicenseRepository = new ClientLicenseRepository(objectSerializationProvider,
																																										 symmetricEncryptionProvider);
			IClientLicenseService clientLicenseService = new ClientLicenseService(clientLicenseRepository);

			IKeyGenerator smallKeyGenerator = new WaveTech.Scutex.Generators.StaticKeyGeneratorSmall.KeyGenerator(symmetricEncryptionProvider, asymmetricEncryptionProvider, hashingProvider);
			LicenseKeyService licenseKeyService = new LicenseKeyService(smallKeyGenerator, packingService, clientLicenseService);

			ClientLicense license = new ClientLicense();
			LicenseGenerationOptions generationOptions = new LicenseGenerationOptions();

			license.UniqueId = Guid.NewGuid();
			license.Product = new Product();
			license.Product.Name = "My Great Uber Cool Product, with new juice!";
			license.Product.ProductId = 1;
			string productHash = hashingProvider.Checksum32(license.GetLicenseProductIdentifier()).ToString("X");

			DateTime start = DateTime.Now;

			List<string> licenseKeys = licenseKeyService.GenerateLicenseKeys("TEST", license, generationOptions, 100000);
			Dictionary<string, string> doubleCheck = new Dictionary<string, string>();


			DateTime end = DateTime.Now;

			foreach (string s in licenseKeys)
			{
				doubleCheck.Add(s, "");
				Console.WriteLine(s);
			}

			Console.WriteLine();
			Console.WriteLine("=================================");

			Console.WriteLine(string.Format("Key Generation took {0}", end - start));
			Console.WriteLine(string.Format("Generated {0} unique license keys", licenseKeys.Count));

			Console.WriteLine();
			Console.WriteLine("Press enter to exit.");
			Console.ReadLine();

		}

		public static void BatchLargeLicenseKeyGenrationTest()
		{
			//IAsymmetricEncryptionProvider asymmetricEncryptionProvider = new AsymmetricEncryptionProvider();
			//ISymmetricEncryptionProvider symmetricEncryptionProvider = new SymmetricEncryptionProvider();
			//IHashingProvider hashingProvider = new HashingProvider();

			//IKeyGenerator smallKeyGenerator = new WaveTech.Scutex.Generators.StaticKeyGeneratorLarge.KeyGenerator(symmetricEncryptionProvider, asymmetricEncryptionProvider, hashingProvider);
			//LicenseKeyService licenseKeyService = new LicenseKeyService(smallKeyGenerator);

			//ClientLicense license = new ClientLicense();
			//LicenseGenerationOptions generationOptions = new LicenseGenerationOptions();

			//license.UniqueId = Guid.NewGuid();
			//license.Product = new Product();
			//license.Product.Name = "My Great Uber Cool Product, with new juice!";
			//license.Product.ProductId = 1;
			//string productHash = hashingProvider.Checksum32(license.GetLicenseProductIdentifier()).ToString("X");

			//DateTime start = DateTime.Now;

			//List<string> licenseKeys = licenseKeyService.GenerateLicenseKeys("TEST", license, generationOptions, 1000000);

			//DateTime end = DateTime.Now;

			//foreach (string s in licenseKeys)
			//{
			//  Console.WriteLine(s);
			//}

			//Console.WriteLine();
			//Console.WriteLine("=================================");

			//Console.WriteLine(string.Format("Key Generation took {0}", end - start));
			//Console.WriteLine(string.Format("Generated {0} unique license keys", licenseKeys.Count));

			//Console.WriteLine();
			//Console.WriteLine("Press enter to exit.");
			//Console.ReadLine();

		}

		public static void SmallLicenseKeyWithLessThen15CharsTest()
		{
			IAsymmetricEncryptionProvider asymmetricEncryptionProvider;
			ISymmetricEncryptionProvider symmetricEncryptionProvider;
			IHashingProvider hashingProvider;

			WaveTech.Scutex.Generators.StaticKeyGeneratorSmall.KeyGenerator smallKeyGenerator;
			ClientLicense license;
			LicenseGenerationOptions generationOptions;

			//List<LicensePlaceholder> placeholders;
			//Dictionary<int, LicensePlaceholder> placeholdersInTemplate;

			for (int i = 0; i < 100000; i++)
			{
				asymmetricEncryptionProvider = new AsymmetricEncryptionProvider();
				symmetricEncryptionProvider = new SymmetricEncryptionProvider();
				hashingProvider = new HashingProvider();

				smallKeyGenerator = new WaveTech.Scutex.Generators.StaticKeyGeneratorSmall.KeyGenerator(symmetricEncryptionProvider, asymmetricEncryptionProvider, hashingProvider);

				license = new ClientLicense();
				generationOptions = new LicenseGenerationOptions();

				license.UniqueId = Guid.NewGuid();
				license.Product = new Product();
				license.Product.Name = "My Great Uber Cool Product, with new juice!";
				license.Product.ProductId = 1;

				license.LicenseSets = new NotifyList<LicenseSet>();
				license.LicenseSets.Add(new LicenseSet());

				license.LicenseSets.First().SupportedLicenseTypes = LicenseKeyTypeFlag.SingleUser;
				license.LicenseSets.First().SupportedLicenseTypes |= LicenseKeyTypeFlag.Enterprise;
				license.LicenseSets.First().SupportedLicenseTypes |= LicenseKeyTypeFlag.Unlimited;

				generationOptions.LicenseKeyType = LicenseKeyTypes.Enterprise;

				string key = smallKeyGenerator.GenerateLicenseKey("TEST", license, generationOptions);

				if (key.Length < 15)
				{
					string error = key;
					Console.WriteLine("ERROR: " + error);
				}

				Console.WriteLine(key);
			}

		}

		public static void TestService()
		{
			ActivationServiceClient client = new ActivationServiceClient();

		}

		public static void SerializeServiceData()
		{
			IServicesService servicesService = ObjectLocator.GetInstance<IServicesService>();
			IObjectSerializationProvider objectSerializationProvider = ObjectLocator.GetInstance<IObjectSerializationProvider>();

			Service service = servicesService.GetServiceById(2);
			string serializedService = objectSerializationProvider.Serialize(service);

			if (File.Exists(Directory.GetCurrentDirectory() + "\\Service.dat"))
				File.Delete(Directory.GetCurrentDirectory() + "\\Service.dat");

			using (TextWriter writer = new StreamWriter((Directory.GetCurrentDirectory() + "\\Service.dat")))
			{
				writer.WriteLine(serializedService);
			}
		}

		public static void SerializeMasterServiceData()
		{
			ICommonRepository commonRepository = ObjectLocator.GetInstance<ICommonRepository>();
			IObjectSerializationProvider objectSerializationProvider = ObjectLocator.GetInstance<IObjectSerializationProvider>();

			MasterServiceData service = commonRepository.GetMasterServiceData();
			string serializedService = objectSerializationProvider.Serialize(service);

			if (File.Exists(Directory.GetCurrentDirectory() + "\\MasterService.dat"))
				File.Delete(Directory.GetCurrentDirectory() + "\\MasterService.dat");

			using (TextWriter writer = new StreamWriter((Directory.GetCurrentDirectory() + "\\MasterService.dat")))
			{
				writer.WriteLine(serializedService);
			}
		}

		public static void HashTokenWithSalt()
		{
			HashingProvider provider = new HashingProvider();
			Console.WriteLine(provider.ComputeHashWithSalt("b$7SDt%43J*a!9", "SHA256", null));

			PackingService service = new PackingService(new NumberDataGenerator());
			Token t = new Token();
			t.Data = "MXLBEcLe6/i1CjdyomC7T0vTlACTXpdRmnxcDXDE8yDuCal0xA==";
			t.Timestamp = DateTime.Now;

			Console.WriteLine(service.PackToken(t));

			SymmetricEncryptionProvider encryption = new SymmetricEncryptionProvider();
			EncryptionInfo ei = new EncryptionInfo();
			ei.HashAlgorithm = "SHA1";
			ei.InitVector = "a01JQ3481Ahnqwe9";
			ei.Iterations = 2;
			ei.KeySize = 256;
			ei.PassPhrase = "Da*eW6_EzU4_swuk8*hU";
			ei.SaltValue = "VuW9uDrE";

			Console.WriteLine(encryption.Encrypt("861641072009MXLBEcLe6/i1CjdyomC7T0vTlACTXpdRmnxcDXDE8yDuCal0xA==41410860", ei));

			Console.WriteLine();
			Console.WriteLine("Press enter to exit.");
			Console.ReadLine();
		}

		public static void TestLicensingManagerWithOptions()
		{
			LicensingManagerOptions options = new LicensingManagerOptions
																											{
																												DataFileLocation = @"C:\Temp\Scutex\License.lic",
																												PublicKey = "31|32|36|32|30|34|33|37|31|35|34|35|34|36|36|34|37|32|37|39|39|36|31|35|33|32|36|34|39|36|37|30|38|34|36|30|38|31|32|34|31|36|34|31|30|35|33|39|31|31|38|32|34|30|32|36|37|32|37|37|35|32|31|31|31|33|33|32|33|34|34|31|30|36|36|31|38|39|39|34|31|35|32|38|33|38|30|31|39|33|39|32|37|38|38|30|39|38|30|31|30|34|38|38|30|35|31|38|32|38|35|37|31|31|31|34|32|35|33|33|37|32|30|32|37|30|32|37|35|35|31|32|39|36|37|39|35|34|35|30|37|39|34|39|30|30|35|35|35|35|37|36|39|35|39|34|34|36|39|39|38|36|36|31|30|31|31|30|31|38|32|39|32|37|35|36|37|34|31|39|36|37|32|36|35|32|35|34|37|31|38|31|38|33|38|34|34|39|39|35|34|30|34|36|34|30|36|39|35|30|34|35|35|37|31|39|37|31|37|32|34|37|30|32|33|37|37|33|39|32|36|33|7c|36|35|35|33|37",
																												DllHash = "43|44|2d|36|42|2d|31|45|2d|30|30|2d|42|43|2d|45|32|2d|41|33|2d|31|33|2d|31|33|2d|34|38|2d|39|35|2d|44|32|2d|46|44|2d|43|42|2d|33|45|2d|35|36|2d|38|34|2d|33|30|2d|42|32|2d|46|46",
																												KillOnError = false
																											};

			LicensingManager LicensingManager = new LicensingManager(null, options);
			LicensingManager.Validate(InteractionModes.Component);
		}

	}
}