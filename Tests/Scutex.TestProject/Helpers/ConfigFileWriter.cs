using System.IO;

namespace WaveTech.Scutex.UnitTests.Helpers
{
	public static class ConfigFileWriter
	{
		public static void CreateClientWebServiceConfig(string databaseName, string fullPath)
		{
			if (File.Exists(Helper.AssemblyDirectory + "\\WebServices\\Client\\Web.config"))
				File.Delete(Helper.AssemblyDirectory + "\\WebServices\\Client\\Web.config");

			string connectionString = "<add name=\"ScutexServiceEntities\" connectionString=\"metadata=res://*/DB.csdl|res://*/DB.ssdl|res://*/DB.msl;provider=System.Data.SqlClient;provider connection string=&quot;Data Source=.\\SQLEXPRESS;Initial Catalog={0};Integrated Security=True;AttachDbFileName={1};User Instance=True;MultipleActiveResultSets=True&quot;\" providerName=\"System.Data.EntityClient\" />";
			string[] config = File.ReadAllLines(Helper.AssemblyDirectory + "\\WebServices\\Configs\\Client\\Web.config");

			config[3] = string.Format(connectionString, databaseName, fullPath);

			File.WriteAllLines(Helper.AssemblyDirectory + "\\WebServices\\Client\\Web.config", config);
		}

		public static void CreateMgmtWebServiceConfig(string databaseName, string fullPath)
		{
			if (File.Exists(Helper.AssemblyDirectory + "\\WebServices\\Mgmt\\Web.config"))
				File.Delete(Helper.AssemblyDirectory + "\\WebServices\\Mgmt\\Web.config");

			string connectionString = "<add name=\"ScutexServiceEntities\" connectionString=\"metadata=res://*/DB.csdl|res://*/DB.ssdl|res://*/DB.msl;provider=System.Data.SqlClient;provider connection string=&quot;Data Source=.\\SQLEXPRESS;Initial Catalog={0};Integrated Security=True;AttachDbFileName={1};User Instance=True;MultipleActiveResultSets=True&quot;\" providerName=\"System.Data.EntityClient\" />";
			string[] config = File.ReadAllLines(Helper.AssemblyDirectory + "\\WebServices\\Configs\\Mgmt\\Web.config");

			config[3] = string.Format(connectionString, databaseName, fullPath);

			File.WriteAllLines(Helper.AssemblyDirectory + "\\WebServices\\Mgmt\\Web.config", config);
		}
	}
}
