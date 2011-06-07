using System;
using System.Windows;
using System.Windows.Media;
using Infragistics.Windows.Ribbon;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Manager.Windows
{
	/// <summary>
	/// Interaction logic for UpdateServiceWindow.xaml
	/// </summary>
	public partial class UpdateServiceWindow : XamRibbonWindow
	{
		public static readonly DependencyProperty ServiceProperty =
		DependencyProperty.Register("Service", typeof(Service), typeof(UpdateServiceWindow),
		new FrameworkPropertyMetadata(
				null,
				FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
				LicensePropertyChanged));

		public UpdateServiceWindow()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);
		}

		/// <summary>
		/// Constructor that takes a parent for this GenerationWindow window.
		/// </summary>
		/// <param name="parent">Parent window for this dialog.</param>
		public UpdateServiceWindow(Window parent, Service service)
			: this()
		{
			this.Owner = parent;
			Service = service;
		}

		/// <summary>
		/// Constructor that takes a parent for this GenerationWindow window.
		/// </summary>
		/// <param name="parent">Parent window for this dialog.</param>
		public UpdateServiceWindow(Window parent)
			: this()
		{
			this.Owner = parent;
		}

		public Service Service
		{
			get { return (Service)GetValue(ServiceProperty); }
			set { SetValue(ServiceProperty, value); }
		}

		private static void LicensePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

		}

		private bool IsFormValid()
		{
			bool isValid = true;

			if (String.IsNullOrEmpty(txtServiceName.Text))
			{
				txtServiceName.Background = new SolidColorBrush(Colors.IndianRed);
				isValid = false;
			}
			else
			{
				txtServiceName.Background = new SolidColorBrush(Colors.White);
			}

			if (String.IsNullOrEmpty(txtClientUrl.Text))
			{
				txtClientUrl.Background = new SolidColorBrush(Colors.IndianRed);
				isValid = false;
			}
			else
			{
				if (StringHelpers.IsValidUrl(txtClientUrl.Text) == false || txtClientUrl.Text.EndsWith("/") == false)
				{
					MessageBox.Show("Client Service Url is invalid, make sure it's a valid url with http[s]:// prefix and with trailing /");
					txtClientUrl.Background = new SolidColorBrush(Colors.IndianRed);
					isValid = false;
				}
				else
				{
					txtClientUrl.Background = new SolidColorBrush(Colors.White);
				}
			}

			if (String.IsNullOrEmpty(txtManagementUrl.Text))
			{
				txtManagementUrl.Background = new SolidColorBrush(Colors.IndianRed);
				isValid = false;
			}
			else
			{
				if (StringHelpers.IsValidUrl(txtManagementUrl.Text) == false || txtManagementUrl.Text.EndsWith("/") == false)
				{
					MessageBox.Show("Management Service Url is invalid, make sure it's a valid url with http[s]:// prefix and with trailing /");
					txtManagementUrl.Background = new SolidColorBrush(Colors.IndianRed);
					isValid = false;
				}
				else
				{
					txtManagementUrl.Background = new SolidColorBrush(Colors.White);
				}
			}

			if (!isValid)
				MessageBox.Show("Please correct the errors and try again");

			return isValid;
		}

		private void btnDeleteService_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show(string.Format("Are you sure you want to delete service {0}", Service.Name), "Delete Service?", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
			{
				IServicesService servicesService = ObjectLocator.GetInstance<IServicesService>();
				servicesService.DeleteServiceById(Service.ServiceId);

				this.Close();
			}
		}

		private void btnSaveService_Click(object sender, RoutedEventArgs e)
		{
			if (IsFormValid())
			{
				IServicesService servicesService = ObjectLocator.GetInstance<IServicesService>();
				servicesService.SaveService(Service);

				this.Close();
			}
		}
	}
}