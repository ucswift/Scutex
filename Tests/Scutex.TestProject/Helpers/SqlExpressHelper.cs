using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.Win32;

namespace WaveTech.Scutex.UnitTests.Helpers
{
	internal class ConnectionOptions
	{
		// Fields
		internal int closeTimeout = 0x3e8;
		internal int commandTimeout = 20;
		internal int connectionTimeout = 20;
		internal string password;
		internal bool promptForPassword;
		internal string runningAs;
		internal string serverName;
		internal bool useMainInstance = false;
		internal string userName;
	}

	internal class DatabaseInfo
	{
		// Fields
		private string name;
		private string path;

		// Methods
		internal DatabaseInfo(string name, string path)
		{
			this.name = name;
			this.path = path;
		}

		// Properties
		internal string Name
		{
			get
			{
				return this.name;
			}
		}

		internal string Path
		{
			get
			{
				return this.path;
			}
		}
	}

	internal class Settings
	{
		// Fields
		internal static string ConsoleCommandPrefix = "!";
		internal static string DatabaseNamePrefix = "name=";
		internal static string ExpansionVariablePrefix = "_$$(";
		internal static string ExpansionVariableSuffix = ")";
	}





	internal class SqlConnectionWrapper : IDbConnection, IDisposable
	{
		// Fields
		private SqlConnection connection;

		// Methods
		internal SqlConnectionWrapper(SqlConnection connection)
		{
			this.connection = connection;
		}

		private void OpenConnectionInternal(bool isNested)
		{
			try
			{
				this.connection.Open();
			}
			catch (Exception exception)
			{
				if ((isNested || (this.connection == null)) || (!(exception is SqlException)))
				{
					throw;
				}
				this.OpenConnectionInternal(true);
			}
		}

		IDbTransaction IDbConnection.BeginTransaction()
		{
			return this.connection.BeginTransaction();
		}

		IDbTransaction IDbConnection.BeginTransaction(IsolationLevel il)
		{
			return this.connection.BeginTransaction(il);
		}

		void IDbConnection.ChangeDatabase(string databaseName)
		{
			this.connection.ChangeDatabase(databaseName);
		}

		void IDbConnection.Close()
		{
			this.connection.Close();
		}

		IDbCommand IDbConnection.CreateCommand()
		{
			return this.connection.CreateCommand();
		}

		void IDbConnection.Open()
		{
			this.OpenConnectionInternal(false);
		}

		void IDisposable.Dispose()
		{
			this.connection.Dispose();
		}

		// Properties
		internal SqlConnection SqlConnection
		{
			get
			{
				return this.connection;
			}
		}

		string IDbConnection.ConnectionString
		{
			get
			{
				return this.connection.ConnectionString;
			}
			set
			{
				this.connection.ConnectionString = value;
			}
		}

		int IDbConnection.ConnectionTimeout
		{
			get
			{
				return this.connection.ConnectionTimeout;
			}
		}

		string IDbConnection.Database
		{
			get
			{
				return this.connection.Database;
			}
		}

		ConnectionState IDbConnection.State
		{
			get
			{
				return this.connection.State;
			}
		}
	}

	internal class PathUtil
	{
		// Methods
		internal static string EnsureFullPath(string filePath)
		{
			return Path.GetFullPath(filePath);
		}

		internal static bool IsContained(string root, string path)
		{
			if (!Path.IsPathRooted(root) || !Path.IsPathRooted(path))
			{
				throw new ArgumentException("Path must be rooted.");
			}
			path = Path.GetDirectoryName(path);
			if (!path.EndsWith(Path.DirectorySeparatorChar.ToString(), true, CultureInfo.InvariantCulture))
			{
				path = path + Path.DirectorySeparatorChar.ToString();
			}
			root = Path.GetFullPath(root);
			path = Path.GetFullPath(path);
			return path.StartsWith(root, true, CultureInfo.InvariantCulture);
		}
	}

