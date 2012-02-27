using StructureMap;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Services;

namespace WaveTech.Scutex.FingerprintViewer
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
						scanner.AddRegistry(new Providers.HashingProvider.ProviderRegistry());
						scanner.AddRegistry(new Providers.WmiDataProvider.ProviderRegistry());

					}
																						);

				Framework.ObjectLocator.IsInitialized = true;
				IsInitialized = true;
			}
		}
	}
}