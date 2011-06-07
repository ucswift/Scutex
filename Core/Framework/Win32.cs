using System;
using System.Runtime.InteropServices;

namespace WaveTech.Scutex.Framework
{
	internal class Win32
	{
		[DllImport("kernel32.dll")]
		public static extern Boolean AllocConsole();
		[DllImport("kernel32.dll")]
		public static extern Boolean FreeConsole();
	}
}