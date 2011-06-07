namespace WaveTech.Scutex.Framework
{
	internal class ApplicationConstants
	{
		public const bool IsCommunityEdition = true;

#if (NOLOG)
		public const bool IsLoggingEnabled = false;
#else
		public const bool IsLoggingEnabled = true;
#endif
	}
}