using System;
using StructureMap;

namespace WaveTech.Scutex.Framework
{
	internal static class Bootstrapper
	{
		public static void Configure()
		{
			ObjectFactory.Configure(x => x.Scan(
				scan =>
				{
					scan.AssembliesFromPath(Environment.CurrentDirectory, assembly => assembly.GetName().Name.Contains("WaveTech.Scutex") || assembly.GetName().Name.Contains("LicensingManager"));
					scan.WithDefaultConventions();
					scan.LookForRegistries();
				}
			));
		}
	}
}