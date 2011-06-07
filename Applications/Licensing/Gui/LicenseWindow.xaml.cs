using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WaveTech.Scutex.Model;

namespace WaveTech.Scutex.Licensing.Gui
{
	/// <summary>
	/// Interaction logic for LicenseWindow.xaml
	/// </summary>
	public partial class LicenseWindow : Window
	{
		private ScutexLicense _scutexLicense;
		private ClientLicense _clientLicense;
		private DispatcherTimer tryButtonDelayTimer;
		private int _tryTimerTick = 0;
		private LicensingManager _licensingManager;

		public LicenseWindow() { }

		public LicenseWindow(object licensingManager, ScutexLicense scutexLicense, ClientLicense clientLicense)
		{
			InitializeComponent();

			_clientLicense = clientLicense;
			_licensingManager = licensingManager as LicensingManager;
			_scutexLicense = scutexLicense;

			SetIcon();
			SetWindowIcon();
			SetFormData();
			SetFormDisplay();
			SetTrialDelayTimer();
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Environment.Exit(1005);
		}

		private void SetWindowIcon()
		{
			try
			{
				IconBitmapDecoder ibd = new IconBitmapDecoder(
				new Uri("pack://application:,,,/" + GetType().Assembly.GetName().Name + ";component/Scutex2.ico", UriKind.RelativeOrAbsolute),
				BitmapCreateOptions.None,
				BitmapCacheOption.Default);
				Icon = ibd.Frames[0];
			}
			catch
			{ }

		}

		private void SetIcon()
		{
			try
			{
				Uri myUri = new Uri("pack://application:,,,/" + GetType().Assembly.GetName().Name + ";component/Scutex.png",
														UriKind.RelativeOrAbsolute);
				PngBitmapDecoder decoder = new PngBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
				BitmapSource bitmapSource = decoder.Frames[0];
				imageIcon.Source = bitmapSource;
			}
			catch
			{ }
		}

		private void SetFormData()
		{
			ccContent.Content = new WelcomeContent(_clientLicense.Product.Name);
		}

		private void SetFormDisplay()
		{
			switch (_scutexLicense.TrialSettings.ExpirationOptions)
			{
				case TrialExpirationOptions.Days:
					pgbDaysRemaining.Maximum = int.Parse(_scutexLicense.TrialSettings.ExpirationData);
					pgbDaysRemaining.Value = _scutexLicense.TrialUsed;

					txtDaysRemaining.Text = string.Format("{0} of {1} Days Remaining", _scutexLicense.TrialRemaining, _scutexLicense.TrialSettings.ExpirationData);
					break;
			}


		}

		private void SetTrialDelayTimer()
		{
			if (_scutexLicense.IsTrialExpired == false)
			{
				if (_scutexLicense.TrailNotificationSettings != null)
				{
					btnTry.IsEnabled = false;
					btnTry.Content = new TextBlock
														{
															Text = string.Format("({0}) Try", _scutexLicense.TrailNotificationSettings.TryButtonDelay),
															TextWrapping = TextWrapping.NoWrap
														};

					if (_scutexLicense.TrailNotificationSettings.TryButtonDelay > 0)
					{
						tryButtonDelayTimer = new DispatcherTimer();
						tryButtonDelayTimer.Interval = TimeSpan.FromMilliseconds(1000);
						tryButtonDelayTimer.Tick += new EventHandler(tryButtonDelayTimer_Elapsed);
						tryButtonDelayTimer.Start();
					}
					else
					{
						btnTry.IsEnabled = true;
						btnTry.Content = string.Format("Try");
					}
				}
				else
				{
					btnTry.IsEnabled = true;
					btnTry.Content = string.Format("Try");
				}
			}
			else
			{
				btnTry.IsEnabled = false;
				btnTry.Content = string.Format("EXPIRED");
			}
		}

		private void tryButtonDelayTimer_Elapsed(object sender, EventArgs e)
		{
			_tryTimerTick++;
			btnTry.Content = new TextBlock
												{
													Text = string.Format("({0}) Try",
																				(_scutexLicense.TrailNotificationSettings.TryButtonDelay - _tryTimerTick)),
													TextWrapping = TextWrapping.NoWrap
												};

			if ((_scutexLicense.TrailNotificationSettings.TryButtonDelay - _tryTimerTick) <= 0)
			{
				btnTry.IsEnabled = true;
				btnTry.Content = string.Format("Try");
				tryButtonDelayTimer.Stop();
			}
		}

		private void btnExit_OnClick(object sender, RoutedEventArgs e)
		{
			LicensingManager.InterfaceInteraction.ExitButtonClicked = true;
			Environment.Exit(1000);
		}

		private void BtnRegister_OnClick(object sender, RoutedEventArgs e)
		{
			LicensingManager.InterfaceInteraction.RegisterButtonClicked = true;
			ccContent.Content = new RegisterContent(_clientLicense, _scutexLicense);
		}

		private void BtnMoreInfo_OnClick(object sender, RoutedEventArgs e)
		{
			LicensingManager.InterfaceInteraction.MoreInfoButtonClicked = true;
			ccContent.Content = new MoreInfoContent(_clientLicense);
		}

		private void BtnTry_OnClick(object sender, RoutedEventArgs e)
		{
			LicensingManager.InterfaceInteraction.TryButtonClicked = true;

			LicensingManager.FormLicense = _scutexLicense;

			System.Windows.Threading.Dispatcher.CurrentDispatcher.InvokeShutdown();
		}
	}
}