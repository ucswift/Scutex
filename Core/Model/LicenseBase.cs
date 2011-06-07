using System;

namespace WaveTech.Scutex.Model
{
	/// <summary>
	/// The LicenseBase base class defines the required set of data for vital 
	/// operations against the license objects, for example key generation
	/// and validation.
	/// </summary>
	public abstract class LicenseBase : BaseObject
	{
		#region Private Members
		private Guid _uniqueId;
		private NotifyList<LicenseSet> _licenseSets;
		private Product _product;
		private KeyPair _keyPair;
		private TrialNotificationTypes _trialNotificationType;
		private TrailNotificationSettings _trailNotificationSettings;
		private TrialSettings _trialSettings;
		private KeyGeneratorTypes _keyGeneratorType;
		#endregion Private Members

		#region Constructor
		protected LicenseBase()
		{
			_trailNotificationSettings = new TrailNotificationSettings();
			_licenseSets = new NotifyList<LicenseSet>();
		}
		#endregion Constructor

		#region Public Properties
		public virtual Guid UniqueId
		{
			get
			{
				return _uniqueId;
			}

			set
			{
				if (value != _uniqueId)
				{
					_uniqueId = value;
					OnPropertyChanged("UniqueId");
				}
			}
		}

		public virtual Product Product
		{
			get
			{
				return _product;
			}

			set
			{
				if (value != _product)
				{
					_product = value;
					OnPropertyChanged("Product");
				}
			}
		}

		public virtual KeyPair KeyPair
		{
			get
			{
				return _keyPair;
			}

			set
			{
				if (value != _keyPair)
				{
					_keyPair = value;
					OnPropertyChanged("KeyPair");
				}
			}
		}

		public virtual TrailNotificationSettings TrailNotificationSettings
		{
			get
			{
				return _trailNotificationSettings;
			}

			set
			{
				if (value != _trailNotificationSettings)
				{
					_trailNotificationSettings = value;
					OnPropertyChanged("TrailNotificationSettings");
				}
			}
		}

		public virtual NotifyList<LicenseSet> LicenseSets
		{
			get
			{
				return _licenseSets;
			}

			set
			{
				if (value != _licenseSets)
				{
					_licenseSets = value;
					OnPropertyChanged("LicenseSets");
				}
			}
		}

		public virtual TrialNotificationTypes TrialNotificationType
		{
			get
			{
				return _trialNotificationType;
			}

			set
			{
				if (value != _trialNotificationType)
				{
					_trialNotificationType = value;
					OnPropertyChanged("TrialNotificationType");
				}
			}
		}

		public virtual TrialSettings TrialSettings
		{
			get
			{
				return _trialSettings;
			}

			set
			{
				if (value != _trialSettings)
				{
					_trialSettings = value;
					OnPropertyChanged("TrialSettings");
				}
			}
		}

		public virtual KeyGeneratorTypes KeyGeneratorType
		{
			get
			{
				return _keyGeneratorType;
			}

			set
			{
				if (value != _keyGeneratorType)
				{
					_keyGeneratorType = value;
					OnPropertyChanged("KeyGeneratorType");
				}
			}
		}
		#endregion Public Properties

		#region Public Methods
		public virtual string GetLicenseProductIdentifier()
		{
			return UniqueId + Product.UniquePad;
		}
		#endregion Public Methods
	}
}