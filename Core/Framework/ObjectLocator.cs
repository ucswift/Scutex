using StructureMap;

namespace WaveTech.Scutex.Framework
{
	internal static class ObjectLocator
	{
		public static bool IsInitialized { get; set; }

		private static void Initialize()
		{
			if (!IsInitialized)
			{
				Bootstrapper.Configure();
				IsInitialized = true;
			}
		}

		public static T GetInstance<T>()
		{
			Initialize();
			return ObjectFactory.GetInstance<T>();
		}

		public static T GetInstance<T>(string name)
		{
			Initialize();
			return ObjectFactory.GetNamedInstance<T>(name);
		}
	}
}