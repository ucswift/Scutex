using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Services;
using MessageBox = System.Windows.MessageBox;

namespace WaveTech.Scutex.Manager.Windows
{
	/// <summary>
	/// Interaction logic for NewServiceWindow.xaml
	/// </summary>
	public partial class NewServiceWindow : Window
	{
		private IServicesService servicesService;
		private bool _hasPackgageBeenDownloaded = false;
		private Service _service;

		public NewServiceWindow()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);

			servicesService = ObjectLocator.GetInstance<IServicesService>();
		}

		/// <summary>
		/// Constructor that takes a parent for this GenerationWindow window.
		/// </summary>
		/// <param name="parent">Parent window for this dialog.</param>
		public NewServiceWindow(Window parent)
			: this()
		{
			this.Owner = parent;
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

		private void ResetForm()
		{
			txtServiceName.Text = String.Empty;
			txtClientUrl.Text = String.Empty;
			txtManagementUrl.Text = String.Empty;

			_hasPackgageBeenDownloaded = false;
			btnDownloadService.Foreground = new SolidColorBrush(Colors.Red);
		}

		private void SetService()
		{
			if (_service == null)
			{
				IAsymmetricEncryptionProvider asymmetricEncryptionProvider =
					ObjectLocator.GetInstance<IAsymmetricEncryptionProvider>();

				_service = new Service();
				_service.OutboundKeyPair = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);
				_service.InboundKeyPair = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);
				_service.ManagementInboundKeyPair = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);
				_service.ManagementOutboundKeyPair = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);
				_service.Token = Guid.NewGuid().ToString();

				IStringDataGeneratorProvider stringDataGenerator = ObjectLocator.GetInstance<IStringDataGeneratorProvider>();
				_service.ClientRequestToken = stringDataGenerator.GenerateRandomString(10, 25, true, true);
				_service.ManagementRequestToken = stringDataGenerator.GenerateRandomString(10, 25, true, true);
			}
		}

		private void btnDownloadService_Click(object sender, RoutedEventArgs e)
		{
			_hasPackgageBeenDownloaded = true;
			btnDownloadService.Foreground = new SolidColorBrush(Colors.ForestGreen);

			SaveFileDialog dialog = new SaveFileDialog();
			dialog.DefaultExt = ".zip";
			dialog.Filter = "Zip Files (.zip)|*.zip";

			dialog.ShowDialog();
			if (!String.IsNullOrEmpty(dialog.FileName))
			{
				SetService();

				IWcfPackagingService wcfPackagingService = ObjectLocator.GetInstance<IWcfPackagingService>();
				wcfPackagingService.PackageService(dialog.FileName, _service);
			}
		}

		private void btnSaveService_Click(object sender, RoutedEventArgs e)
		{
			if (IsFormValid())
			{
				if (_hasPackgageBeenDownloaded)
				{
					SetService();

					_service.ClientUrl = txtClientUrl.Text;
					_service.ManagementUrl = txtManagementUrl.Text;
					_service.Name = txtServiceName.Text;
					_service.UniquePad = Guid.NewGuid();
					_service.Initialized = false;
					_service.CreatedDate = DateTime.Now;

					if (chkLockToIp.IsChecked.HasValue)
						_service.LockToIp = chkLockToIp.IsChecked.Value;

					servicesService.SaveService(_service);
					ResetForm();
				}
				else
				{
					MessageBox.Show("Please download the service package before saving.");
				}
			}
		}
	}
}