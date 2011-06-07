using System.Windows;
using System.Windows.Controls;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model;

namespace WaveTech.Scutex.Manager.Forms
{
	/// <summary>
	/// Interaction logic for TrialSettings.xaml
	/// </summary>
	public partial class TrialSettings : UserControl
	{
		public static readonly DependencyProperty LicenseProperty =
			DependencyProperty.Register("License", typeof(License), typeof(TrialSettings),
			new FrameworkPropertyMetadata(
					null,
					FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
					LicensePropertyChanged));

		public TrialSettings()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);

			Loaded += new RoutedEventHandler(TrialSettings_Loaded);
		}

		private void TrialSettings_Loaded(object sender, RoutedEventArgs e)
		{
			if (License != null && License.TrialSettings != null)
			{
				switch (License.TrialSettings.ExpirationOptions)
				{
					case TrialExpirationOptions.Days:
						rdoTrialExpireDays.IsChecked = true;
						break;
				}
			}
			else
			{
				if (License != null)
				{
					if (License.TrialSettings == null)
						License.TrialSettings = new LicenseTrialSettings();
				}
			}
		}

		public License License
		{
			get { return (License)GetValue(LicenseProperty); }
			set { SetValue(LicenseProperty, value); }
		}

		private static void LicensePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

		}

		private void cboTrialNotificationType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox comboBox = sender as ComboBox;

			if (comboBox != null && comboBox.SelectedValue != null)
			{
				if (ApplicationConstants.IsCommunityEdition && (TrialNotificationTypes)comboBox.SelectedValue != TrialNotificationTypes.Form)
				{
					MessageBox.Show("The Community Edition of Scutex only supports the Form trial notificaiton type.");
					comboBox.SelectedValue = TrialNotificationTypes.Form;
				}
			}
		}

		private void rdoTrialExpireDays_Checked(object sender, RoutedEventArgs e)
		{
			License.TrialSettings.ExpirationOptions = TrialExpirationOptions.Days;

		}
	}
}