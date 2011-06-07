
namespace WaveTech.Scutex.Model
{
	public class KeyPair: BaseObject
	{
		#region Private Members
		private string _privateKey;
		private string _publicKey;
		#endregion Private Members

		#region Public Properties
		public virtual string PrivateKey 
		{
			get
			{
				return _privateKey;
			}

			set
			{
				if (value != _privateKey)
				{
					_privateKey = value;
					OnPropertyChanged("PrivateKey");
				}
			}
		}

		public virtual string PublicKey
		{
			get
			{
				return _publicKey;
			}

			set
			{
				if (value != _publicKey)
				{
					_publicKey = value;
					OnPropertyChanged("PublicKey");
				}
			}
		}
		#endregion Public Properties
	}
}