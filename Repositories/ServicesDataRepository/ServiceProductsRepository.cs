using System.Collections.Generic;
using System.Linq;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Repositories;

namespace WaveTech.Scutex.Repositories.ServicesDataRepository
{
	internal class ServiceProductsRepository : IServiceProductsRepository
	{
		private readonly ScutexServiceEntities db;

		public ServiceProductsRepository(ScutexServiceEntities db)
		{
			this.db = db;
		}

		public List<ServiceProduct> GetAllProducts()
		{
			return (from lic in db.Licenses.AsEnumerable()
							select new ServiceProduct()
							{
								LicenseId = lic.LicenseId,
								LicenseName = lic.Name,
								LicenseSets = GetAllServiceLicenseSets().Where(x => x.LicenseId == lic.LicenseId).ToList()
							}).ToList();
		}

		public ServiceProduct GetProduct(int licenseId)
		{
			return (from p in GetAllProducts()
							where p.LicenseId == licenseId
							select p).FirstOrDefault();
		}

		public List<ServiceLicenseSet> GetAllServiceLicenseSets()
		{
			return (from licSet in db.LicenseSets.AsEnumerable()
							select new ServiceLicenseSet()
											{
												LicenseId = licSet.LicenseId,
												LicenseSetId = licSet.LicenseSetId,
												LicenseSetName = licSet.Name,
												LicenseType = (LicenseKeyTypeFlag)licSet.LicenseType,
												MaxUsers = licSet.MaxUsers,
												Keys = GetAllServiceLicenseKeys().Where(x => x.LicenseSetId == licSet.LicenseSetId).ToList()
											}).ToList();
		}

		public ServiceLicenseSet GetServiceLicenseSetById(int licenseSetId)
		{
			return (from ls in GetAllServiceLicenseSets()
							where ls.LicenseSetId == licenseSetId
							select ls).FirstOrDefault();
		}

		public List<ServiceLicenseSet> GetServiceLicenseSetsByProduct(int licenseId)
		{
			return (from ls in GetAllServiceLicenseSets()
							where ls.LicenseId == licenseId
							select ls).ToList();
		}

		public List<ServiceLicenseKey> GetAllServiceLicenseKeys()
		{
			return (from key in db.LicenseKeys
							select new ServiceLicenseKey()
							{
								LicenseKeyId = key.LicenseKeyId,
								LicenseSetId = key.LicenseSetId,
								Key = key.Key,
								CreatedOn = key.CreatedOn,
								ActivationCount = key.ActivationCount,
								Deactivated = key.Deactivated,
								DeactivatedReason = key.DeactivatedReason,
								DeactivatedOn = key.DeactivatedOn,
							}).ToList();
		}

		public List<ServiceLicenseKey> GetServiceLicenseKeysByLicenseSet(int licenseSetId)
		{
			return (from key in GetAllServiceLicenseKeys()
							where key.LicenseSetId == licenseSetId
							select key).ToList();
		}

		public ServiceLicenseKey GetServiceLicenseKeyByKey(string key)
		{
			return (from k in GetAllServiceLicenseKeys()
							where k.Key == key
							select k).FirstOrDefault();
		}

		public ServiceLicenseKey GetServiceLicenseKeyById(int licenseKeyId)
		{
			return (from k in GetAllServiceLicenseKeys()
							where k.LicenseKeyId == licenseKeyId
							select k).FirstOrDefault();
		}

		public ServiceLicenseKey GetServiceLicenseKeyByKeyLicenseSet(string key, int licenseSetId)
		{
			return (from k in GetAllServiceLicenseKeys()
							where k.Key == key && k.LicenseSetId == licenseSetId
							select k).FirstOrDefault();
		}

		public ServiceProduct SaveServiceProduct(ServiceProduct product)
		{
			if (GetProduct(product.LicenseId) != null)
				UpdateServiceProduct(product);
			else
				InsertServiceProduct(product);

			foreach (var ls in product.LicenseSets)
			{
				SaveServiceLicenseSet(ls);
			}

			return GetProduct(product.LicenseId);
		}

		private void UpdateServiceProduct(ServiceProduct product)
		{
			using (ScutexServiceEntities db1 = new ScutexServiceEntities())
			{
				var lic = (from l in db1.Licenses
									 where l.LicenseId == product.LicenseId
									 select l).First();

				lic.Name = product.LicenseName;

				db1.SaveChanges();
			}
		}

		private void InsertServiceProduct(ServiceProduct product)
		{
			using (ScutexServiceEntities db1 = new ScutexServiceEntities())
			{
				License l = new License();
				l.LicenseId = product.LicenseId;
				l.Name = product.LicenseName;

				db1.AddToLicenses(l);
				db1.SaveChanges();
			}
		}

		private void DeleteServiceProductById(int productId)
		{
			using (ScutexServiceEntities db1 = new ScutexServiceEntities())
			{
				var prods = from prod in db1.Licenses
								where prod.LicenseId == productId
								select prod;

				foreach (var p in prods)
					db1.Licenses.DeleteObject(p);

				db1.SaveChanges();
			}
		}

		public ServiceLicenseSet SaveServiceLicenseSet(ServiceLicenseSet licenseSet)
		{
			if (GetServiceLicenseSetById(licenseSet.LicenseSetId) != null)
				UpdateServiceLicenseSet(licenseSet);
			else
				InsertServiceLicenseSet(licenseSet);

			foreach (var key in licenseSet.Keys)
			{
				SaveServiceLicenseKey(key);
			}

			return GetServiceLicenseSetById(licenseSet.LicenseSetId);
		}

