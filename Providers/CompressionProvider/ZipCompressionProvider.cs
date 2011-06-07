using ICSharpCode.SharpZipLib.Zip;
using WaveTech.Scutex.Model.Interfaces.Providers;

namespace WaveTech.Scutex.Providers.CompressionProvider
{
	internal class ZipCompressionProvider : IZipCompressionProvider
	{
		public void CreateZip(string fileName, string path)
		{
			FastZip zip = new FastZip();

			zip.CreateZip(fileName, path, true, null, null);
		}
	}
}