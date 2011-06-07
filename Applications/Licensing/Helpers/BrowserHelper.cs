using Microsoft.Win32;

namespace WaveTech.Scutex.Licensing.Helpers
{
	internal static class BrowserHelper
	{
		/// <summary>
		/// Reads path of default browser from registry
		/// </summary>
		/// <returns></returns>
		internal static string GetDefaultBrowserPath()
		{
			try
			{
				string key = @"htmlfile\shell\open\command";
				RegistryKey registryKey =
				Registry.ClassesRoot.OpenSubKey(key, false);

				// get default browser path
				return ((string)registryKey.GetValue(null, null)).Split('"')[1];
			}
			catch
			{
				return "iexplore";
			}
		}
	}
}