namespace WaveTech.Scutex.WcfServices.ServicesLibrary
{
	internal static class MasterHooker
	{
		internal static void HookAsymEncryptionProvider()
		{
			Providers.AsymmetricEncryptionProvider.Hooker.HookAsymmetricEncryptionProvider();
		}

		internal static void HookHashingProvider()
		{
			Providers.HashingProvider.Hooker.HookHashingProvider();
		}

		internal static void HookObjectSerializationProvider()
		{
			Providers.ObjectSerialization.Hooker.HookObjectSerializationProvider();
		}

		internal static void HookSymEncryptionProvider()
		{
			Providers.SymmetricEncryptionProvider.Hooker.HookSymmetricEncryptionProvider();
		}

		internal static void HookWmiDataProvider()
		{
			Providers.WmiDataProvider.Hooker.HookWmiDataProvider();
		}

		internal static void HookServices()
		{
			WaveTech.Scutex.Services.Hooker.HookServices();
		}

		internal static void HookNetworkTimeProvider()
		{
			Providers.NetworkTimeProvider.Hooker.HookNetworkTimeProvider();
		}

		internal static void HookServicesDataRepository()
		{
			Repositories.ServicesDataRepository.Hooker.HookServicesDataRepository();
		}

		internal static void HookDataGenerationProvider()
		{
			Providers.DataGenerationProvider.Hooker.HookDataGenerationProvider();
		}

		internal static void HookSmallKeyGenerator()
		{
			Generators.StaticKeyGeneratorSmall.Hooker.HookStaticKeyGeneratorSmall();
		}

		internal static void HooKLargeKeyGenerator()
		{
			Generators.StaticKeyGeneratorLarge.Hooker.HookStaticKeyGeneratorLarge();
		}

		internal static void HookClientDataRepo()
		{
			Repositories.ClientDataRepository.Hooker.HookClientDataDepository();
		}

		internal static void HookSharpZipLib()
		{
			ICSharpCode.SharpZipLib.Zip.FastZip fz = new ICSharpCode.SharpZipLib.Zip.FastZip();
		}

		internal static void HookAutomapper()
		{
			AutoMapper.PropertyMap pm = new AutoMapper.PropertyMap(null);
		}
	}
}