using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace WaveTech.Scutex.Manager.Classes
{
	public static class DatabaseFileHelper
	{
		public static void ResetDatabaseReadOnlyFlag()
		{
			string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
			path = path.Replace("file:\\", "");

			DirectoryInfo dir = new DirectoryInfo(path + @"\db");

			List<FileInfo> databaseFiles = new List<FileInfo>();
			databaseFiles.AddRange(dir.GetFiles("*.*"));

			foreach (FileInfo fi in databaseFiles)
			{
				fi.IsReadOnly = false;
			}
		}
	}
}