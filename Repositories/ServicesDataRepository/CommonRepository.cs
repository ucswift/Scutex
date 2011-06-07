using System.Linq;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Repositories;

namespace WaveTech.Scutex.Repositories.ServicesDataRepository
{
	internal class CommonRepository : ICommonRepository
	{
		private readonly ScutexServiceEntities db;

		public CommonRepository(ScutexServiceEntities db)
		{
			this.db = db;
		}

		public MasterServiceData GetMasterServiceData()
		{
			return (from master in db.Masters
							select new MasterServiceData()
							{
								ServiceId = master.ServiceId,
								ClientInboundKey = master.ClientInboundKey,
								ClientOutboundKey = master.ClientOutboundKey,
								ManagementInboundKey = master.ManagementInboundKey,
								ManagementOutboundKey = master.ManagementOutboundKey,
								Token = master.Token
							}).FirstOrDefault();
		}

		public MasterServiceData SetMasterServiceData(MasterServiceData data)
		{
			if (GetMasterServiceData() != null)
				return null;

			using (ScutexServiceEntities db1 = new ScutexServiceEntities())
			{
				Master m = new Master();
				m.ServiceId = data.ServiceId;
				m.ClientInboundKey = data.ClientInboundKey;
				m.ClientOutboundKey = data.ClientOutboundKey;
				m.ManagementInboundKey = data.ManagementInboundKey;
				m.ManagementOutboundKey = data.ManagementOutboundKey;
				m.Token = data.Token;

				db1.AddToMasters(m);
				db1.SaveChanges();
			}

			return GetMasterServiceData();
		}
	}
}