	internal class SqlServerEnumerator
	{
		// Methods
		internal static string[] GetLocalInstances()
		{
			RegistryKey key = null;
			string[] strArray;
			try
			{
				key = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Microsoft SQL Server", false);
			}
			catch
			{
			}
			if (key == null)
			{
				throw new Exception(@"Could not open key registry key 'Software\Microsoft\Microsoft SQL Server'");
			}
			try
			{
				strArray = (string[])key.GetValue("InstalledInstances");
			}
			catch
			{
				throw new Exception("Failed to get the values under InstalledInstances.");
			}
			if ((strArray != null) && (strArray.Length > 0))
			{
				for (int i = 0; i < strArray.Length; i++)
				{
					if (string.Compare(strArray[i], "MSSQLSERVER", StringComparison.OrdinalIgnoreCase) == 0)
					{
						strArray[i] = Environment.MachineName;
					}
					else
					{
						strArray[i] = Environment.MachineName + @"\" + strArray[i];
					}
				}
			}
			return strArray;
		}

		internal static string[] GetRemoteInstances()
		{
			DataTable dataSources = SqlDataSourceEnumerator.Instance.GetDataSources();
			if ((dataSources == null) || (dataSources.Rows.Count == 0))
			{
				return null;
			}
			if ((dataSources.Columns["ServerName"] == null) || (dataSources.Columns["InstanceName"] == null))
			{
				throw new Exception("The underlying SQL Server enumerator did not return the information.");
			}
			List<string> list = new List<string>();
			foreach (DataRow row in dataSources.Rows)
			{
				string item = row["ServerName"] as string;
				if (item != null)
				{
					string str2 = row["InstanceName"] as string;
					if (!string.IsNullOrEmpty(str2))
					{
						item = item + @"\" + str2;
					}
					list.Add(item);
				}
			}
			list.Sort();
			return list.ToArray();
		}
	}



	internal class DataUtil
	{
		// Fields
		internal const int CONNECTION_DEFAULT_TIMEOUT = 20;
		private const string createMdfSql = "DECLARE @databaseName sysname\nSET @databaseName = CONVERT(sysname, NEWID())\nWHILE EXISTS (SELECT name FROM sys.databases WHERE name = @databaseName)\nBEGIN\n\tSET @databaseName = CONVERT(sysname, NEWID())\nEND\nSET @databaseName = '[' + @databaseName + ']'\nDECLARE @sqlString nvarchar(MAX)\nSET @sqlString = 'CREATE DATABASE ' + @databaseName + N' ON ( NAME = [{0}], FILENAME = N''{1}'')'\nEXEC sp_executesql @sqlString\nSET @sqlString = 'ALTER DATABASE ' + @databaseName + ' SET AUTO_SHRINK ON'\nEXEC sp_executesql @sqlString\nSET @sqlString = 'ALTER DATABASE ' + @databaseName + ' SET OFFLINE WITH ROLLBACK IMMEDIATE'\nEXEC sp_executesql @sqlString\nSET @sqlString = 'EXEC sp_detach_db ' + @databaseName\nEXEC sp_executesql @sqlString";
		internal const string DEFAULT_SERVER_NAME = @".\SQLExpress";
		private const string unlockDatabaseSql = "USE master\nIF EXISTS (SELECT * FROM sysdatabases WHERE name = N'{0}')\nBEGIN\n\tALTER DATABASE [{1}] SET OFFLINE WITH ROLLBACK IMMEDIATE\n\tEXEC sp_detach_db [{1}]\nEND";

		// Methods
		internal static string BuildConnectionString(string serverName, bool useMainInstance, int timeout)
		{
			return BuildConnectionString(serverName, null, null, null, useMainInstance, timeout);
		}

		internal static string BuildConnectionString(string serverName, string filePath, bool useMainInstance, int timeout)
		{
			return BuildConnectionString(serverName, null, null, filePath, useMainInstance, timeout);
		}

		internal static string BuildConnectionString(string serverName, string userName, string password, bool useMainInstance, int timeout)
		{
			return BuildConnectionString(serverName, userName, password, null, useMainInstance, timeout);
		}

