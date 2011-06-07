using System;
using System.Windows;

namespace Scutex.ManagerWpf
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{

		private void Application_Startup(object sender, StartupEventArgs e)
		{

		}

		private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			if (e.Exception.Message.Contains("The underlying provider failed on Open."))
			{
				MessageBox.Show("There was a problem talking to the database. Please check your database and your connection settings and try again.");
				Environment.Exit(101);
			}

		}
	}
}