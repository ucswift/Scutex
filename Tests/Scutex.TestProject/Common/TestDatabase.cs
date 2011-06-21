using System;
using System.Data.SqlClient;
using System.IO;
using WaveTech.Scutex.UnitTests.Helpers;

namespace WaveTech.Scutex.UnitTests.Common
{
	public class TestDatabase : IDisposable
	{
		private readonly string connectionString;
		private readonly string databaseFilename;
		private readonly string databaseName;

		public string ConnectionString { get { return connectionString; } }
		public string DatabaseName { get { return databaseName; } }
		public string DatabaseFilePath { get { return databaseFilePath; } }
		public string Schema { get; set; }

		private string databaseFilePath { get { return string.Format("{0}\\Data\\{1}", Helper.AssemblyDirectory, databaseFilename); } }

		public TestDatabase(string schema)
		{
			databaseName = "testdb" + Guid.NewGuid().ToString("N");
			databaseName = databaseName.Substring(0, databaseName.Length - 10);

			databaseFilename = databaseName + ".mdf";

			connectionString = string.Format(
					@"Server=.\SQLEXPRESS; Integrated Security=true;AttachDbFileName={0};User Instance=True;", databaseFilePath);

			Schema = schema;

			CreateDatabase();
		}

		public void Dispose()
		{
			DeleteDatabaseFiles();
		}

		// Create a new file-based SQLEXPRESS database
		// (Credit to Louis DeJardin - thanks! http://snurl.com/5nbrc)
		protected void CreateDatabase()
		{
			string databaseName = Path.GetFileNameWithoutExtension(databaseFilename);

			using (var connection = new SqlConnection(
					@"Data Source=.\sqlexpress;Initial Catalog=tempdb;Integrated Security=true;User Instance=True;"))
			{
				connection.Open();
				using (var command = connection.CreateCommand())
				{
					command.CommandText =
							"CREATE DATABASE " + databaseName +
							" ON PRIMARY (NAME=" + databaseName +
							", FILENAME='" + databaseFilePath + "')";
					command.ExecuteNonQuery();

					command.CommandText =
							"EXEC sp_detach_db '" + databaseName + "', 'true'";
					command.ExecuteNonQuery();
				}
			}

			// After we've created the database, initialize it with any
			// schema we've been given
			if (!string.IsNullOrEmpty(Schema))
				ExecuteQuery(Schema);
		}

		//protected void DetachDatabase()
		//{
		//  string databaseName = Path.GetFileNameWithoutExtension(databaseFilename);

		//  try
		//  {
		//    //ExecuteQuery("EXEC sp_detach_db '" + databaseName + "', 'true'");

		//    string command = string.Format("{0}\\SSEUtil.exe", Helper.AssemblyDirectory);
		//    string parmeters = string.Format(" -d {0}", databaseFilePath);

		//    System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(command);
		//    psi.RedirectStandardOutput = true;
		//    psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
		//    psi.UseShellExecute = false;
		//    psi.Arguments = parmeters;

		//    System.Diagnostics.Process sseUtil;
		//    sseUtil = System.Diagnostics.Process.Start(psi);
		//    StreamReader myOutput = sseUtil.StandardOutput;

		//    sseUtil.WaitForExit(2000);
		//    if (sseUtil.HasExited)
		//    {
		//      string output = myOutput.ReadToEnd();
		//      Console.WriteLine(output);
		//    }

		//  }
		//  catch { }

		//}

		protected void DeleteDatabaseFiles()
		{
			try
			{
				DetachDatabase();

				string databaseName = Path.GetFileNameWithoutExtension(databaseFilename);
				string databasePath = Path.GetDirectoryName(databaseFilePath);

				if (File.Exists(databaseFilePath))
					File.Delete(databaseFilePath);

				if (File.Exists(databasePath + "\\" + databaseName + "_log.LDF"))
					File.Delete(databasePath + "\\" + databaseName + "_log.LDF");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		protected void ExecuteQuery(string sql)
		{
			using (var connection = new SqlConnection(ConnectionString))
			{
				connection.Open();
				using (var command = connection.CreateCommand())
				{
					command.CommandText = sql;
					command.ExecuteNonQuery();
				}
			}
		}

		public void DetachDatabase()
		{
			string databaseName = Path.GetFileNameWithoutExtension(databaseFilename);
			string str = "[[" + databaseName + "]]";

			Commands.DetachDatabase(databaseFilePath);
		}

	}
}