		internal static string BuildConnectionString(string serverName, string userName, string password, string filePath, bool useMainInstance, int timeout)
		{
			if ((serverName == null) || (serverName.Length == 0))
			{
				serverName = @".\SQLExpress";
			}
			string str = "Data Source=" + serverName + ";";
			if (userName == null)
			{
				str = str + "Integrated Security = SSPI;";
			}
			else
			{
				string str2 = str;
				str = str2 + "user=" + userName + ";password=" + password + ";";
			}
			if ((filePath != null) && (filePath.Length > 0))
			{
				if (!Path.IsPathRooted(filePath))
				{
					filePath = PathUtil.EnsureFullPath(filePath);
				}
				if (!File.Exists(filePath))
				{
					throw new Exception("File '" + Path.GetFileName(filePath) + "' could not be found.");
				}
				str = str + "AttachDbFileName=\"" + filePath + "\";";
			}
			if (!useMainInstance)
			{
				str = str + "User Instance=true;";
			}
			object obj2 = str;
			return string.Concat(new object[] { obj2, "Timeout=", timeout, ";" });
		}

		internal static List<DatabaseInfo> BuildDatabaseInfoList(ConnectionManager connectionManager, IDbConnection connection)
		{
			List<DatabaseInfo> list = new List<DatabaseInfo>();
			IDbCommand command = CreateCommand(connectionManager, connection);
			command.CommandText = "SELECT * FROM SYSDATABASES";
			try
			{
				IDataReader reader = command.ExecuteReader();
				if (reader == null)
				{
					return list;
				}
				try
				{
					string str;
					if (!((SqlDataReader)reader).HasRows || (reader.FieldCount <= 0))
					{
						return list;
					}
					int ordinal = reader.GetOrdinal("name");
					int i = reader.GetOrdinal("filename");
					if ((ordinal != -1) && (i != -1))
					{
						goto Label_0088;
					}
					throw new Exception("SYSDATABASE didn't adhere to the expected schema.");
				Label_0066:
					str = reader.GetString(ordinal);
					string path = reader.GetString(i);
					list.Add(new DatabaseInfo(str, path));
				Label_0088:
					if (reader.Read())
					{
						goto Label_0066;
					}
					return list;
				}
				finally
				{
					try
					{
						reader.Close();
					}
					catch
					{
					}
				}
			}
			catch
			{
				connection.Close();
			}
			return list;
		}

		internal static string BuildOleDbConnectionString(string serverName, string userName, string password)
		{
			if ((serverName == null) || (serverName.Length == 0))
			{
				serverName = @".\SQLExpress";
			}
			string str = "Provider=SQLOLEDB;Data Source=" + serverName + ";";
			if (userName == null)
			{
				return (str + "Integrated Security = SSPI;");
			}
			string str2 = str;
			return (str2 + "User ID=" + userName + ";Password=" + password + ";");
		}

		internal static IDbCommand CreateCommand(ConnectionManager connectionManager, IDbConnection connection)
		{
			IDbCommand command = connection.CreateCommand();
			command.CommandTimeout = connectionManager.ConnectionOptions.commandTimeout;
			return command;
		}

