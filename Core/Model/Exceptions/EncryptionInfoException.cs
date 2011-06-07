using System;
using System.Runtime.Serialization;

namespace WaveTech.Scutex.Model.Exceptions
{
	/// <summary>
	/// The EncryptionInfoException occurs when attempting to set invalid
	/// values inside the <see cref="EncryptionInfo"/> EncryptionInfo class.
	/// </summary>
	[Serializable]
	public class EncryptionInfoException : Exception
	{
		public EncryptionInfoException() : base() { }
		public EncryptionInfoException(string message) : base(message) { }
		public EncryptionInfoException(string message, Exception inner) : base(message, inner) { }
		protected EncryptionInfoException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}