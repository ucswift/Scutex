using System;

namespace WaveTech.Scutex.Model
{
	public class License: LicenseBase
	{
		#region Private Members
		private int _licenseId;
		private string _name;
		private string _buyNowUrl;
		private string _productUrl;
		private string _eulaUrl;
		private string _supportEmail;
		private string _salesEmail;
		private DateTime _createdOn;
		private DateTime? _updatedOn;
		private string _salesPhone;
		private LicenseTrialSettings _licenseTrialSettings;
		private Service _service;
		#endregion Private Members

		#region Constructor
		public License()
		{
			TrialSettings = new LicenseTrialSettings();
		}
		#endregion Constructor

		#region Private Event Handlers
		private void _licenseTrialSettings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.TrialSettings = _licenseTrialSettings;
		}
		#endregion Private Event Handlers

		#region Public Properties
		public int LicenseId
		{
			get
			{
				return _licenseId;
			}

			set
			{
				if (value != _licenseId)
				{
					_licenseId = value;
					OnPropertyChanged("LicenseId");
				}
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}

			set
			{
				if (value != _name)
				{
					_name = value;
					OnPropertyChanged("Name");
				}
			}
		}

		public string BuyNowUrl
		{
			get
			{
				return _buyNowUrl;
			}

			set
			{
				if (value != _buyNowUrl)
				{
					_buyNowUrl = value;
					OnPropertyChanged("BuyNowUrl");
				}
			}
		}

		public string ProductUrl
		{
			get
			{
				return _productUrl;
			}

			set
			{
				if (value != _productUrl)
				{
					_productUrl = value;
					OnPropertyChanged("ProductUrl");
				}
			}
		}

		public string EulaUrl
		{
			get
			{
				return _eulaUrl;
			}

			set
			{
				if (value != _eulaUrl)
				{
					_eulaUrl = value;
					OnPropertyChanged("EulaUrl");
				}
			}
		}

		public string SupportEmail
		{
			get
			{
				return _supportEmail;
			}

			set
			{
				if (value != _supportEmail)
				{
					_supportEmail = value;
					OnPropertyChanged("SupportEmail");
				}
			}
		}

		public string SalesEmail
		{
			get
			{
				return _salesEmail;
			}

			set
			{
				if (value != _salesEmail)
				{
					_salesEmail = value;
					OnPropertyChanged("SalesEmail");
				}
			}
		}

		public DateTime CreatedOn
		{
			get
			{
				return _createdOn;
			}

			set
			{
				if (value != _createdOn)
				{
					_createdOn = value;
					OnPropertyChanged("CreatedOn");
				}
			}
		}

		public DateTime? UpdatedOn
		{
			get
			{
				return _updatedOn;
			}

			set
			{
				if (value != _updatedOn)
				{
					_updatedOn = value;
					OnPropertyChanged("UpdatedOn");
				}
			}
		}

		public string SalesPhone
		{
			get
			{
				return _salesPhone;
			}

			set
			{
				if (value != _salesPhone)
				{
					_salesPhone = value;
					OnPropertyChanged("SalesPhone");
				}
			}
		}

		public new LicenseTrialSettings TrialSettings
		{
			get
			{
				return _licenseTrialSettings;
			}

			set
			{
				if (value != _licenseTrialSettings)
				{
					_licenseTrialSettings = value;

					if (_licenseTrialSettings != null)
						_licenseTrialSettings.PropertyChanged += _licenseTrialSettings_PropertyChanged;

					base.TrialSettings = value;

					OnPropertyChanged("TrialSettings");
				}
			}
		}

		public Service Service
		{
			get
			{
				return _service;
			}

			set
			{
				if (value != _service)
				{
					_service = value;
					OnPropertyChanged("Service");
				}
			}
		}
		#endregion Public Properties
	}
}