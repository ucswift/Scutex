using System;
using System.Configuration;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using Infragistics.Windows.Ribbon;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Providers.DatabaseUpdateProvider;

namespace WaveTech.Scutex.Manager.Wizards
{
	/// <summary>
	/// Interaction logic for FristTimeWizard.xaml
	/// </summary>
	public partial class FirstTimeWizard : XamRibbonWindow
	{
		#region Externals/Constants
		[DllImport("user32.dll", EntryPoint = "GetSystemMenu")]
		private static extern IntPtr GetSystemMenu(IntPtr hwnd, int revert);

		[DllImport("user32.dll", EntryPoint = "GetMenuItemCount")]
		private static extern int GetMenuItemCount(IntPtr hmenu);

		[DllImport("user32.dll", EntryPoint = "RemoveMenu")]
		private static extern int RemoveMenu(IntPtr hmenu, int npos, int wflags);

		[DllImport("user32.dll", EntryPoint = "DrawMenuBar")]
		private static extern int DrawMenuBar(IntPtr hwnd);

		private const int MF_BYPOSITION = 0x0400;
		private const int MF_DISABLED = 0x0002;
		#endregion Externals/Constants

		#region Private Members
		private MainWindow _mainWindow;
		private bool _isWizardFinished;
		private FirstTimeWizardPresentationModel model;
		private Timer _dbTestTimer;
		#endregion Private Members

		#region Constructors
		public FirstTimeWizard()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);

			_isWizardFinished = false;
			model = new FirstTimeWizardPresentationModel();
			DataContext = model;

