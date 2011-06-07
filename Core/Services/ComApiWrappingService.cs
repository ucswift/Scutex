
using System.Reflection;
using TriAxis.RunSharp;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Services
{
	internal class ComApiWrappingService : IComApiWrappingService
	{
		private readonly IStringDataGeneratorProvider _stringDataGeneratorProvider;

		public ComApiWrappingService(IStringDataGeneratorProvider stringDataGeneratorProvider)
		{
			_stringDataGeneratorProvider = stringDataGeneratorProvider;
		}

		public void CreateComWrapper(string filePath, string dllPath, string p1, string p2)
		{
			AssemblyGen ag = new AssemblyGen(filePath);

			Assembly asm = Assembly.LoadFrom(dllPath);
			ag.Attribute(asm.GetType("WaveTech.Scutex.Model.LicenseAttribute"), p1, p2);

			ag.Attribute(typeof(System.Runtime.InteropServices.ComVisibleAttribute), true);
			ag.Attribute(typeof(System.Reflection.AssemblyVersionAttribute), "1.0.0.0");
			ag.Attribute(typeof(System.Reflection.AssemblyFileVersionAttribute), "1.0.0.0");
			ag.Attribute(typeof(System.Runtime.InteropServices.GuidAttribute), "DC7DE67E-EA7A-4D26-89FF-FECEF2937268");

			ag.Namespace("ScutexLicensingCCW");

			TypeGen ComWrapper = ag.Public.Class(_stringDataGeneratorProvider.GenerateRandomString(10, 50, false, false)).Attribute(typeof(System.Runtime.InteropServices.ClassInterfaceAttribute), System.Runtime.InteropServices.ClassInterfaceType.AutoDual);
			{
				CodeGen g1 = ComWrapper.Public.Constructor();

				CodeGen g2 = ComWrapper.Public.Method(typeof(int), "Validate").Parameter(typeof(int), "interactionMode");
				{
					Operand licensingManager = g2.Local(Exp.New(asm.GetType("WaveTech.Scutex.Licensing.LicensingManager"), g2.This()));
					Operand scutexLicensing = g2.Local(asm.GetType("WaveTech.Scutex.Model.ScutexLicense"));

					Operand value = g2.Local(asm.GetType("WaveTech.Scutex.Model.InteractionModes"));
					g2.Assign(value, g2.Arg("interactionMode").Cast(asm.GetType("WaveTech.Scutex.Model.InteractionModes")));

					g2.Assign(scutexLicensing, licensingManager.Invoke("Validate", value));

					g2.Return(0);
				}

				CodeGen g3 = ComWrapper.Public.Method(typeof(int), "Register").Parameter(typeof(string), "licenseKey");
				{
					Operand licensingManager = g3.Local(Exp.New(asm.GetType("WaveTech.Scutex.Licensing.LicensingManager"), g3.This()));
					Operand scutexLicensing = g3.Local(asm.GetType("WaveTech.Scutex.Model.ScutexLicense"));

					g3.Assign(scutexLicensing, licensingManager.Invoke("Register", g3.Arg("licenseKey")));

					g3.Return(0);
				}
			}

			ag.Save();
			asm = null;
		}
	}
}