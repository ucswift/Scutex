
using StructureMap;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Generators.StaticKeyGeneratorSmall;
using WaveTech.Scutex.Services;

namespace WaveTech.Scutex.Licensing
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
						scanner.AddRegistry(new Providers.AsymmetricEncryptionProvider.ProviderRegistry());
						scanner.AddRegistry(new Providers.CompressionProvider.ProviderRegistry());
						scanner.AddRegistry(new Providers.DataGenerationProvider.ProviderRegistry());
						scanner.AddRegistry(new Providers.HashingProvider.ProviderRegistry());
						scanner.AddRegistry(new Providers.NetworkTimeProvider.ProviderRegistry());
						scanner.AddRegistry(new Providers.ObjectSerialization.ProviderRegistry());
						scanner.AddRegistry(new Providers.SymmetricEncryptionProvider.ProviderRegistry());
						scanner.AddRegistry(new Providers.WebServicesProvider.ProviderRegistry());
						scanner.AddRegistry(new Providers.WmiDataProvider.ProviderRegistry());
						scanner.AddRegistry(new Providers.ComBypassProvider.ProviderRegistry());

						scanner.AddRegistry(new Repositories.ClientDataRepository.DataRegistry());
					}
																						);

				Framework.ObjectLocator.IsInitialized = true;
				IsInitialized = true;
			}
		}
	}
}