			txtServerName.Focus();
			rdoSQLServer.IsChecked = true;
		}

		public FirstTimeWizard(MainWindow mainWindow)
			: this()
		{
			_mainWindow = mainWindow;
		}
		#endregion Constructors

		#region Private Event Handlers
		private void Wizard_OnFinishClick(object sender, RoutedEventArgs e)
		{
			ConfigFileHelper configFile = new ConfigFileHelper();
			DatabaseUpdater databaseUpdateProvider = new DatabaseUpdater();

			DatabaseFileHelper.ResetDatabaseReadOnlyFlag();

			if (rdoSQLServer.IsChecked.HasValue && rdoSQLServer.IsChecked.Value)
				configFile.SaveConfigFile(model.CreateConnectionString(txtPassword.Password));
			else
			{
				if (databaseUpdateProvider.IsDatabaseVersionCorrect(GetConnectionString(true)))
					configFile.SaveConfigFileForSqlExpress2008(txtServerName.Text);
				else
					configFile.SaveConfigFileForSqlExpress2005(txtServerName.Text);
			}

			_isWizardFinished = true;
			_mainWindow.Visibility = Visibility.Visible;

			ConfigurationManager.RefreshSection("connectionStrings");
			Bootstrapper.Configure();

			databaseUpdateProvider.InitializeDatabase(GetConnectionString(false));

			_mainWindow.RefreshData();
			_mainWindow.SetRecentItems();

			Close();
		}

		private void Wizard_OnCancelClick(object sender, RoutedEventArgs e)
		{
			Environment.Exit(0);
		}

		private void cboAuthenticationType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (cboAuthenticationType.SelectedIndex == 0)
			{
				DisableFormForSqlAuth();
			}
			else
			{
				EnableFormForSqlAuth();
			}

			CheckDbDataForm();
		}

		private void XamRibbonWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			if (!_isWizardFinished)
				Environment.Exit(0);
		}

		private void wizardPageTestDb_PageShow(object sender, RoutedEventArgs e)
		{
			if (rdoSQLServer.IsChecked.HasValue && rdoSQLServer.IsChecked.Value)
			{
				_dbTestTimer = new Timer();
				_dbTestTimer.Elapsed += new ElapsedEventHandler(_dbTestTimer_Elapsed);
				_dbTestTimer.Interval = 1000;

				_dbTestTimer.Start();
			}
			else
			{
				wizardPageTestDb.CanNext = true;
				pdgTestDatabaseProgress.IsIndeterminate = false;
				lblDbTestText.Content = "Database test passed, press Next to continue.";
			}
		}

		private void _dbTestTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			_dbTestTimer.Stop();

			if (TestConnectionString())
			{
				wizardPageTestDb.Dispatcher.Invoke(
				System.Windows.Threading.DispatcherPriority.Normal,
				new Action(
					delegate()
					{
						wizardPageTestDb.CanNext = true;
					}));

				pdgTestDatabaseProgress.Dispatcher.Invoke(
				System.Windows.Threading.DispatcherPriority.Normal,
				new Action(
					delegate()
					{
						pdgTestDatabaseProgress.IsIndeterminate = false;
					}));

				lblDbTestText.Dispatcher.Invoke(
					System.Windows.Threading.DispatcherPriority.Normal,
					new Action(
						delegate()
						{
							lblDbTestText.Content = "Database test passed, press Next to continue.";
						}));
			}
			else
			{
				lblDbTestText.Dispatcher.Invoke(
					System.Windows.Threading.DispatcherPriority.Normal,
					new Action(
						delegate()
						{
							lblDbTestText.Content = "Database test failed, press Previous and try again.";
						}));

				pdgTestDatabaseProgress.Dispatcher.Invoke(
					System.Windows.Threading.DispatcherPriority.Normal,
					new Action(
						delegate()
						{
							pdgTestDatabaseProgress.IsIndeterminate = false;
						}));
			}
		}

		public void ShowDbTestFailDialog()
		{
			MessageBox.Show("Database test failed. Please press back, check the parameters and try again.");
		}

		private void txtServerName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			CheckDbDataForm();
		}

		private void txtDatabaseName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			CheckDbDataForm();
		}

		private void txtUserName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
		{
			CheckDbDataForm();
		}

		private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
		{
			CheckDbDataForm();
		}

		private void wizardPageTestDb_PageClose(object sender, RoutedEventArgs e)
		{
			lblDbTestText.Content = "Database test in progress, please wait...";
			pdgTestDatabaseProgress.IsIndeterminate = true;
		}

		private void rdoSQLServer_Checked(object sender, RoutedEventArgs e)
		{
			SwtichForm();
		}

		private void rdoSQLExpress_Checked(object sender, RoutedEventArgs e)
		{
			SwtichForm();
		}

		private void wizardPageDbValidation_PageShow(object sender, RoutedEventArgs e)
		{
			lstValidationIssues.Items.Add("Below are possible issues with your system:");
			lstValidationIssues.Items.Add("------------------------------------------------------------------------------------------------------");

			DatabaseUpdater databaseUpdateProvider = new DatabaseUpdater();

			bool isSql2008r2 = databaseUpdateProvider.IsDatabaseVersionCorrect(GetConnectionString(true));

			//if (rdoSQLServer.IsChecked.HasValue && rdoSQLServer.IsChecked.Value)
			//{
			//  if (isSql2008r2 == false)
			//  {
			//    lstValidationIssues.Items.Add(
			//      "You are not using SQL 2008, ensure that you have the DTC (Distributed Transaction Coordinator) enabled for network access. See the Scutex Documentation for more information.");
			//  }
			//}
			//else
			//{
			//  if (isSql2008r2 == false)
			//  {
			//    lstValidationIssues.Items.Add(
			//      "You are not using SQL Express 2008 R2, it's recommended that you have the latest version. See the Scutex Documentation for more information.");
			//  }
			//}

			if (lstValidationIssues.Items.Count == 2)
				lstValidationIssues.Items.Add("No issues detected.");

			pdgValidateDatabaseProgress.IsIndeterminate = false;
			lblDbValidateText.Content = "System validation complete, please review issues below.";
			wizardPageDbValidation.CanNext = true;
		}

		private void wizardPageDbValidation_PageClose(object sender, RoutedEventArgs e)
		{
			lstValidationIssues.Items.Clear();
			pdgValidateDatabaseProgress.IsIndeterminate = true;
			lblDbValidateText.Content = "Please wait while the system is validated...";
			wizardPageDbValidation.CanNext = false;
		}
		#endregion Private Event Handlers

		#region Private Methods
		private void EnableFormForSqlAuth()
		{
			try
			{
				txtUserName.IsEnabled = true;
				txtUserName.Background = Brushes.White;

				txtPassword.IsEnabled = true;
				txtPassword.Background = Brushes.White;
			}
			catch
			{

			}
		}

		private void DisableFormForSqlAuth()
		{
			try
			{
				txtUserName.Text = String.Empty;
				txtUserName.IsEnabled = false;
				txtUserName.Background = Brushes.Gray;

				txtPassword.Password = String.Empty;
				txtPassword.IsEnabled = false;
				txtPassword.Background = Brushes.Gray;
			}
			catch
			{

			}
		}

		private bool TestConnectionString()
		{
			bool connectionOk;
			OleDbConnection con = new OleDbConnection(model.CreateConnectionStringForTest(txtPassword.Password));
			OleDbCommand command = con.CreateCommand();

			command.CommandText = string.Format("USE {0}", model.DatabaseName);

			try
			{
				con.Open();
				command.ExecuteScalar();

				connectionOk = true;
			}
			catch
			{
				connectionOk = false;
			}
			finally
			{
				con.Close();
			}

			return connectionOk;
		}

		private void CheckDbDataForm()
		{
			bool enableNext = true;

			if (rdoSQLServer.IsChecked.HasValue && rdoSQLServer.IsChecked.Value)
			{
				if (String.IsNullOrEmpty(txtServerName.Text))
					enableNext = false;

				if (String.IsNullOrEmpty(txtDatabaseName.Text))
					enableNext = false;

				if (cboAuthenticationType.SelectedIndex == 1)
				{
					if (String.IsNullOrEmpty(txtUserName.Text))
						enableNext = false;

					if (String.IsNullOrEmpty(txtPassword.Password))
						enableNext = false;
				}
			}

			wizardPageDbInfo.CanNext = enableNext;
		}

		private void DisableForm()
		{
			txtDatabaseName.Text = String.Empty;
			txtDatabaseName.IsEnabled = false;
			txtDatabaseName.Background = Brushes.Gray;

			cboAuthenticationType.IsEnabled = false;

			DisableFormForSqlAuth();
		}

		private void EnableForm()
		{
			txtDatabaseName.Text = String.Empty;
			txtDatabaseName.IsEnabled = true;
			txtDatabaseName.Background = Brushes.White;

			cboAuthenticationType.IsEnabled = true;

			if (cboAuthenticationType.SelectedIndex == 0)
			{
				DisableFormForSqlAuth();
			}
			else
			{
				EnableFormForSqlAuth();
			}
		}

		private string GetConnectionString(bool isTest)
		{
			if (rdoSQLServer.IsChecked.HasValue && rdoSQLServer.IsChecked.Value)
				return model.CreateConnectionString(txtPassword.Password);
			else
				if (isTest)
					return model.CreateConnectionStringForSETest(txtServerName.Text);
				else
				{
					DatabaseUpdater databaseUpdateProvider = new DatabaseUpdater();

					if (databaseUpdateProvider.IsDatabaseVersionCorrect(model.CreateConnectionStringForSETest(txtServerName.Text)))
						return model.CreateConnectionStringForSE2008(txtServerName.Text);
					else
						return model.CreateConnectionStringForSE2005(txtServerName.Text);
				}
		}

		private void SwtichForm()
		{
			if (rdoSQLServer.IsChecked.HasValue && rdoSQLServer.IsChecked.Value)
			{
				EnableForm();
				CheckDbDataForm();
			}
			else
			{
				txtServerName.Text = @".\SQLEXPRESS";
				model.ServerName = @".\SQLEXPRESS";
				DisableForm();
				DisableFormForSqlAuth();
			}

			CheckDbDataForm();
		}
		#endregion Private Methods
	}
}