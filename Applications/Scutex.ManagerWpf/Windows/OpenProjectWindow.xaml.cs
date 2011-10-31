using System.Windows;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Manager.Windows
{
	/// <summary>
	/// Interaction logic for OpenProjectWindow.xaml
	/// </summary>
	public partial class OpenProjectWindow : Window
	{
		#region Constructors
		public OpenProjectWindow()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);
		}

		/// <summary>
		/// Constructor that takes a parent for this GenerationWindow window.
		/// </summary>
		/// <param name="parent">Parent window for this dialog.</param>
		public OpenProjectWindow(Window parent)
			: this()
		{
			this.Owner = parent;
		}
		#endregion Constructors

		private void OpenProject()
		{
			if (lstProjects.SelectedValue != null)
			{
				MainWindow mainWindow = Owner as MainWindow;

				UIContext.License = (License)lstProjects.SelectedValue;

				ILicenseService licenseService = ObjectLocator.GetInstance<ILicenseService>();
				UIContext.License = licenseService.GetLicenseById(((License)lstProjects.SelectedValue).LicenseId);

				mainWindow.Initalize();

				Close();
			}
			else
			{
				MessageBox.Show("You must select a project to open.");
			}
		}

		private void btnOpenProject_Click(object sender, RoutedEventArgs e)
		{
			OpenProject();
		}

		private void lstProjects_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			OpenProject();
		}
	}
}