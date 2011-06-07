using System;
using System.Diagnostics;
using System.Windows.Controls;
using WaveTech.Scutex.Licensing.Helpers;
using WaveTech.Scutex.Model;

namespace WaveTech.Scutex.Licensing.Gui
{
	/// <summary>
	/// Interaction logic for MoreInfoContent.xaml
	/// </summary>
	public partial class MoreInfoContent : UserControl
	{
		private ClientLicense _clientLicense;
		public MoreInfoContent()
		{
			InitializeComponent();
		}

		public MoreInfoContent(ClientLicense clientLicense)
			: this()
		{
			_clientLicense = clientLicense;

			btnBuyNowUrl.Content = _clientLicense.BuyNowUrl;
			btnHomepage.Content = _clientLicense.ProductUrl;
			btnEulaUrl.Content = _clientLicense.EulaUrl;
		}

		private void btnHomepage_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			LicensingManager.InterfaceInteraction.ProductUrlClicked = true;

			if (!String.IsNullOrEmpty(_clientLicense.ProductUrl))
			{
				try
				{
					Process.Start(BrowserHelper.GetDefaultBrowserPath(), _clientLicense.ProductUrl);
				}
				catch { }
			}
		}

		private void btnBuyNowUrl_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			LicensingManager.InterfaceInteraction.BuyNowUrlClicked = true;

			if (!String.IsNullOrEmpty(_clientLicense.ProductUrl))
			{
				try
				{
					Process.Start(BrowserHelper.GetDefaultBrowserPath(), _clientLicense.BuyNowUrl);
				}
				catch { }
			}
		}

		private void btnEulaUrl_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			LicensingManager.InterfaceInteraction.EulaUrlClicked = true;

			if (!String.IsNullOrEmpty(_clientLicense.ProductUrl))
			{
				try
				{
					Process.Start(BrowserHelper.GetDefaultBrowserPath(), _clientLicense.EulaUrl);
				}
				catch { }
			}
		}
	}
}