		private void UpdateServiceLicenseSet(ServiceLicenseSet licenseSet)
		{
			using (ScutexServiceEntities db1 = new ScutexServiceEntities())
			{
				var licSet = (from ls in db1.LicenseSets
											where ls.LicenseSetId == licenseSet.LicenseSetId
											select ls).First();

				licSet.LicenseSetId = licenseSet.LicenseSetId;
				licSet.LicenseId = licenseSet.LicenseId;
				licSet.Name = licenseSet.LicenseSetName;
				licSet.LicenseType = (int)licenseSet.LicenseType;
				licSet.MaxUsers = licenseSet.MaxUsers;

				db1.SaveChanges();
			}
		}

		private void InsertServiceLicenseSet(ServiceLicenseSet licenseSet)
		{
			using (ScutexServiceEntities db1 = new ScutexServiceEntities())
			{
				LicenseSet ls = new LicenseSet();
				ls.LicenseSetId = licenseSet.LicenseSetId;
				ls.LicenseId = licenseSet.LicenseId;
				ls.Name = licenseSet.LicenseSetName;
				ls.LicenseType = (int)licenseSet.LicenseType;
				ls.MaxUsers = licenseSet.MaxUsers;

				db1.AddToLicenseSets(ls);
				db1.SaveChanges();
			}
		}

		public ServiceLicenseKey SaveServiceLicenseKey(ServiceLicenseKey serviceLicenseKey)
		{
			if (GetServiceLicenseKeyByKey(serviceLicenseKey.Key) != null)
				UpdateServiceLicenseKey(serviceLicenseKey);
			else
				InsertServiceLicenseKey(serviceLicenseKey);

			return GetServiceLicenseKeyByKey(serviceLicenseKey.Key);
		}

		private void UpdateServiceLicenseKey(ServiceLicenseKey serviceLicenseKey)
		{
			using (ScutexServiceEntities db1 = new ScutexServiceEntities())
			{
				var licKey = (from lk in db1.LicenseKeys
											where lk.Key == serviceLicenseKey.Key
											select lk).First();

				licKey.Key = serviceLicenseKey.Key;
				licKey.CreatedOn = serviceLicenseKey.CreatedOn;
				licKey.ActivationCount = serviceLicenseKey.ActivationCount;
				licKey.Deactivated = serviceLicenseKey.Deactivated;
				licKey.DeactivatedOn = serviceLicenseKey.DeactivatedOn;
				licKey.DeactivatedReason = serviceLicenseKey.DeactivatedReason;
				licKey.LicenseSetId = serviceLicenseKey.LicenseSetId;

				db1.SaveChanges();
			}
		}

		private void InsertServiceLicenseKey(ServiceLicenseKey serviceLicenseKey)
		{
			using (ScutexServiceEntities db1 = new ScutexServiceEntities())
			{
				LicenseKey licKey = new LicenseKey();
				licKey.Key = serviceLicenseKey.Key;
				licKey.ActivationCount = serviceLicenseKey.ActivationCount;
				licKey.Deactivated = serviceLicenseKey.Deactivated;
				licKey.DeactivatedOn = serviceLicenseKey.DeactivatedOn;
				licKey.DeactivatedReason = serviceLicenseKey.DeactivatedReason;
				licKey.LicenseSetId = serviceLicenseKey.LicenseSetId;
				licKey.CreatedOn = serviceLicenseKey.CreatedOn;

				db1.AddToLicenseKeys(licKey);
				db1.SaveChanges();
			}
		}

		public void DeleteLicenseActivationsByKeyId(int licenseKeyId)
		{
			using (ScutexServiceEntities db1 = new ScutexServiceEntities())
			{
				var licenseActivations = from la in db1.LicenseActivations
																 where la.LicenseKeyId == licenseKeyId
																 select la;

				foreach (var i in licenseActivations)
					db1.LicenseActivations.DeleteObject(i);

				db1.SaveChanges();
			}
		}

		public void DeleteLicenseKeyByKey(string licenseKey)
		{
			using (ScutexServiceEntities db1 = new ScutexServiceEntities())
			{
				var keys = from lk in db1.LicenseKeys
									 where lk.Key == licenseKey
									 select lk;

				foreach (var i in keys)
					db1.LicenseKeys.DeleteObject(i);

				db1.SaveChanges();
			}
		}

		public void DeleteLicenseKeyByLicenseSetId(int licenseSetId)
		{
			using (ScutexServiceEntities db1 = new ScutexServiceEntities())
			{
				var keys = from lk in db1.LicenseKeys
									 where lk.LicenseSetId == licenseSetId
									 select lk;

				foreach (var i in keys)
					db1.LicenseKeys.DeleteObject(i);

				db1.SaveChanges();
			}
		}

		public void DeleteLicenseSetId(int licenseSetId)
		{
			using (ScutexServiceEntities db1 = new ScutexServiceEntities())
			{
				var keys = from lk in db1.LicenseSets
									 where lk.LicenseSetId == licenseSetId
									 select lk;

				foreach (var i in keys)
					db1.LicenseSets.DeleteObject(i);

				db1.SaveChanges();
			}
		}

		public void DeleteLicenseId(int licenseId)
		{
			using (ScutexServiceEntities db1 = new ScutexServiceEntities())
			{
				var keys = from lk in db1.Licenses
									 where lk.LicenseId == licenseId
									 select lk;

				foreach (var i in keys)
					db1.Licenses.DeleteObject(i);

				db1.SaveChanges();
			}
		}
	}
}