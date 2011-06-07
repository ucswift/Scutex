using System.ComponentModel;
using System.Text;

namespace WaveTech.Scutex.Manager.Wizards
{
	public class FirstTimeWizardPresentationModel : INotifyPropertyChanged
	{
		#region Private Members
		private int _authenticationType;
		private string _serverName;
		private string _databaseName;
		private string _userName;
		private string _password;
		#endregion Private Members

		#region Public Properties
		public int AuthenticationType
		{
			get { return _authenticationType; }
			set
			{
				_authenticationType = value;
				OnPropertyChanged("AuthenticationType");
			}
		}

		public string ServerName
		{
			get { return _serverName; }
			set
			{
				_serverName = value;
				OnPropertyChanged("ServerName");
			}
		}

		public string DatabaseName
		{
			get { return _databaseName; }
			set
			{
				_databaseName = value;
				OnPropertyChanged("DatabaseName");
			}
		}

		public string UserName
		{
			get { return _userName; }
			set
			{
				_userName = value;
				OnPropertyChanged("UserName");
			}
		}

		public string Password
		{
			get { return _password; }
			set
			{
				_password = value;
				OnPropertyChanged("Password");
			}
		}
		#endregion Public Properties

		#region INotifyPropertyChanged Members
		protected virtual void OnPropertyChanged(string property)
		{
			if (PropertyChanged != null)
				PropertyChanged(this, new PropertyChangedEventArgs(property));
		}

		public event PropertyChangedEventHandler PropertyChanged;
		#endregion INotifyPropertyChanged Members

		#region Public Methods
		public string CreateConnectionString(string password)
		{
			StringBuilder connectionString = new StringBuilder();

			connectionString.Append(string.Format("Data Source={0};Initial Catalog={1};", ServerName, DatabaseName));

			if (AuthenticationType > 0)
				connectionString.Append(string.Format("Uid={0};Pwd={1};", UserName, password));
			else
				connectionString.Append("Integrated Security=SSPI;");

			return connectionString.ToString();
		}

		public string CreateConnectionStringForTest(string password)
		{
			StringBuilder connectionString = new StringBuilder();

			connectionString.Append(string.Format("Provider=SQLOLEDB;Data Source={0};Initial Catalog={1};", ServerName, DatabaseName));

			if (AuthenticationType > 0)
				connectionString.Append(string.Format("Uid={0};Pwd={1};", UserName, password));
			else
				connectionString.Append("Integrated Security=SSPI;");

			return connectionString.ToString();
		}

		public string CreateConnectionStringForSE2005(string serverName)
		{
			StringBuilder connectionString = new StringBuilder();

			connectionString.Append(@"Data Source=" + serverName + @";AttachDbFilename=|DataDirectory|\db\ScutexDb05.mdf;Integrated Security=True;User Instance=True;MultipleActiveResultSets=True;");

			return connectionString.ToString();
		}

		public string CreateConnectionStringForSE2008(string serverName)
		{
			StringBuilder connectionString = new StringBuilder();

			connectionString.Append(@"Data Source=" + serverName + @";AttachDbFilename=|DataDirectory|\db\ScutexDb08.mdf;Integrated Security=True;User Instance=True;MultipleActiveResultSets=True;");

			return connectionString.ToString();
		}

		public string CreateConnectionStringForSETest(string serverName)
		{
			StringBuilder connectionString = new StringBuilder();

			connectionString.Append(@"Data Source=" + serverName + @";Integrated Security=True;MultipleActiveResultSets=True;");

			return connectionString.ToString();
		}

		#endregion Public Methods
	}
}