		internal static void CreateDatabaseFile(ConnectionManager connectionManager, string filePath)
		{
			IDbConnection connection = connectionManager.BuildMasterConnection();
			connection.Open();
			try
			{
				string str = Path.GetFileNameWithoutExtension(filePath).Replace("]", "]]").Replace("'", "''");
				filePath = Path.GetFullPath(filePath).Replace("'", "''''");
				try
				{
					IDbCommand command = CreateCommand(connectionManager, connection);
					command.CommandText = string.Format(CultureInfo.InvariantCulture, "DECLARE @databaseName sysname\nSET @databaseName = CONVERT(sysname, NEWID())\nWHILE EXISTS (SELECT name FROM sys.databases WHERE name = @databaseName)\nBEGIN\n\tSET @databaseName = CONVERT(sysname, NEWID())\nEND\nSET @databaseName = '[' + @databaseName + ']'\nDECLARE @sqlString nvarchar(MAX)\nSET @sqlString = 'CREATE DATABASE ' + @databaseName + N' ON ( NAME = [{0}], FILENAME = N''{1}'')'\nEXEC sp_executesql @sqlString\nSET @sqlString = 'ALTER DATABASE ' + @databaseName + ' SET AUTO_SHRINK ON'\nEXEC sp_executesql @sqlString\nSET @sqlString = 'ALTER DATABASE ' + @databaseName + ' SET OFFLINE WITH ROLLBACK IMMEDIATE'\nEXEC sp_executesql @sqlString\nSET @sqlString = 'EXEC sp_detach_db ' + @databaseName\nEXEC sp_executesql @sqlString", new object[] { str, filePath });
					command.ExecuteNonQuery();
				}
				catch (Exception exception)
				{
					throw new Exception("An error occurred while creating a new DB file: " + Environment.NewLine + exception.ToString());
				}
			}
			finally
			{
				connection.Close();
			}
		}

		internal static void DetachDatabase(ConnectionManager connectionManager, IDbConnection connection, string dbName, bool silent)
		{
			IDbCommand command = CreateCommand(connectionManager, connection);
			command.CommandType = CommandType.Text;
			string str = dbName.Replace("]", "]]").Replace("'", "''");
			command.CommandText = string.Format("USE master\nIF EXISTS (SELECT * FROM sysdatabases WHERE name = N'{0}')\nBEGIN\n\tALTER DATABASE [{1}] SET OFFLINE WITH ROLLBACK IMMEDIATE\n\tEXEC sp_detach_db [{1}]\nEND", dbName, str);
			try
			{
				command.ExecuteNonQuery();
				if (!silent)
				{
					Console.WriteLine("Detached '" + dbName + "' successfully.");
				}
			}
			catch (SqlException exception)
			{
				if (!exception.Message.StartsWith("Unable to open the physical file", StringComparison.OrdinalIgnoreCase))
				{
					if (!silent)
					{
						Console.WriteLine("Failed to detach '" + dbName + "'");
					}
					return;
				}
				if (!silent)
				{
					Console.WriteLine("Detached '" + dbName + "' successfully.");
				}
			}
			catch
			{
			}
		}

		internal static void DisplayException(Exception ex)
		{
			string message = null;
			SqlException exception = ex as SqlException;
			if (exception != null)
			{
				if (exception.Errors.Count == 0)
				{
					if (exception.Message != null)
					{
						message = message + exception.Message;
					}
				}
				else
				{
					int num = 1;
					foreach (SqlError error in exception.Errors)
					{
						if (error.Message != null)
						{
							object obj2 = message + error.Message + Environment.NewLine;
							message = string.Concat(new object[] { obj2, "[SqlException Number ", error.Number, ", Class ", error.Class, ", State ", error.State, ", Line ", error.LineNumber, "]" }) + Environment.NewLine;
							num++;
						}
					}
				}
			}
			else
			{
				message = ex.Message;
			}
			if (message == null)
			{
				message = "Exception occurred. No information provided.";
			}
			Console.WriteLine(message);
		}



		//internal static int FetchAndDisplayRows(IDataReader reader, SqlConsole.ProcessingOption option, out DataTable table, out object scalar)
		//{
		//  DataTable dataTable = new DataTable();
		//  for (int i = 0; i < reader.FieldCount; i++)
		//  {
		//    dataTable.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
		//  }
		//  dataTable.BeginLoadData();
		//  object[] values = new object[reader.FieldCount];
		//  while (reader.Read())
		//  {
		//    reader.GetValues(values);
		//    dataTable.LoadDataRow(values, true);
		//  }
		//  dataTable.EndLoadData();
		//  ITableFormatter formatter = TableFormatterBuilder.BuildTableFormatter(dataTable);
		//  Console.WriteLine(formatter.ReadHeader());
		//  Console.WriteLine("");
		//  Console.WriteLine(new string('-', formatter.TotalColumnWidth));
		//  int num2 = 0;
		//  string str = null;
		//  while ((str = formatter.ReadNextRow()) != null)
		//  {
		//    Console.WriteLine(str);
		//    Console.WriteLine("");
		//    num2++;
		//  }
		//  scalar = null;
		//  table = null;
		//  if (option == SqlConsole.ProcessingOption.Scalar)
		//  {
		//    if ((dataTable.Rows.Count > 0) && (dataTable.Columns.Count > 0))
		//    {
		//      scalar = dataTable.Rows[0][0];
		//    }
		//    return num2;
		//  }
		//  if (option == SqlConsole.ProcessingOption.Table)
		//  {
		//    table = dataTable;
		//  }
		//  return num2;
		//}


