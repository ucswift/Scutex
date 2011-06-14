using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Exceptions;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Licensing.Gui
{
	/// <summary>
	/// Interaction logic for RegisterContent.xaml
	/// </summary>
	public partial class RegisterContent : UserControl
	{
		private ILicenseKeyService _licenseKeyService;
		private IRegisterService _registerService;
		private ClientLicense _clientLicense;
		private ScutexLicense _scutexLicense;
		private Brush _originalBorderBrush;
		private Thickness _originalThickness;
		private Brush _originalResultColor;
		private int tryCount = 0;

		public RegisterContent()
		{
			InitializeComponent();

			txtLicenseKey.Width = this.Width - 50;

			_licenseKeyService = ObjectLocator.GetInstance<ILicenseKeyService>();
			_registerService = ObjectLocator.GetInstance<IRegisterService>();
		}

		public RegisterContent(ClientLicense clientLicense, ScutexLicense scutexLicense)
			: this()
		{
			_clientLicense = clientLicense;
			_scutexLicense = scutexLicense;

			switch (_clientLicense.KeyGeneratorType)
			{
				case KeyGeneratorTypes.StaticSmall:
					txtLicenseKey.InputMask = @"www-wwwwww-wwww";
					break;
				case KeyGeneratorTypes.StaticLarge:
					txtLicenseKey.InputMask = @"wwwww-wwwww-wwwww-wwwww-wwwww";
					break;
			}
		}

		private bool IsFormValid()
		{
			bool isValid = true;

			if (_originalBorderBrush == null)
				_originalBorderBrush = txtLicenseKey.BorderBrush;

			if (_originalThickness == null)
				_originalThickness = txtLicenseKey.BorderThickness;

			if (String.IsNullOrEmpty(txtLicenseKey.Text))
			{
				txtLicenseKey.BorderThickness = new Thickness(2);
				txtLicenseKey.BorderBrush = new SolidColorBrush(Colors.Red);

				lblResult.Foreground = new SolidColorBrush(Colors.Red);
				lblResult.Text = "License key not supplied!";

				isValid = false;
			}
			else
			{
				if (_licenseKeyService.ValidateLicenseKey(txtLicenseKey.Text, _clientLicense, true) == false)
				{
					txtLicenseKey.BorderThickness = new Thickness(2);
					txtLicenseKey.BorderBrush = new SolidColorBrush(Colors.Red);

					lblResult.Foreground = new SolidColorBrush(Colors.Red);
					lblResult.Text = "License key is invalid!";

					isValid = false;
				}
				else
				{
					txtLicenseKey.BorderBrush = _originalBorderBrush;
					txtLicenseKey.BorderThickness = _originalThickness;
				}
			}

			return isValid;
		}

		private void UserPermissionForActiviation()
		{
			switch (_clientLicense.KeyGeneratorType)
			{
				case KeyGeneratorTypes.StaticSmall:



					break;
			}
		}

		private void SetFormForInProcess()
		{
			pdgActivationProgress.Visibility = Visibility.Visible;

			if (_originalResultColor == null)
				_originalResultColor = lblResult.Foreground;

			lblResult.Text = "Activation in progress, please wait...";
			lblResult.Visibility = Visibility.Visible;
		}

		private void SetFormForInProcessFinished()
		{
			pdgActivationProgress.Visibility = System.Windows.Visibility.Hidden;

			if (_originalResultColor != null)
				lblResult.Foreground = _originalResultColor;

			lblResult.Text = "";
			lblResult.Visibility = Visibility.Hidden;
		}

		private void btnActivite_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			LicensingManager.InterfaceInteraction.ActiviateLicenseClicked = true;

			if (IsFormValid())
			{
				UserPermissionForActiviation();

				BackgroundWorker worker = new BackgroundWorker();
				SetFormForInProcess();

				worker.DoWork += delegate(object s, DoWorkEventArgs args)
				{
					object[] data = args.Argument as object[];

					RegisterResult result = _registerService.Register((string)data[0], (LicenseBase)data[1], (ScutexLicense)data[2]);
					object[] resultPkg = new object[2];

					resultPkg[0] = result.Result;
					resultPkg[1] = result.ScutexLicense;

					args.Result = resultPkg;
				};

				worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
				{
					ProcessCodes result = (ProcessCodes)((object[])args.Result)[0];

					SetFormForInProcessFinished();

					if (result == ProcessCodes.LicenseKeyInvalid)
					{
						tryCount++;
						LicensingManager.InterfaceInteraction.TimesActiviationAttempted = tryCount;

						if (tryCount > 3)
							throw new ScutexAttemptsException("More then three invalid licensing attempts.");

						lblResult.Foreground = new SolidColorBrush(Colors.Red);
						lblResult.Text = "License key is invalid!";
					}
					else if (result == ProcessCodes.ActivationFailed)
					{
						MessageBox.Show("Activation failed, please try again latter");
					}
					else if (result == ProcessCodes.ActivationSuccess)
					{
						MessageBox.Show("Your product is now registered. Thank you!");

						LicensingManager.ClientLicense = (ClientLicense)((object[])args.Result)[1];
						System.Windows.Threading.Dispatcher.CurrentDispatcher.InvokeShutdown();
					}
				};

				worker.RunWorkerAsync(new object[]
				                      	{
				                      		txtLicenseKey.Text,
																	_clientLicense,
																	_scutexLicense
				                      	});

			}
		}
	}
}