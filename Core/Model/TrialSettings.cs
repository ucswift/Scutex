using System.Xml.Serialization;

namespace WaveTech.Scutex.Model
{
	[XmlInclude(typeof(LicenseTrialSettings))]
	public class TrialSettings : BaseObject
	{
		private TrialExpirationOptions _expirationOptions;
		private string _expirationData;

		public virtual TrialExpirationOptions ExpirationOptions
		{
			get
			{
				return _expirationOptions;
			}

			set
			{
				if (value != _expirationOptions)
				{
					_expirationOptions = value;
					OnPropertyChanged("ExpirationOptions");
				}
			}
		}

		public virtual string ExpirationData
		{
			get
			{
				return _expirationData;
			}

			set
			{
				if (value != _expirationData)
				{
					_expirationData = value;
					OnPropertyChanged("ExpirationData");
				}
			}
		}
	}
}