		internal static List<DatabaseFileInfo> GetDatabaseFiles(ConnectionManager connectionManager, IDbConnection connection)
		{
			IDbCommand command = CreateCommand(connectionManager, connection);
			command.CommandText = "sp_helpfile";
			List<DatabaseFileInfo> list = new List<DatabaseFileInfo>();
			SqlDataReader reader = (SqlDataReader)command.ExecuteReader();
			if (reader != null)
			{
				try
				{
					string str;
					int ordinal = reader.GetOrdinal("name");
					int num2 = reader.GetOrdinal("filename");
					int num3 = reader.GetOrdinal("size");
					if (reader.HasRows && (num2 >= 0))
					{
						goto Label_0094;
					}
					throw new Exception("Could not get file information from the server for the given database.");
				Label_0066:
					str = reader.GetString(ordinal);
					string filePath = reader.GetString(num2);
					string size = reader.GetString(num3);
					list.Add(new DatabaseFileInfo(str, filePath, size));
				Label_0094:
					if (reader.Read())
					{
						goto Label_0066;
					}
				}
				finally
				{
					reader.Close();
				}
			}
			return list;
		}

		internal static string GetLogFilePath(string dataFilePath)
		{
			return Path.Combine(Path.GetDirectoryName(dataFilePath), Path.GetFileNameWithoutExtension(dataFilePath) + "_log.ldf");
		}

		internal static string IsFileAttached(ConnectionManager connectionManager, IDbConnection connection, string filePath)
		{
			filePath = PathUtil.EnsureFullPath(filePath);
			List<DatabaseInfo> list = BuildDatabaseInfoList(connectionManager, connection);
			if ((list != null) && (list.Count > 0))
			{
				foreach (DatabaseInfo info in list)
				{
					string b = PathUtil.EnsureFullPath(info.Path);
					if (string.Equals(filePath, b, StringComparison.OrdinalIgnoreCase))
					{
						return info.Name;
					}
				}
			}
			return null;
		}

		internal static string QuoteDbObjectName(string dbObjectName)
		{
			if (string.IsNullOrEmpty(dbObjectName))
			{
				return dbObjectName;
			}
			StringBuilder builder = new StringBuilder();
			builder.Append('[');
			int startIndex = 0;
			int index = -1;
			do
			{
				index = dbObjectName.IndexOf(']');
				if (index != -1)
				{
					builder.Append(dbObjectName.Substring(startIndex, index - startIndex));
					builder.Append("]]");
					startIndex = index + 1;
				}
			}
			while ((startIndex < dbObjectName.Length) && (index != -1));
			if (startIndex < dbObjectName.Length)
			{
				builder.Append(dbObjectName.Substring(startIndex, dbObjectName.Length - startIndex));
			}
			builder.Append("]");
			return builder.ToString();
		}

		// Nested Types
		internal class DatabaseFileInfo
		{
			// Fields
			internal string FilePath;
			internal string Name;
			internal string Size;

			// Methods
			internal DatabaseFileInfo(string name, string filePath, string size)
			{
				this.Name = name;
				this.FilePath = filePath;
				this.Size = size;
			}
		}
	}

	internal class DatabaseIdentifier
	{
		// Fields
		private bool isDatabaseName;
		private string value;

		// Methods
		private DatabaseIdentifier()
		{
		}

