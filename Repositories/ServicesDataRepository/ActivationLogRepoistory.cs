using System.Linq;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Repositories;

namespace WaveTech.Scutex.Repositories.ServicesDataRepository
{
	internal class ActivationLogRepoistory : IActivationLogRepoistory
	{
		private readonly ScutexServiceEntities db;

		public ActivationLogRepoistory(ScutexServiceEntities db)
		{
			this.db = db;
		}

		public IQueryable<Model.ActivationLog> GetAllActivationLogs()
		{
			return from al in db.ActivationLogs
						 select new Model.ActivationLog
						 {
							 LicenseKey = al.LicenseKey,
							 ActivationResult = (ActivationResults)al.ActivationResult,
							 IpAddress = al.IPAddress,
							 Timestamp = al.Timestamp
						 };
		}

		public void SaveActivationLog(Model.ActivationLog activationLog)
		{
			using (ScutexServiceEntities db1 = new ScutexServiceEntities())
			{
				ActivationLog al = new ActivationLog();
				al.LicenseKey = activationLog.LicenseKey;
				al.ActivationResult = (int)activationLog.ActivationResult;
				al.IPAddress = activationLog.IpAddress;
				al.Timestamp = activationLog.Timestamp;

				db1.AddToActivationLogs(al);
				db1.SaveChanges();
			}
		}

		public void DeleteActivationLogByKey(string key)
		{
			using (ScutexServiceEntities db1 = new ScutexServiceEntities())
			{
				var logs = from l in db1.ActivationLogs
									 where l.LicenseKey == key
									 select l;

				foreach (var l in logs)
				{
					db1.ActivationLogs.DeleteObject(l);
				}

				db1.SaveChanges();
			}
		}
	}
}
