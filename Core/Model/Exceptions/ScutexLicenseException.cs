using System;
using System.Runtime.Serialization;

namespace WaveTech.Scutex.Model.Exceptions
{
	/// <summary>
	/// This is a generic exception inside the Scutex licensing system
	/// </summary>
	[Serializable]
	public class ScutexLicenseException : Exception
	{
		public ScutexLicenseException() : base() { }
		public ScutexLicenseException(string message) : base(message) { }
		public ScutexLicenseException(string message, Exception inner) : base (message, inner) { }
		protected ScutexLicenseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}