		internal static DatabaseIdentifier Parse(string databaseString)
		{
			if (string.IsNullOrEmpty(databaseString))
			{
				throw new Exception("Database should not be null.");
			}
			DatabaseIdentifier identifier = new DatabaseIdentifier();
			if (databaseString.StartsWith(Settings.DatabaseNamePrefix))
			{
				identifier.value = databaseString.Substring(Settings.DatabaseNamePrefix.Length);
				identifier.isDatabaseName = true;
				return identifier;
			}
			identifier.value = databaseString;
			identifier.isDatabaseName = false;
			return identifier;
		}

		// Properties
		internal bool IsDatabaseName
		{
			get
			{
				return this.isDatabaseName;
			}
		}

		internal string Value
		{
			get
			{
				return this.value;
			}
		}
	}




	internal class ConnectionManager
	{
		// Fields
		private ConnectionOptions connectionOptions;
		private string originalServerName;

		// Methods
		internal IDbConnection BuildAttachConnection(string filePath)
		{
			return this.BuildAttachConnection(filePath, (string)null);
		}

		internal IDbConnection BuildAttachConnection(string filePath, int timeout)
		{
			return new SqlConnectionWrapper(new SqlConnection(DataUtil.BuildConnectionString(this.connectionOptions.serverName, this.connectionOptions.userName, this.connectionOptions.password, filePath, this.connectionOptions.useMainInstance, timeout)));
		}

		internal IDbConnection BuildAttachConnection(string filePath, string dbName)
		{
			string connectionString = DataUtil.BuildConnectionString(this.connectionOptions.serverName, this.connectionOptions.userName, this.connectionOptions.password, filePath, this.connectionOptions.useMainInstance, this.connectionOptions.connectionTimeout);
			if (!string.IsNullOrEmpty(dbName))
			{
				connectionString = "Database=" + dbName + ";" + connectionString;
			}
			return new SqlConnectionWrapper(new SqlConnection(connectionString));
		}

		internal IDbConnection BuildMasterConnection()
		{
			return this.BuildMasterConnection(this.connectionOptions.useMainInstance);
		}

		internal IDbConnection BuildMasterConnection(bool useMainInstance)
		{
			return new SqlConnectionWrapper(new SqlConnection(DataUtil.BuildConnectionString(this.connectionOptions.serverName, this.connectionOptions.userName, this.connectionOptions.password, useMainInstance, this.connectionOptions.connectionTimeout)));
		}

		internal bool Initialize(ConnectionOptions connectionOptions)
		{
			this.connectionOptions = connectionOptions;
			if (connectionOptions.serverName == null)
			{
				if (false) //ProgramInfo.InSQLMode
				{
					string[] localInstances = null;
					try
					{
						localInstances = SqlServerEnumerator.GetLocalInstances();
					}
					catch (Exception)
					{
					}
					if ((localInstances != null) && (localInstances.Length > 0))
					{
						this.connectionOptions.serverName = localInstances[0];
					}
					else
					{
						this.connectionOptions.serverName = ".";
					}
				}
				else
				{
					this.connectionOptions.serverName = @".\SQLEXPRESS";
				}
			}
			if (connectionOptions.promptForPassword)
			{
				//try
				//{
				//  this.connectionOptions.password = ConsoleUtil.PromptForPassword("Enter password for user '" + connectionOptions.userName + "': ");
				//  Console.WriteLine("");
				//}
				//catch (UserCanceledException)
				//{
				//  return false;
				//}
			}
			if (!string.IsNullOrEmpty(connectionOptions.runningAs) && !connectionOptions.useMainInstance)
			{
				IDbConnection connection = this.BuildMasterConnection(true);
				string str = "SELECT owning_principal_name, instance_pipe_name FROM sys.dm_os_child_instances";
				IDbCommand command = DataUtil.CreateCommand(this, connection);
				command.CommandText = str;
				connection.Open();
				try
				{
					SqlDataReader reader = (SqlDataReader)command.ExecuteReader();
					if (reader != null)
					{
						try
						{
							if (reader.HasRows)
							{
								int ordinal = reader.GetOrdinal("owning_principal_name");
								int num2 = reader.GetOrdinal("instance_pipe_name");
								while (reader.Read())
								{
									if (string.Equals(reader.GetString(ordinal), connectionOptions.runningAs, StringComparison.OrdinalIgnoreCase))
									{
										string str3 = reader.GetString(num2);
										if (this.originalServerName == null)
										{
											Console.WriteLine("Using instance '" + str3 + "'.");
											Console.WriteLine("");
											this.originalServerName = connectionOptions.serverName;
											this.connectionOptions.serverName = str3;
											this.connectionOptions.useMainInstance = true;
										}
										else
										{
											Console.WriteLine("Multiple child instances were found to run under the principal name you specified. The first one will be used.");
										}
									}
								}
							}
							if (this.originalServerName == null)
							{
								throw new Exception("No child instance is running under the principal name you specified.");
							}
						}
						finally
						{
							reader.Close();
						}
					}
				}
				finally
				{
					connection.Close();
				}
			}
			return true;
		}

