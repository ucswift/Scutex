using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace WaveTech.Scutex.Framework
{
	internal static class Logging
	{
		private static LoggingConfiguration _config;
		private static Logger _logger;
		private static bool _isInitialized;
		private static bool _consoleVisible;

		private static void Initialize()
		{
			if (_isInitialized == false)
			{
				// Step 1. Create configuration object 
				_config = new LoggingConfiguration();

				// Step 2. Create targets and add them to the configuration 
				//ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget();
				//config.AddTarget("console", consoleTarget);

				FileTarget fileTarget = new FileTarget();
				_config.AddTarget("file", fileTarget);

				// Step 3. Set target properties 
				//consoleTarget.Layout = "${date:format=HH\\:MM\\:ss}: ${message}";

				string path = null;
				try
				{
					path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
					path = path.Replace("file:\\", "");
				}
				catch
				{ }

				if (String.IsNullOrEmpty(path))
					path = Path.GetTempPath();

				// set the file 
				fileTarget.FileName = path + "Scutex-Log.txt";
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

				_logger = LogManager.GetLogger("ScutexLogger");

				_isInitialized = true;
			}
		}

		private static void ShowConsole()
		{
			if (!_consoleVisible)
			{
				Win32.AllocConsole();
				_consoleVisible = true;
			}
		}

		public static void LogException(Exception exception)
		{
			if (ApplicationConstants.IsLoggingEnabled)
			{
				try
				{
					Initialize();

					_logger.LogException(LogLevel.Fatal, exception.ToString(), exception);
				}
				catch
				{
					FallbackLogger(exception.ToString());
				}
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

		public static void Write(string message)
		{
			if (ApplicationConstants.IsLoggingEnabled)
			{
				ShowConsole();
				Console.WriteLine(message);
			}
		}

		private static void FallbackLogger(string message)
		{
			try
			{
				if (!EventLog.SourceExists("Scutex"))
					EventLog.CreateEventSource("Scutex", "Application");

				EventLog.WriteEntry("Scutex", message, EventLogEntryType.Warning, 500);
			}
			catch { }
		}
	}
}
