using System;
using System.Runtime.Serialization;

namespace WaveTech.Scutex.Model.Exceptions
{
	/// <summary>
	/// This is a exception occurs when the Scutex licensing system
	/// fails internal checks and audits to ensure the system is
	/// working correctly
	/// </summary>
	[Serializable]
	public class ScutexAuditException : Exception
	{
		public ScutexAuditException() : base() { }
		public ScutexAuditException(string message) : base(message) { }
		public ScutexAuditException(string message, Exception inner) : base(message, inner) { }
		protected ScutexAuditException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}