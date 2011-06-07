using System;
using System.Runtime.InteropServices;

namespace WaveTech.Scutex.Manager.Classes
{
	// http://winsharp93.wordpress.com/2009/07/21/wpf-hide-the-window-buttons-minimize-restore-and-close-and-the-icon-of-a-window/

	internal static class Win32
	{
		internal const int GWL_STYLE = (-16);
		internal const UInt32 SWP_FRAMECHANGED = 0x0020;
		internal const UInt32 SWP_NOSIZE = 0x0001;
		internal const UInt32 SWP_NOMOVE = 0x0002;
		internal const int WS_SYSMENU = 0x00080000;

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);
		internal static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
		{
			if (IntPtr.Size == 8)
				return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);

			return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
		}
		[DllImport("user32.dll", EntryPoint = "SetWindowLong")]
		private static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);
		[DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
		private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
	}
}