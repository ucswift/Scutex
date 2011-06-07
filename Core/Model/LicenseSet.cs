using System;

namespace WaveTech.Scutex.Model
{
	public class LicenseSet: BaseObject
	{
		#region Private Members
		private int _licenseSetId;
		private int _licenseId;
		private string _name;
		private LicenseKeyTypeFlag _supportedLicenseTypes;
		private Guid _uniquePad;
		private NotifyList<Feature> _features;
		private int? _maxUsers;
		#endregion Private Members

		#region Constructor
		public LicenseSet()
		{
			_uniquePad = Guid.NewGuid();
			_features = new NotifyList<Feature>();
		}
		#endregion Constructor

		#region Public Properties
		public virtual int LicenseSetId
		{
			get
			{
				return _licenseSetId;
			}

			set
			{
				if (value != _licenseSetId)
				{
					_licenseSetId = value;
					OnPropertyChanged("LicenseSetId");
				}
			}
		}

		public virtual int LicenseId
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

		public virtual string Name
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

		public virtual LicenseKeyTypeFlag SupportedLicenseTypes
		{
			get
			{
				return _supportedLicenseTypes;
			}

			set
			{
				if (value != _supportedLicenseTypes)
				{
					_supportedLicenseTypes = value;
					OnPropertyChanged("SupportedLicenseTypes");
				}
			}
		}

		public virtual Guid UniquePad
		{
			get
			{
				return _uniquePad;
			}

			set
			{
				if (value != _uniquePad)
				{
					_uniquePad = value;
					OnPropertyChanged("UniquePad");
				}
			}
		}

		public virtual NotifyList<Feature> Features
		{
			get
			{
				return _features;
			}

			set
			{
				if (value != _features)
				{
					_features = value;
					OnPropertyChanged("Features");
				}
			}
		}

		public virtual int? MaxUsers
		{
			get
			{
				return _maxUsers;
			}

			set
			{
				if (value != _maxUsers)
				{
					_maxUsers = value;
					OnPropertyChanged("MaxUsers");
				}
			}
		}
		#endregion Public Properties
	}
}