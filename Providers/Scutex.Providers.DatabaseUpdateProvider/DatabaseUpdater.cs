using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using System.Transactions;
using DbUp;
using DbUp.Helpers;
using DbUp.ScriptProviders;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Model.Interfaces.Providers;

namespace WaveTech.Scutex.Providers.DatabaseUpdateProvider
{
	internal class DatabaseUpdater : IDatabaseUpdateProvider
	{
		public bool ApplyBaseDatabaseSchema(string connectionString)
		{
			ResourceHelper helper = new ResourceHelper();
			SqlConnection con = new SqlConnection(connectionString);
			SqlCommand command = con.CreateCommand();

			command.CommandText = helper.GetDatabaseSchema();

			try
			{
				con.Open();
				int recrods = command.ExecuteNonQuery();

				//if (recrods > 0)
				return true;
			}
			catch (Exception ex)
			{
				Logging.Write(ex.ToString());
			}
			finally
			{
				con.Close();
			}

			return false;
		}

		public bool ApplyBaseDatabaseData(string connectionString)
		{
			ResourceHelper helper = new ResourceHelper();
			SqlConnection con = new SqlConnection(connectionString);
			SqlCommand command = con.CreateCommand();

			command.CommandText = helper.GetDatabaseData();

			try
			{
				con.Open();
				int recrods = command.ExecuteNonQuery();

				//if (recrods > 0)
				return true;
			}
			catch (Exception ex)
			{
				Logging.Write(ex.ToString());
			}
			finally
			{
				con.Close();
			}

			return false;
		}

		public bool InitializeDatabase(string connectionString)
		{
			if (IsDatabaseBasePopulated(connectionString) == false)
			{
				using (TransactionScope scope = new TransactionScope())
				{

					//Logging.Write("Applying database schema");
					if (ApplyBaseDatabaseSchema(connectionString) == false)
						return false;

					//Logging.Write("Applying database data");
					if (ApplyBaseDatabaseData(connectionString) == false)
						return false;

					scope.Complete();
				}
			}

			string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
			path = path.Replace("file:\\", "");
			path = path + @"\db\Scripts";

			DirectoryInfo di = new DirectoryInfo(path);
			FileInfo[] sqlFiles = di.GetFiles("*.sql");

			var upgrader = new DatabaseUpgrader(connectionString, new FileEnumerationScriptProvider(sqlFiles));
			var result = upgrader.PerformUpgrade(new MemoryLog());

			return true;
		}

		public bool IsDatabaseBasePopulated(string connectionString)
		{
			SqlConnection con = new SqlConnection(connectionString);
			SqlCommand command = con.CreateCommand();

			command.CommandText = "SELECT * FROM DBVersions";

			try
			{
				con.Open();
				SqlDataReader record = command.ExecuteReader();

				int records = 0;

				while (record.Read())
				{
					records++;
				}

				if (records > 0)
					return true;
			}
			catch { }
			finally
			{
				con.Close();
			}

			return false;
		}

		public bool IsDatabaseVersionCorrect(string connectionString)
		{
			SqlConnection con = new SqlConnection(connectionString);
			SqlCommand command = con.CreateCommand();

			command.CommandText = "SELECT @@Version";
			List<string> data = new List<string>();
			bool validVersion = true;


			try
			{
				con.Open();
				SqlDataReader record = command.ExecuteReader();

				while (record.Read())
				{
					data.Add(record.GetString(0));
				}

				foreach (var s in data)
				{
					if (s.Contains(Properties.Resources.DatabaseVersion) == false)
						validVersion = false;
				}
			}
			catch (Exception ex)
			{
				Logging.Write(ex.ToString());

				validVersion = false;
			}
			finally
			{
				con.Close();
			}

			return validVersion;
		}
	}
}