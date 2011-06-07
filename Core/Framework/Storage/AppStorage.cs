using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WaveTech.Scutex.Framework.Storage
{
	///<summary>
	/// Inherits from <see cref="Store"/> to allow storing of application specific data
	/// in the AppDomain / current HttpContext.
	///</summary>
	public class AppStorage : Store
	{
		#region fields

		private static Hashtable _internalStorage;

		#endregion

		#region properties

		/// <summary>
		/// Overriden. Configures the storage to use locking when getting and setting values.
		/// </summary>
		protected override bool UseLocking
		{
			get { return true; }
		}

		/// <summary>
		/// Gets the object used by the <see cref="Store"/> for locking when retrieving and setting values.
		/// </summary>
		protected override object LockInstance
		{
			get { return AppStorageLock; }
		}

		#endregion

		#region methods

		///<summary>
		/// Overriden. Gets the internal hash table that is used to store and retrieve application
		/// specific data.
		///</summary>
		///<returns>A <see cref="Hashtable"/> that is used to store application specific data.</returns>
		/// <remarks>
		/// This method implementation uses locking as multiple threads (or requests in the case of a web app) can call
		/// the GetInternalHashtable at the same time.
		/// </remarks>
		protected override Hashtable GetInternalHashtable()
		{
			if (IsWebApplication)
			{
				//This code is executing under a WebSite. Use the Application context to retrieve the hash table.
				//var internalHashtable = HttpContext.Current.Application[typeof(AppStorage).FullName] as Hashtable;
				//if (internalHashtable == null)
				//{
				//  lock (AppStorageLock)
				//  {
				//    internalHashtable = HttpContext.Current.Application[typeof(AppStorage).FullName] as Hashtable;
				//    if (internalHashtable == null)
				//      HttpContext.Current.Application[typeof(AppStorage).FullName] =
				//          internalHashtable = new Hashtable();
				//  }
				//}
				//return internalHashtable;
			}

			//The code is running under a normal windows application. Use the static property.
			if (_internalStorage == null)
			{
				lock (SessionStorageLock)
				{
					if (_internalStorage == null)
						_internalStorage = new Hashtable();
				}
			}
			return _internalStorage;
		}

		#endregion
	}
}
