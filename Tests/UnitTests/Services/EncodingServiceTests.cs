using NUnit.Framework;
using WaveTech.Scutex.Model.Interfaces.Services;
using WaveTech.Scutex.Services;

namespace WaveTech.Scutex.UnitTests.Services
{
	[TestFixture]
	public class EncodingServiceTests
	{
		private string originalValue = "Ahj912j&*&91hLa97Pr4prQGpaQ/XADxgEaVSfA==";
		private string encodedValue = "41|68|6a|39|31|32|6a|26|2a|26|39|31|68|4c|61|39|37|50|72|34|70|72|51|47|70|61|51|2f|58|41|44|78|67|45|61|56|53|66|41|3d|3d";
		private string base64Original = "This is a test STRING for the base64 encoding test.";
		private string base64After = "VGhpcyBpcyBhIHRlc3QgU1RSSU5HIGZvciB0aGUgYmFzZTY0IGVuY29kaW5nIHRlc3Qu";

		[Test]
		public void EncodeTest()
		{
			IEncodingService service = new EncodingService();

			string test = service.Encode(originalValue);
			
			Assert.IsNotNull(test);
			Assert.AreEqual(encodedValue,test);
		}

		[Test]
		public void DecodeTest()
		{
			IEncodingService service = new EncodingService();

			string test = service.Decode(encodedValue);

			Assert.IsNotNull(test);
			Assert.AreEqual(originalValue, test);
		}

		[Test]
		public void Encode64Test()
		{
			EncodingService encodingService = new EncodingService();
			string test1 = encodingService.Encode64(base64Original);

			Assert.AreEqual(base64After, test1);
		}

		[Test]
		public void Decode64Test()
		{
			EncodingService encodingService = new EncodingService();
			string test1 = encodingService.Decode64(base64After);

			Assert.AreEqual(base64Original, test1);
		}
	}
}