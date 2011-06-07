using System;
using NLog;
using NLog.Config;
using NLog.Targets;
using WaveTech.Scutex.Framework;

namespace WaveTech.Scutex.WcfServices.ServicesLibrary
{
	internal static class LoggingHelper
	{
		private static LoggingConfiguration _config;
		private static Logger _logger;
		private static bool _isInitialized;

		private static void Initialize()
		{
			if (_isInitialized == false)
			{
				// Step 1. Create configuration object 
				_config = new LoggingConfiguration();

				FileTarget fileTarget = new FileTarget();
				_config.AddTarget("file", fileTarget);

				string path = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;

				// set the file 
				fileTarget.FileName = path + string.Format("\\{0}-Log.txt", Guid.NewGuid());
				fileTarget.Layout = "${date:format=HH\\:MM\\:ss}: ${message}";

				// don’t clutter the hard drive
				fileTarget.DeleteOldFileOnStartup = true;

				// Step 4. Define rules 
				LoggingRule rule1 = new LoggingRule("*", LogLevel.Fatal, fileTarget);
				_config.LoggingRules.Add(rule1);

				LoggingRule rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
				_config.LoggingRules.Add(rule2);

				LoggingRule rule3 = new LoggingRule("*", LogLevel.Error, fileTarget);
				_config.LoggingRules.Add(rule3);

				// Step 5. Activate the configuration 
				LogManager.Configuration = _config;

				_logger = LogManager.GetLogger("ScutexServiceLogger");

				_isInitialized = true;
			}
		}

		public static void LogException(Exception exception)
		{
			if (ApplicationConstants.IsLoggingEnabled)
			{
				Initialize();

				_logger.LogException(LogLevel.Fatal, exception.ToString(), exception);
			}
		}

		public static void LogError(string message)
		{
			if (ApplicationConstants.IsLoggingEnabled)
			{
				Initialize();

				_logger.Error(message);
			}
		}

		public static void LogDebug(string message)
		{
			if (ApplicationConstants.IsLoggingEnabled)
			{
				Initialize();

				_logger.Debug(message);
			}
		}
	}
}