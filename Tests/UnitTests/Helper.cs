using System;
using System.IO;
using System.Reflection;

namespace WaveTech.Scutex.UnitTests
{
	public class Helper
	{
		/// <summary>
		///		Runs a method on a type, given its parameters. This is useful for
		///		calling private methods.
		/// </summary>
		/// <param name="t"></param>
		/// <param name="strMethod"></param>
		/// <param name="aobjParams"></param>
		/// <returns>The return value of the called method.</returns>
		public static object RunStaticMethod(Type t, string strMethod, object[] aobjParams)
		{
			BindingFlags eFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			return RunMethod(t, strMethod, null, aobjParams, eFlags);
		} //end of method

		public static object RunInstanceMethod(Type t, string strMethod, object objInstance, object[] aobjParams)
		{
			BindingFlags eFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
			return RunMethod(t, strMethod, objInstance, aobjParams, eFlags);
		} //end of method

		private static object RunMethod(Type t, string strMethod, object objInstance, object[] aobjParams, BindingFlags eFlags)
		{
			MethodInfo m;
			try
			{
				m = t.GetMethod(strMethod, eFlags);
				if (m == null)
				{
					throw new ArgumentException("There is no method '" + strMethod + "' for type '" + t + "'.");
				}

				object objRet = m.Invoke(objInstance, aobjParams);
				return objRet;
			}
			catch
			{
				throw;
			}
		} //end of method

		public static string AssemblyDirectory
		{
			get
			{
				string codeBase = Assembly.GetExecutingAssembly().CodeBase;
				UriBuilder uri = new UriBuilder(codeBase);
				string path = Uri.UnescapeDataString(uri.Path);
				return Path.GetDirectoryName(path);
			}
		}
	}
}