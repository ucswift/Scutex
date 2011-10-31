using System.Windows.Controls;
using WaveTech.Scutex.Framework;
using WPF.Themes;

namespace WaveTech.Scutex.Manager.Classes
{
	internal static class WindowHelper
	{
		internal static void CheckAndApplyTheme(ContentControl control)
		{
			OperatingSystemVersion os = OperatingSystemVersion.Current;

			if (os.OSVersion == OSVersion.Win7)
			{
				ThemeManager.ApplyTheme(control, "TwilightBlue");
			}
		}
	}
}