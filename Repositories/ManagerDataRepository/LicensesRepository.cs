using System;
using System.Linq;
using System.Transactions;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Repositories;

namespace WaveTech.Scutex.Repositories.ManagerDataRepository
{
	internal class LicensesRepository : ILicensesRepository
	{
		private readonly ScutexEntities db;
		private readonly IFeaturesRepository _featuresRepository;
		private readonly IProductsRepository _productsRepository;
		private readonly ILicenseSetsRepository _licenseSetsRepository;
		private readonly ITrialSettingsRepository _trialSettingsRepository;
		private readonly IServicesRepository _servicesRepository;

		public LicensesRepository(ScutexEntities db, IFeaturesRepository featuresRepository, IProductsRepository productsRepository,
			ILicenseSetsRepository licenseSetsRepository, ITrialSettingsRepository trialSettingsRepository, IServicesRepository servicesRepository)
		{
			this.db = db;

			_featuresRepository = featuresRepository;
			_productsRepository = productsRepository;
			_licenseSetsRepository = licenseSetsRepository;
			_trialSettingsRepository = trialSettingsRepository;
			_servicesRepository = servicesRepository;
		}

		public IQueryable<Model.License> GetAllLicenses()
		{
			var query = from lic in db.Licenses.AsEnumerable()
									select new Model.License
													 {
														 LicenseId = lic.LicenseId,
														 Name = lic.Name,
														 BuyNowUrl = lic.BuyNowUrl,
														 ProductUrl = lic.ProductUrl,
														 EulaUrl = lic.EulaUrl,
														 SupportEmail = lic.SupportEmail,
														 SalesEmail = lic.SalesEmail,
														 KeyGeneratorType = (KeyGeneratorTypes)lic.KeyGeneratorTypeId,
														 CreatedOn = lic.CreatedOn,
														 UpdatedOn = lic.UpdatedOn,
														 UniqueId = lic.UniqueId,
														 Product = _productsRepository.GetAllProducts().Where(x => x.ProductId == lic.ProductId).FirstOrDefault(),
														 KeyPair = new KeyPair { PrivateKey = lic.PrivateKey, PublicKey = lic.PublicKey },
														 TrailNotificationSettings = new TrailNotificationSettings { TryButtonDelay = lic.TrialTryButtonDelay },
														 LicenseSets = new NotifyList<Model.LicenseSet>(_licenseSetsRepository.GetAllLicenseSets().Where(x => x.LicenseId == lic.LicenseId).ToList()),
														 TrialSettings = _trialSettingsRepository.GetAllTrialSettings().Where(x => x.LicenseId == lic.LicenseId).FirstOrDefault(),
														 Service = _servicesRepository.GetAllServices().Where(x => x.ServiceId == lic.ServiceId).FirstOrDefault()
													 };

			return query.AsQueryable();
		}

		public IQueryable<Model.License> GetLicenseById(int licenseId)
		{
			return from l in GetAllLicenses()
						 where l.LicenseId == licenseId
						 select l;
		}

		public IQueryable<Model.License> InsertLicense(Model.License license)
		{
			int newId = 0;

			using (TransactionScope scope = new TransactionScope())
			{
				//using (ScutexEntities db1 = new ScutexEntities())
				//{
				License l = new License();

				//Mapper.CreateMap<Model.License, License>();
				//l = Mapper.Map<Model.License, License>(license);

				l.Name = license.Name;
				l.BuyNowUrl = license.BuyNowUrl;
				l.ProductUrl = license.ProductUrl;
				l.EulaUrl = license.EulaUrl;
				l.SupportEmail = license.SupportEmail;
				l.SalesEmail = license.SalesEmail;
				l.KeyGeneratorTypeId = (int)license.KeyGeneratorType;
				l.CreatedOn = license.CreatedOn;

				if (license.UpdatedOn.HasValue)
					l.UpdatedOn = license.UpdatedOn;

				if (license.Service != null)
					l.ServiceId = license.Service.ServiceId;

				l.UniqueId = license.UniqueId;
				l.ProductId = license.Product.ProductId;
				l.PrivateKey = license.KeyPair.PrivateKey;
				l.PublicKey = license.KeyPair.PublicKey;

				if (license.TrailNotificationSettings != null)
					l.TrialTryButtonDelay = license.TrailNotificationSettings.TryButtonDelay;

				db.AddToLicenses(l);
				db.SaveChanges();

				newId = l.LicenseId;
				//}

				foreach (var ls in license.LicenseSets)
				{
					ls.LicenseId = newId;
					_licenseSetsRepository.InsertLicenseSet(ls);
				}

				license.TrialSettings.LicenseId = newId;
				_trialSettingsRepository.InsertTrialSettings(license.TrialSettings);

				scope.Complete();
			}

			return GetLicenseById(newId);
		}

		public IQueryable<Model.License> UpdateLicense(Model.License license)
		{
			int newId;

			using (TransactionScope scope = new TransactionScope())
			{
				//using (ScutexEntities db1 = new ScutexEntities())
				//{
				var lic = (from l in db.Licenses
									 where l.LicenseId == license.LicenseId
									 select l).First();

				lic.Name = license.Name;
				lic.BuyNowUrl = license.BuyNowUrl;
				lic.ProductUrl = license.ProductUrl;
				lic.EulaUrl = license.EulaUrl;
				lic.SupportEmail = license.SupportEmail;
				lic.SalesEmail = license.SalesEmail;
				lic.KeyGeneratorTypeId = (int)license.KeyGeneratorType;
				lic.CreatedOn = license.CreatedOn;

				if (license.UpdatedOn.HasValue)
					lic.UpdatedOn = license.UpdatedOn;

				lic.UniqueId = license.UniqueId;
				lic.ProductId = license.Product.ProductId;
				lic.PrivateKey = license.KeyPair.PrivateKey;
				lic.PublicKey = license.KeyPair.PublicKey;

				if (license.Service != null)
					lic.ServiceId = license.Service.ServiceId;

				if (license.TrailNotificationSettings != null)
					lic.TrialTryButtonDelay = license.TrailNotificationSettings.TryButtonDelay;

				db.SaveChanges();

				newId = lic.LicenseId;
				//}

				_productsRepository.UpdateProduct(license.Product);

				foreach (var ls in license.LicenseSets)
				{
					ls.LicenseId = newId;

					if (ls.LicenseSetId == 0)
						_licenseSetsRepository.InsertLicenseSet(ls);
					else
						_licenseSetsRepository.UpdateLicenseSet(ls);
				}

				_trialSettingsRepository.DeleteTrialSettingsByLicenseId(newId);
				license.TrialSettings.LicenseId = newId;
				_trialSettingsRepository.InsertTrialSettings(license.TrialSettings);

				scope.Complete();
			}

			return GetLicenseById(newId);
		}

		public void DeleteLicenseById(int licenseId)
		{
			throw new NotImplementedException();
		}

		public bool IsProductIdUsed(int productId)
		{
			var licenses = from l in GetAllLicenses()
										 where l.Product.ProductId == productId
										 select l;

			if (licenses.Count() > 0)
				return true;

			return false;
		}
	}
}