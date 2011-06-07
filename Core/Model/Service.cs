using System;

namespace WaveTech.Scutex.Model
{
	public class Service : BaseObject
	{
		#region Private Members
		private int _serviceId;
		private string _name;
		private string _clientUrl;
		private string _managementUrl;
		private string _token;
		private KeyPair _inboundKeyPair;
		private KeyPair _outboundKeyPair;
		private KeyPair _managementInboundKeyPair;
		private KeyPair _managementOutboundKeyPair;
		private Guid _uniquePad;
		private bool _initialized;
		private bool _tested;
		private bool _lockToIp;
		private string _clientRequestToken;
		private string _managementRequestToken;
		private DateTime _createdDate;
		#endregion Private Members

		#region Public Properties
		public virtual int ServiceId
		{
			get
			{
				return _serviceId;
			}

			set
			{
				if (value != _serviceId)
				{
					_serviceId = value;
					OnPropertyChanged("ServiceId");
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

		public virtual string ClientUrl
		{
			get
			{
				return _clientUrl;
			}

			set
			{
				if (value != _clientUrl)
				{
					_clientUrl = value;
					OnPropertyChanged("ClientUrl");
				}
			}
		}

		public virtual string ManagementUrl
		{
			get
			{
				return _managementUrl;
			}

			set
			{
				if (value != _managementUrl)
				{
					_managementUrl = value;
					OnPropertyChanged("ManagementUrl");
				}
			}
		}

		public virtual string Token
		{
			get
			{
				return _token;
			}

			set
			{
				if (value != _token)
				{
					_token = value;
					OnPropertyChanged("Token");
				}
			}
		}

		public virtual KeyPair InboundKeyPair
		{
			get
			{
				return _inboundKeyPair;
			}

			set
			{
				if (value != _inboundKeyPair)
				{
					_inboundKeyPair = value;
					OnPropertyChanged("InboundKeyPair");
				}
			}
		}

		public virtual KeyPair OutboundKeyPair
		{
			get
			{
				return _outboundKeyPair;
			}

			set
			{
				if (value != _outboundKeyPair)
				{
					_outboundKeyPair = value;
					OnPropertyChanged("OutboundKeyPair");
				}
			}
		}

		public virtual KeyPair ManagementInboundKeyPair
		{
			get
			{
				return _managementInboundKeyPair;
			}

			set
			{
				if (value != _managementInboundKeyPair)
				{
					_managementInboundKeyPair = value;
					OnPropertyChanged("ManagementInboundKeyPair");
				}
			}
		}

		public virtual KeyPair ManagementOutboundKeyPair
		{
			get
			{
				return _managementOutboundKeyPair;
			}

			set
			{
				if (value != _managementOutboundKeyPair)
				{
					_managementOutboundKeyPair = value;
					OnPropertyChanged("ManagementOutboundKeyPair");
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

		public virtual bool Initialized
		{
			get
			{
				return _initialized;
			}

			set
			{
				if (value != _initialized)
				{
					_initialized = value;
					OnPropertyChanged("Initialized");
				}
			}
		}

		public virtual bool Tested
		{
			get
			{
				return _tested;
			}

			set
			{
				if (value != _tested)
				{
					_tested = value;
					OnPropertyChanged("Tested");
				}
			}
		}

		public virtual bool LockToIp
		{
			get
			{
				return _lockToIp;
			}

			set
			{
				if (value != _lockToIp)
				{
					_lockToIp = value;
					OnPropertyChanged("LockToIp");
				}
			}
		}

		public virtual string ClientRequestToken
		{
			get
			{
				return _clientRequestToken;
			}

			set
			{
				if (value != _clientRequestToken)
				{
					_clientRequestToken = value;
					OnPropertyChanged("ClientRequestToken");
				}
			}
		}

		public virtual string ManagementRequestToken
		{
			get
			{
				return _managementRequestToken;
			}

			set
			{
				if (value != _managementRequestToken)
				{
					_managementRequestToken = value;
					OnPropertyChanged("ManagementRequestToken");
				}
			}
		}

		public virtual DateTime CreatedDate
		{
			get
			{
				return _createdDate;
			}

			set
			{
				if (value != _createdDate)
				{
					_createdDate = value;
					OnPropertyChanged("CreatedDate");
				}
			}
		}
		#endregion Public Properties

		#region Client Keys
		public string GetClientInboundKeyPart1()
		{
			string inKey1 = _inboundKeyPair.PublicKey.Substring(0, (_inboundKeyPair.PublicKey.Length / 2));

			return inKey1;
		}

		public string GetClientInboundKeyPart2()
		{
			string inKey2 = _inboundKeyPair.PublicKey.Substring(GetClientInboundKeyPart1().Length, (_inboundKeyPair.PublicKey.Length - GetClientInboundKeyPart1().Length));

			return inKey2;
		}

		public string GetClientOutboundKeyPart1()
		{
			string outKey1 = _outboundKeyPair.PrivateKey.Substring(0, (_outboundKeyPair.PrivateKey.Length / 2));

			return outKey1;
		}

		public string GetClientOutboundKeyPart2()
		{
			string outKey2 = _outboundKeyPair.PrivateKey.Substring(GetClientOutboundKeyPart1().Length, (_outboundKeyPair.PrivateKey.Length - GetClientOutboundKeyPart1().Length));

			return outKey2;
		}
		#endregion Client Keys

		#region Management Keys
		public string GetManagementInboundKeyPart1()
		{
			string inKey1 = _managementInboundKeyPair.PublicKey.Substring(0, (_managementInboundKeyPair.PublicKey.Length / 2));

			return inKey1;
		}

		public string GetManagementInboundKeyPart2()
		{
			string inKey2 = _managementInboundKeyPair.PublicKey.Substring(GetManagementInboundKeyPart1().Length, (_managementInboundKeyPair.PublicKey.Length - GetManagementInboundKeyPart1().Length));

			return inKey2;
		}


		public string GetManagementOutboundKeyPart1()
		{
			string outKey1 = _managementOutboundKeyPair.PrivateKey.Substring(0, (_managementOutboundKeyPair.PrivateKey.Length / 2));

			return outKey1;
		}

		public string GetManagementOutboundKeyPart2()
		{
			string outKey2 = _managementOutboundKeyPair.PrivateKey.Substring(GetManagementOutboundKeyPart1().Length, (_managementOutboundKeyPair.PrivateKey.Length - GetManagementOutboundKeyPart1().Length));

			return outKey2;
		}
		#endregion Management Keys

		public Token GetManagementToken()
		{
			Token t = new Token();
			t.Data = _managementRequestToken;
			t.Timestamp = DateTime.Now;

			return t;
		}

		public Token GetClientToken()
		{
			Token t = new Token();
			t.Data = _clientRequestToken;
			t.Timestamp = DateTime.Now;

			return t;
		}

		public KeyPair GetClientServiceKeyPair()
		{
			KeyPair kp = new KeyPair();
			kp.PrivateKey = _inboundKeyPair.PrivateKey;
			kp.PublicKey = _outboundKeyPair.PublicKey;

			return kp;
		}

		public KeyPair GetManagementServiceKeyPair()
		{
			KeyPair kp = new KeyPair();
			kp.PrivateKey = _managementInboundKeyPair.PrivateKey;
			kp.PublicKey = _managementOutboundKeyPair.PublicKey;

			return kp;
		}

		#region Equality Members
		public bool Equals(Service other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return other._serviceId == _serviceId && Equals(other._name, _name) && Equals(other._clientUrl, _clientUrl) && Equals(other._managementUrl, _managementUrl) && Equals(other._token, _token) && other._uniquePad.Equals(_uniquePad);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof(Service)) return false;
			return Equals((Service)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = _serviceId;
				result = (result * 397) ^ (_name != null ? _name.GetHashCode() : 0);
				result = (result * 397) ^ (_clientUrl != null ? _clientUrl.GetHashCode() : 0);
				result = (result * 397) ^ (_managementUrl != null ? _managementUrl.GetHashCode() : 0);
				result = (result * 397) ^ (_token != null ? _token.GetHashCode() : 0);
				result = (result * 397) ^ _uniquePad.GetHashCode();
				return result;
			}
		}

		public static bool operator ==(Service left, Service right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Service left, Service right)
		{
			return !Equals(left, right);
		}
		#endregion Equality Members
	}
}