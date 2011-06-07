using System;
using System.Collections.Generic;
using DbUp;

namespace WaveTech.Scutex.Providers.DatabaseUpdateProvider
{
	/// <summary>
	/// A log that writes to the console in a colorful way.
	/// </summary>
	public class MemoryLog : ILog
	{
		private List<string> _logData;

		public MemoryLog()
		{
			_logData = new List<string>();
		}

		/// <summary>
		/// Writes an informational message to the log.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="args">The args.</param>
		public void WriteInformation(string format, params object[] args)
		{
			_logData.Add(args.ToString());
		}

		/// <summary>
		/// Writes an error message to the log.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="args">The args.</param>
		public void WriteError(string format, params object[] args)
		{
			_logData.Add(args.ToString());
		}

		/// <summary>
		/// Writes a warning message to the log.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="args">The args.</param>
		public void WriteWarning(string format, params object[] args)
		{
			_logData.Add(args.ToString());
		}
	}
}
