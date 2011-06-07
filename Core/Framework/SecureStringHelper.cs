using System;
using System.Runtime.InteropServices;
using System.Security;

namespace WaveTech.Scutex.Framework
{
	internal static class SecureStringHelper
	{
		public static SecureString StringToSecureString(string input)
		{
			SecureString output = new SecureString();
			int l = input.Length;
			char[] s = input.ToCharArray(0, l);
			foreach (char c in s)
			{
				output.AppendChar(c);
			}
			return output;
		}

		public static String SecureStringToString(SecureString input)
		{
			string output = "";
			IntPtr ptr = SecureStringToBSTR(input);
			output = PtrToStringBSTR(ptr);
			return output;
		}

		private static IntPtr SecureStringToBSTR(SecureString ss)
		{
			IntPtr ptr = new IntPtr();
			ptr = Marshal.SecureStringToBSTR(ss);
			return ptr;
		}

		private static string PtrToStringBSTR(IntPtr ptr)
		{
			string s = Marshal.PtrToStringBSTR(ptr);
			Marshal.ZeroFreeBSTR(ptr);
			return s;
		}

	}
}