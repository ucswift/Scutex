using System;
using WaveTech.Scutex.UnitTests.Common;

namespace WaveTech.Scutex.UnitTests.Helpers
{
	public class LicenseServiceTestManager
	{
		private TestDatabase _testDatabase;
		private TestWebServer _testClientWebServer;
		private TestWebServer _testMgmtWebServer;
		private LicenseHelper _licenseHelper;

		private int clientWebServerPort;
		private int mgmtWebServerPort;

		public LicenseServiceTestManager(LicenseHelper licenseHelper)
		{
			_licenseHelper = licenseHelper;
		}

		public void Start()
		{
			_testDatabase = new TestDatabase(System.IO.File.ReadAllText(Helper.AssemblyDirectory + "\\ScutexService-CreateDbNG.sql"));

			clientWebServerPort = TcpPortFinder.FindOpenTcpPortInRange(8100, 8200);
			_testClientWebServer = new TestWebServer(clientWebServerPort, @"/", Helper.AssemblyDirectory + @"\WebServices\Client\");
			ConfigFileWriter.CreateClientWebServiceConfig(_testDatabase.DatabaseName, _testDatabase.DatabaseFilePath);
			
			_testClientWebServer.Start();

			mgmtWebServerPort = TcpPortFinder.FindOpenTcpPortInRange(8200, 8300);
			_testMgmtWebServer = new TestWebServer(mgmtWebServerPort, @"/", Helper.AssemblyDirectory + @"\WebServices\Mgmt\");
			ConfigFileWriter.CreateMgmtWebServiceConfig(_testDatabase.DatabaseName, _testDatabase.DatabaseFilePath);

			_testMgmtWebServer.Start();

			_testDatabase.DetachDatabase();

			_licenseHelper.SetupService(clientWebServerPort, mgmtWebServerPort);
		}

		public void Stop()
		{
			_testClientWebServer.Stop();
			_testMgmtWebServer.Stop();

			_testDatabase.Dispose();
			_testClientWebServer.Dispose();
			_testMgmtWebServer.Dispose();
		}
	}
}