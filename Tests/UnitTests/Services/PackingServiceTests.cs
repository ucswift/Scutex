using System;
using NUnit.Framework;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Providers.DataGenerationProvider;
using WaveTech.Scutex.Services;

namespace WaveTech.Scutex.UnitTests.Services
{
	[TestFixture]
	public class PackingServiceTests
	{
		string tokenData = "Ahj912j&*&91hLa97Pr4prQGpaQ/XADxgEaVSfA==";

		[Test]
		public void CanPackAndUnpackToken()
		{
			NumberDataGenerator numberDataGenerator = new NumberDataGenerator();
			PackingService packingService = new PackingService(numberDataGenerator);

			Token t = new Token();
			t.Data = tokenData;
			t.Timestamp = DateTime.Now;

			string packedToken = packingService.PackToken(t);
			Token unpackedToken = packingService.UnpackToken(packedToken);

			Assert.AreEqual(tokenData, unpackedToken.Data);
			Assert.AreEqual(t.Timestamp.Day, unpackedToken.Timestamp.Day);
			Assert.AreEqual(t.Timestamp.Month, unpackedToken.Timestamp.Month);
			Assert.AreEqual(t.Timestamp.Year, unpackedToken.Timestamp.Year);
			Assert.AreEqual(t.Timestamp.Hour, unpackedToken.Timestamp.Hour);
			Assert.AreEqual(t.Timestamp.Minute, unpackedToken.Timestamp.Minute);
		}
	}
}