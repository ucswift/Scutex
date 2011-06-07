using System.Linq;
using AutoMapper;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Repositories;

namespace WaveTech.Scutex.Repositories.ManagerDataRepository
{
	internal class ServicesRepository : IServicesRepository
	{
		private readonly ScutexEntities db;

		public ServicesRepository(ScutexEntities db)
		{
			this.db = db;
		}

		public IQueryable<Model.Service> GetAllServices()
		{
			return from service in db.Services
						 select new Model.Service
						 {
							 ServiceId = service.ServiceId,
							 Name = service.Name,
							 ClientUrl = service.ClientUrl,
							 ManagementUrl = service.ManagementUrl,
							 Token = service.Token,
							 InboundKeyPair = new KeyPair { PublicKey = service.InboundPublicKey, PrivateKey = service.InboundPrivateKey },
							 OutboundKeyPair = new KeyPair { PublicKey = service.OutboundPublicKey, PrivateKey = service.OutboundPrivateKey },
							 ManagementInboundKeyPair = new KeyPair { PublicKey = service.ManagementInboundPublicKey, PrivateKey = service.ManagementInboundPrivateKey },
							 ManagementOutboundKeyPair = new KeyPair { PublicKey = service.ManagementOutboundPublicKey, PrivateKey = service.ManagementOutboundPrivateKey },
							 UniquePad = service.UniquePad,
							 Initialized = service.Initialized,
							 Tested = service.Tested,
							 LockToIp = service.LockToIp,
							 ClientRequestToken = service.ClientRequestToken,
							 ManagementRequestToken = service.ManagementRequestToken,
							 CreatedDate = service.CreatedDate
						 };
		}

		public IQueryable<Model.Service> GetServiceById(int serviceId)
		{
			return from service in GetAllServices()
						 where service.ServiceId == serviceId
						 select service;
		}

		public IQueryable<Model.Service> InsertService(Model.Service service)
		{
			int newId;

			//using (ScutexEntities db1 = new ScutexEntities())
			//{
			Service serv = new Service();

			Mapper.CreateMap<Model.Service, Service>();
			serv = Mapper.Map<Model.Service, Service>(service);

			serv.InboundPrivateKey = service.InboundKeyPair.PrivateKey;
			serv.InboundPublicKey = service.InboundKeyPair.PublicKey;
			serv.OutboundPrivateKey = service.OutboundKeyPair.PrivateKey;
			serv.OutboundPublicKey = service.OutboundKeyPair.PublicKey;
			serv.ManagementInboundPrivateKey = service.ManagementInboundKeyPair.PrivateKey;
			serv.ManagementInboundPublicKey = service.ManagementInboundKeyPair.PublicKey;
			serv.ManagementOutboundPrivateKey = service.ManagementOutboundKeyPair.PrivateKey;
			serv.ManagementOutboundPublicKey = service.ManagementOutboundKeyPair.PublicKey;

			db.AddToServices(serv);
			db.SaveChanges();

			newId = serv.ServiceId;
			//}

			return GetServiceById(newId);
		}

		public IQueryable<Model.Service> UpdateService(Model.Service service)
		{
			int newId;

			//using (ScutexEntities db1 = new ScutexEntities())
			//{
			var svc = (from s in db.Services
								 where s.ServiceId == service.ServiceId
								 select s).First();

			svc.Name = service.Name;
			svc.ClientUrl = service.ClientUrl;
			svc.ManagementUrl = service.ManagementUrl;
			svc.Token = service.Token;
			svc.Initialized = service.Initialized;
			svc.Tested = service.Tested;
			svc.LockToIp = service.LockToIp;
			svc.InboundPrivateKey = service.InboundKeyPair.PrivateKey;
			svc.InboundPublicKey = service.InboundKeyPair.PublicKey;
			svc.OutboundPrivateKey = service.OutboundKeyPair.PrivateKey;
			svc.OutboundPublicKey = service.OutboundKeyPair.PublicKey;
			svc.ManagementInboundPrivateKey = service.ManagementInboundKeyPair.PrivateKey;
			svc.ManagementInboundPublicKey = service.ManagementInboundKeyPair.PublicKey;
			svc.ManagementOutboundPrivateKey = service.ManagementOutboundKeyPair.PrivateKey;
			svc.ManagementOutboundPublicKey = service.ManagementOutboundKeyPair.PublicKey;
			svc.UniquePad = service.UniquePad;

			db.SaveChanges();

			newId = svc.ServiceId;
			//}

			return GetServiceById(newId);
		}

		public void DeleteServiceById(int serviceId)
		{
			//using (ScutexEntities db1 = new ScutexEntities())
			//{
			var services = from s in db.Services
										 where s.ServiceId == serviceId
										 select s;

			foreach (var i in services)
				db.Services.DeleteObject(i);

			db.SaveChanges();
			//}
		}
	}
}