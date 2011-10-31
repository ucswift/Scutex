using System;
using System.Collections.Generic;
using System.ComponentModel;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Services;
using License = WaveTech.Scutex.Model.License;

namespace WaveTech.Scutex.Manager.Classes
{
	public class UIContext
	{
		#region Private Members
		private static bool _newLicense;
		private static License _license;
		#endregion Private Members

		#region Public Properties
		public static License License
		{
			get { return _license; }
			set
			{
				if (value != null || value != _license)
				{
					_license = value;
				}
			}
		}
		#endregion Public Properties

		public static License GetLicense()
		{
			return _license;
		}

		public static BindingList<Product> GetProducts()
		{
			try
			{
				IProductsService productsService = ObjectLocator.GetInstance<IProductsService>();
				return new BindingList<Product>(productsService.GetAllProducts());
			}
			catch
			{
				return null;
			}
		}

		public static BindingList<Service> GetServices()
		{
			try
			{
				IServicesService servicesService = ObjectLocator.GetInstance<IServicesService>();
				return new BindingList<Service>(servicesService.GetAllServices());
			}
			catch
			{
				return null;
			}
		}

		public static BindingList<LicenseSet> GetLicenseSets()
		{
			if (_license != null && _license.LicenseSets != null)
				return new BindingList<LicenseSet>(_license.LicenseSets);

			return new BindingList<LicenseSet>();
		}

		public static BindingList<License> GetLatestLicenses()
		{
			try
			{
				ILicenseService licenseService = ObjectLocator.GetInstance<ILicenseService>();
				return new BindingList<License>(licenseService.GetLast10Licenses());
			}
			catch
			{
				return null;
			}
		}

		public static BindingList<License> GetAllLicenses()
		{
			try
			{
				ILicenseService licenseService = ObjectLocator.GetInstance<ILicenseService>();
				return new BindingList<License>(licenseService.GetAllLicenses());
			}
			catch
			{
				return null;
			}
		}

		public static BindingList<Service> GetAllServices()
		{
			try
			{
				IServicesService servicesService = ObjectLocator.GetInstance<IServicesService>();
				return new BindingList<Service>(servicesService.GetAllServices());
			}
			catch
			{
				return null;
			}
		}

		public static BindingList<Service> GetAllActiveServices()
		{
			try
			{
				IServicesService servicesService = ObjectLocator.GetInstance<IServicesService>();
				return new BindingList<Service>(servicesService.GetAllInitializedActiveServices());
			}
			catch
			{
				return null;
			}
		}

		public static Dictionary<License, List<LicenseSet>> GetAllLicensesAndSets()
		{
			try
			{
				ILicenseService licenseService = ObjectLocator.GetInstance<ILicenseService>();
				return licenseService.GetAllLicensesAndSets();
			}
			catch
			{
				return null;
			}
		}

		public static BindingList<UploadProductDisplayData> GetAllProcutsLicensesAndSets()
		{
			try
			{
				ILicenseService licenseService = ObjectLocator.GetInstance<ILicenseService>();
				IProductsService productsService = ObjectLocator.GetInstance<IProductsService>();

				Dictionary<License, List<LicenseSet>> data = licenseService.GetAllLicensesAndSets();

				return DataConverters.ConvertAllLicensesSetsToDisplay(data);
			}
			catch
			{
				return null;
			}
		}

		public static void SetNewLicense()
		{
			_license = new License();
			_newLicense = true;
		}

		public static void InitializeForNewLicense()
		{
			if (_newLicense)
			{
				_license.UniqueId = Guid.NewGuid();
				IAsymmetricEncryptionProvider asymmetricEncryptionProvider =
					ObjectLocator.GetInstance<IAsymmetricEncryptionProvider>();

				_license.KeyPair = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);

				_newLicense = false;
			}
		}
	}
}
