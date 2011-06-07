using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WaveTech.Scutex.Framework.Storage
{
	/// <summary>
	/// Inherits from <see cref="Store"/> to allow storing of application specific data
	/// in the current thread / web request.
	/// </summary>
	public class LocalStorage : Store
	{
		#region fields
		[ThreadStatic]
		private static Hashtable _internalStorage;
		#endregion

		#region properties

		/// <summary>
		/// Overriden. Configures the storage to not use locking when getting and setting values.
		/// </summary>
		protected override bool UseLocking
		{
			get { return false; }
		}

		/// <summary>
		/// Gets the object used by the <see cref="Store"/> for locking when retrieving and setting values.
		/// </summary>
		protected override object LockInstance
		{
			get { return null; }
		}

		#endregion

		#region methods

		/// <summary>
		/// Overriden. Gets the <see cref="Hashtable"/> used to store thread specific data.
		/// </summary>
		/// <returns>A <see cref="Hashtable"/> that can be used to store thread / request specific data.</returns>
		/// <remarks>
		/// No locking is used when initializing the Hashtables as only one request to GetInternalHashtable can be made at one time.
		/// This code block is thread-safe
		/// </remarks>
		protected override Hashtable GetInternalHashtable()
		{
			if (IsWebApplication)
			{
				//var internalStorage = HttpContext.Current.Items[typeof(LocalStorage).FullName] as Hashtable;
				//if (internalStorage == null)
				//  HttpContext.Current.Items[typeof(LocalStorage).FullName] = internalStorage = new Hashtable();
				//return internalStorage;

				return _internalStorage ?? (_internalStorage = new Hashtable());
			}
			else
				return _internalStorage ?? (_internalStorage = new Hashtable());
		}

		#endregion
	}
}
