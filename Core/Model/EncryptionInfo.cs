using System;
using WaveTech.Scutex.Model.Exceptions;

namespace WaveTech.Scutex.Model
{
	public class EncryptionInfo
	{
		#region Private Members
		private int _keySize;
		private string _hashAlgorithm;
		private string _initVector;
		#endregion Private Members

		#region Public Properties
		/// <summary>
		/// Passphrase from which a pseudo-random password will be derived. The
		/// derived password will be used to generate the encryption key.
		/// Passphrase can be any string. In this example we assume that this
		/// passphrase is an ASCII string.
		/// </summary>
		public string PassPhrase { get; set;}

		/// <summary>
		/// Salt value used along with passphrase to generate password. Salt can
		/// be any string. In this example we assume that salt is an ASCII string.
		/// </summary>
		public string SaltValue { get; set;}
		
		/// <summary>
		/// Number of iterations used to generate password. One or two iterations
		/// should be enough.
		/// </summary>
		public int Iterations { get; set;}
		
		/// <summary>
		/// Size of encryption key in bits. Allowed values are: 128, 192, and 256. 
		/// Longer keys are more secure than shorter keys.
		/// </summary>
		public int KeySize 
		{ 
			get
			{
				return _keySize;
			}
			set
			{
				if (value != 128 && value != 192 && value != 256)
					throw new EncryptionInfoException("The EncryptionInfo key size is limited to values of 128, 192 or 256.");

				_keySize = value;
			}
		}

		/// <summary>
		/// Hash algorithm used to generate password. Allowed values are: "MD5" and
		/// "SHA1". SHA1 hashes are a bit slower, but more secure than MD5 hashes.
		/// </summary>
		public string HashAlgorithm 
		{
			get
			{
				return _hashAlgorithm;
			}
			set
			{
				if (value != "SHA1" && value != "MD5")
					throw new EncryptionInfoException("The EncryptionInfo Hash Algorithm is limited to values of SHA1 or MD5.");

				_hashAlgorithm = value;
			}
		}

		/// <summary>
		/// Initialization vector (or IV). This value is required to encrypt the
		/// first block of plaintext data. For RijndaelManaged class IV must be 
		/// exactly 16 ASCII characters long.
		/// </summary>
		public string InitVector
		{
			get
			{
				return _initVector;
			}
			set
			{
				if (value.Length != 16)
					throw new EncryptionInfoException("The EncryptionInfo Init Vector must be 16 characters in length.");

				_initVector = value;
			}
		}
		#endregion Public Properties

		public bool Equals(EncryptionInfo other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return other._keySize == _keySize && Equals(other._hashAlgorithm, _hashAlgorithm) && Equals(other._initVector, _initVector) && Equals(other.PassPhrase, PassPhrase) && Equals(other.SaltValue, SaltValue) && other.Iterations == Iterations;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (EncryptionInfo)) return false;
			return Equals((EncryptionInfo) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = _keySize;
				result = (result*397) ^ (_hashAlgorithm != null ? _hashAlgorithm.GetHashCode() : 0);
				result = (result*397) ^ (_initVector != null ? _initVector.GetHashCode() : 0);
				result = (result*397) ^ (PassPhrase != null ? PassPhrase.GetHashCode() : 0);
				result = (result*397) ^ (SaltValue != null ? SaltValue.GetHashCode() : 0);
				result = (result*397) ^ Iterations;
				return result;
			}
		}

		public static bool operator ==(EncryptionInfo left, EncryptionInfo right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(EncryptionInfo left, EncryptionInfo right)
		{
			return !Equals(left, right);
		}
	}
}