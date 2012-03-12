using StructureMap;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Generators.StaticKeyGeneratorSmall;
using WaveTech.Scutex.Services;

namespace WaveTech.Scutex.WcfServices.ServicesLibrary
{
	internal static class Bootstrapper
	{
		private static bool IsInitialized;

		public static void Configure()
		{
			if (!IsInitialized)
			{
				ObjectFactory.Configure(scanner =>
				{
					scanner.AddRegistry(new FrameworkRegistry());
					scanner.AddRegistry(new ServicesRegistry());
					scanner.AddRegistry(new GeneratorRegistry());
					scanner.AddRegistry(new Generators.StaticKeyGeneratorLarge.GeneratorRegistry());
					scanner.AddRegistry(new Providers.AsymmetricEncryptionProvider.ProviderRegistry());
					scanner.AddRegistry(new Providers.DataGenerationProvider.ProviderRegistry());
					scanner.AddRegistry(new Providers.HashingProvider.ProviderRegistry());
					scanner.AddRegistry(new Providers.NetworkTimeProvider.ProviderRegistry());
					scanner.AddRegistry(new Providers.ObjectSerialization.ProviderRegistry());
					scanner.AddRegistry(new Providers.SymmetricEncryptionProvider.ProviderRegistry());
					scanner.AddRegistry(new Providers.WmiDataProvider.ProviderRegistry());
					scanner.AddRegistry(new ServicesLibraryRegistry());
					scanner.AddRegistry(new Repositories.ServicesDataRepository.ServicesDataRegistry());
					scanner.AddRegistry(new Repositories.ClientDataRepository.DataRegistry());
				}
																						);

				Framework.ObjectLocator.IsInitialized = true;
				IsInitialized = true;
			}
		}
	}

	#region Old Scanner Based Registry
	//public static class Bootstrapper
	//{
	//  private static bool IsInitialized;

	//  public static void Configure()
	//  {
	//    if (!IsInitialized)
	//    {
	//      string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
	//      path = path + "\\bin";

	//      ObjectFactory.Configure(x => x.Scan(
	//        scan =>
	//          {
	//            scan.AssembliesFromPath(path,
	//                                    assembly => assembly.GetName().Name.Contains("WaveTech.Scutex"));
	//            scan.WithDefaultConventions();
	//            scan.LookForRegistries();
	//          }
	//                                    ));

	//      Framework.ObjectLocator.IsInitialized = true;
	//      IsInitialized = true;
	//    }
	//  }
	//}
	#endregion
}