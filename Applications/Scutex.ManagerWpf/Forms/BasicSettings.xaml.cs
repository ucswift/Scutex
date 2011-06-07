using System.Windows;
using System.Windows.Controls;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Events;
using WaveTech.Scutex.Model.Interfaces.Framework;
using WaveTech.Scutex.Model.Interfaces.Providers;
using UserControl = System.Windows.Controls.UserControl;

namespace WaveTech.Scutex.Manager.Forms
{
	/// <summary>
	/// Interaction logic for BasicSettings.xaml
	/// </summary>
	public partial class BasicSettings : UserControl
	{
		public static readonly DependencyProperty LicenseProperty =
				DependencyProperty.Register("License", typeof(License), typeof(BasicSettings),
				new FrameworkPropertyMetadata(
						null,
						FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
						LicensePropertyChanged));

		public BasicSettings()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);

			//btnAddProduct.Foreground = new SolidColorBrush(Colors.DarkSlateBlue); //Colors.CornflowerBlue
			//btnRegenerateKeys.Foreground = new SolidColorBrush(Colors.DarkSlateBlue);

			UIContext.InitializeForNewLicense();
			License = UIContext.License;

			IEventAggregator eventAggregator = ObjectLocator.GetInstance<IEventAggregator>();
			eventAggregator.AddListener<ProductsUpdatedEvent>(x => cboProduct.ItemsSource = UIContext.GetProducts());
			eventAggregator.SendMessage<LicenseInitializedEvent>();
		}

		public License License
		{
			get { return (License)GetValue(LicenseProperty); }
			set { SetValue(LicenseProperty, value); }
		}

		private static void LicensePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

		}

		private void cboProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		private void btnRegenerateKeys_Click(object sender, RoutedEventArgs e)
		{
			if (MessageBox.Show("Regenerating keys will cause existing clients to be unable to authenticate license keys. It's not recommended you regenerate encryption keys unless they have been comprlimised.\r\n\r\nAre you sure you want to continue?", "WARNING: Key Regeneration", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
			{
				IAsymmetricEncryptionProvider asymmetricEncryptionProvider =
					ObjectLocator.GetInstance<IAsymmetricEncryptionProvider>();

				UIContext.License.KeyPair = asymmetricEncryptionProvider.GenerateKeyPair(BitStrengths.High);
			}
		}

		private void btnAddProduct_Click(object sender, RoutedEventArgs e)
		{
			Commands.ProductsCommand.Execute(this, this);
		}
	}
}