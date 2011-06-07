using System.Text.RegularExpressions;

namespace WaveTech.Scutex.Framework
{
	internal static class StringHelpers
	{
		/// <summary>
		/// method for validating a url with regular expressions
		/// </summary>
		/// <param name="url">url we're validating</param>
		/// <returns>true if valid, otherwise false</returns>
		public static bool IsValidUrl(string url)
		{
			// If it's a localhost url it won't match the regex below.
			if ((url.StartsWith("http://localhost") || url.StartsWith("https://localhost")) && url.EndsWith("/"))
				return true;

			string pattern = @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
			Regex reg = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
			return reg.IsMatch(url);
		}
	}
}