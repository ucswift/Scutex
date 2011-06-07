using System;
using System.Security.Cryptography;
using WaveTech.Scutex.Model.Interfaces.Providers;

namespace WaveTech.Scutex.Providers.DataGenerationProvider
{
	internal class NumberDataGenerator : INumberDataGeneratorProvider
	{
		public int GenerateRandomNumber()
		{
			byte[] buffer = new byte[4];
			RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
			rng.GetBytes(buffer);

			return BitConverter.ToInt32(buffer, 0);
		}

		public int GenerateRandomNumber(int min, int max)
		{
			return new Random(GenerateRandomNumber()).Next(min, max);
		}
	}
}