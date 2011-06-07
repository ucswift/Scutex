using System;
using System.Runtime.Serialization;

namespace WaveTech.Scutex.Model.Exceptions
{
	/// <summary>
	/// This exception occurs when too many invalid key validation attempts
	/// have been conducted via the Trial/Registration form
	/// </summary>
	[Serializable]
	public class ScutexAttemptsException : Exception
	{
		public ScutexAttemptsException() : base() { }
		public ScutexAttemptsException(string message) : base(message) { }
		public ScutexAttemptsException(string message, Exception inner) : base(message, inner) { }
		protected ScutexAttemptsException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}