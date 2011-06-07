using System.IO;
using System.Reflection;
using TriAxis.RunSharp;

namespace WaveTech.Scutex.Manager.Classes
{
	public class DemoHostHelper
	{
		private string _path;

		public DemoHostHelper()
		{
			_path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
			_path = _path.Replace("file:\\", "");
		}

		public void CleanPreviousHost()
		{
			if (File.Exists(_path + @"\DemoHost.exe"))
				File.Delete(_path + @"\DemoHost.exe");

			if (File.Exists(_path + @"\sxu.dll"))
				File.Delete(_path + @"\sxu.dll");

			try
			{
				if (File.Exists(_path + @"\WaveTech.Scutex.Licensing.dll"))
					File.Delete(_path + @"\WaveTech.Scutex.Licensing.dll");
			}
			catch { }

		}

		public void CreateAssembly(string p1, string p2)
		{
			AssemblyGen ag = new AssemblyGen(_path + @"\DemoHost.exe");

			Assembly asm = Assembly.LoadFrom(_path + @"\WaveTech.Scutex.Licensing.dll");
			ag.Attribute(asm.GetType("WaveTech.Scutex.Model.LicenseAttribute"), p1, p2);

			TypeGen DemoHost = ag.Public.Class("DemoHost");
			{
				CodeGen g = DemoHost.Public.Static.Method(typeof(void), "Main");
				{
					g.WriteLine("====================================================");
					g.WriteLine("|                      SCUTEX                      |");
					g.WriteLine("|         DEMO HOST FOR TRIAL DIALOG TESTING       |");
					g.WriteLine("====================================================");
					g.WriteLine("");
					g.WriteLine("");
					g.WriteLine("Your trial dialog or form should display in a few seconds...");

					Operand licensingManager = g.Local(Exp.New(asm.GetType("WaveTech.Scutex.Licensing.LicensingManager")));

					Operand value = g.Local(asm.GetType("WaveTech.Scutex.Model.InteractionModes"));

					Operand value2 = g.Local(typeof(System.Int32));
					g.Assign(value2, 1);

					g.Assign(value, value2.Cast(asm.GetType("WaveTech.Scutex.Model.InteractionModes")));

					Operand scutexLicensing = g.Local(asm.GetType("WaveTech.Scutex.Model.ScutexLicense"));

					g.Assign(scutexLicensing, licensingManager.Invoke("Validate", value));

					g.Return();
				}
			}

			ag.Save();
			asm = null;
		}

		public void ExecuteAssembly()
		{
			//AppDomain domain = AppDomain.CreateDomain("DemoHostAppDomain");
			//domain.ExecuteAssembly(_path + @"\DemoHost.exe", null, null);

			System.Diagnostics.Process.Start(string.Format("{0}\\DemoHost.exe", _path));
		}
	}
}