		// Properties
		internal ConnectionOptions ConnectionOptions
		{
			get
			{
				return this.connectionOptions;
			}
		}
	}

	internal class Commands
	{

		internal static void DetachDatabase(string database)
		{
			ConnectionOptions connectionOptions = new ConnectionOptions();
			ConnectionManager connectionManager = new ConnectionManager();
			connectionManager.Initialize(connectionOptions);


			if (string.IsNullOrEmpty(database))
			{
				throw new Exception("Detach requires a file path or database name (within brackets).");
			}
			bool flag = false;
			DatabaseIdentifier identifier = DatabaseIdentifier.Parse(database);
			database = identifier.Value;
			if (identifier.IsDatabaseName)
			{
				if (string.IsNullOrEmpty(database))
				{
					throw new Exception("Invalid database name specified.");
				}
			}
			else
			{
				if (database.EndsWith("*"))
				{
					database = database.Substring(0, database.Length - 1);
					flag = true;
				}
				database = PathUtil.EnsureFullPath(database);
			}
			IDbConnection connection = connectionManager.BuildMasterConnection();
			connection.Open();
			try
			{
				List<DatabaseInfo> list = DataUtil.BuildDatabaseInfoList(connectionManager, connection);
				if (list != null)
				{
					bool flag2 = false;
					foreach (DatabaseInfo info in list)
					{
						bool flag3 = false;
						try
						{
							if (identifier.IsDatabaseName)
							{
								if (string.Equals(database, info.Name, StringComparison.OrdinalIgnoreCase))
								{
									flag3 = true;
								}
							}
							else
							{
								string b = PathUtil.EnsureFullPath(info.Path);
								if (flag)
								{
									if (b.StartsWith(database, StringComparison.OrdinalIgnoreCase))
									{
										flag3 = true;
									}
								}
								else if (string.Equals(database, b, StringComparison.OrdinalIgnoreCase))
								{
									flag3 = true;
								}
							}
							if (flag3 &&
									(((string.Compare(info.Name, "master", StringComparison.OrdinalIgnoreCase) == 0) ||
										(string.Compare(info.Name, "tempdb", StringComparison.OrdinalIgnoreCase) == 0)) ||
									 ((string.Compare(info.Name, "model", StringComparison.OrdinalIgnoreCase) == 0) ||
										(string.Compare(info.Name, "msdb", StringComparison.OrdinalIgnoreCase) == 0))))
							{
								Console.WriteLine("Warning: Not detaching system database '{0}'. Use SQL commands in the console to do it.",
																	info.Name);
								flag3 = false;
							}
						}
						catch
						{
						}
						if (flag3)
						{
							DataUtil.DetachDatabase(connectionManager, connection, info.Name, false);
							flag2 = true;
						}
					}
					if (!flag2)
					{
						if (identifier.IsDatabaseName)
						{
							Console.WriteLine("No valid database name matches the value specified.");
						}
						else
						{
							Console.WriteLine("No valid database path matches the value specified.");
						}
					}
				}
			}
			finally
			{
				connection.Close();
			}
		}
	}
}