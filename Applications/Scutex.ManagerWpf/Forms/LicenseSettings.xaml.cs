using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Events;
using WaveTech.Scutex.Model.Interfaces.Framework;
using WaveTech.Scutex.Model.Interfaces.Generators;
using License = WaveTech.Scutex.Model.License;

namespace WaveTech.Scutex.Manager.Forms
{
	/// <summary>
	/// Interaction logic for LicenseSettings.xaml
	/// </summary>
	public partial class LicenseSettings : UserControl
	{
		public static readonly DependencyProperty LicenseProperty =
		DependencyProperty.Register("License", typeof(License), typeof(LicenseSettings),
		new FrameworkPropertyMetadata(
				null,
				FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
				LicensePropertyChanged));

		public LicenseSettings()
		{
			IEventAggregator eventAggregator = ObjectLocator.GetInstance<IEventAggregator>();
			eventAggregator.AddListener<LicenseInitializedEvent>(x => SetData());
			eventAggregator.AddListener<ServicesUpdatedEvent>(x => cboService.ItemsSource = UIContext.GetAllServices());

			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);

			this.Loaded += new RoutedEventHandler(LicenseSettings_GotFocus);

			SetData();
		}

		void LicenseSettings_GotFocus(object sender, RoutedEventArgs e)
		{
			SetLicenseCapibilites();
		}

		public License License
		{
			get { return (License)GetValue(LicenseProperty); }
			set { SetValue(LicenseProperty, value); }
		}

		private void SetData()
		{
			if (License != null && License.LicenseSets != null)
			{
			}
			//  licenseSets = new BindingList<LicenseSet>(License.LicenseSets);
			//else
			//  licenseSets = new BindingList<LicenseSet>();

			//gridLicenseSets.DataSource = licenseSets;

			SetLicenseCapibilites();
		}

		private void SetLicenseCapibilites()
		{
			LicenseCapability capibilities = null;

			if (cboLicenseKeyType.SelectedValue != null)
			{
				switch ((KeyGeneratorTypes)cboLicenseKeyType.SelectedValue)
				{
					case KeyGeneratorTypes.StaticSmall:
						IKeyGenerator keygen = ObjectLocator.GetInstance<IKeyGenerator>(InstanceNames.SmallKeyGenerator);
						capibilities = keygen.GetLicenseCapability();

						chkIsLicenseSetUpgradeOnly.IsChecked = false;
						chkIsLicenseSetUpgradeOnly.IsEnabled = false;
						break;
					case KeyGeneratorTypes.StaticLarge:
						IKeyGenerator keygenLarge = ObjectLocator.GetInstance<IKeyGenerator>(InstanceNames.LargeKeyGenerator);
						capibilities = keygenLarge.GetLicenseCapability();

						chkIsLicenseSetUpgradeOnly.IsChecked = false;
						chkIsLicenseSetUpgradeOnly.IsEnabled = true;
						break;
					break;
					case KeyGeneratorTypes.None:
						capibilities = null;
						break;
					default:
						break;
				}
			}

			if (capibilities != null)
			{
				if (capibilities.SupportedLicenseKeyTypes.IsSet(LicenseKeyTypeFlag.SingleUser))
				{
					chkSingleUser.IsEnabled = true;
				}
				else
				{
					chkSingleUser.IsChecked = false;
					chkSingleUser.IsEnabled = false;
				}

				if (capibilities.SupportedLicenseKeyTypes.IsSet(LicenseKeyTypeFlag.MultiUser))
				{
					chkMultiUser.IsEnabled = true;
				}
				else
				{
					chkMultiUser.IsChecked = false;
					chkMultiUser.IsEnabled = false;
				}

				if (capibilities.SupportedLicenseKeyTypes.IsSet(LicenseKeyTypeFlag.HardwareLock))
				{
					chkHardwareLock.IsEnabled = true;
				}
				else
				{
					chkHardwareLock.IsChecked = false;
					chkHardwareLock.IsEnabled = false;
				}

				if (capibilities.SupportedLicenseKeyTypes.IsSet(LicenseKeyTypeFlag.Unlimited))
				{
					chkUnlimited.IsEnabled = true;
				}
				else
				{
					chkUnlimited.IsChecked = false;
					chkUnlimited.IsEnabled = false;
				}

				if (capibilities.SupportedLicenseKeyTypes.IsSet(LicenseKeyTypeFlag.Enterprise))
				{
					chkEnterprise.IsEnabled = true;
				}
				else
				{
					chkEnterprise.IsChecked = false;
					chkEnterprise.IsEnabled = false;
				}
			}
			else
			{
				chkSingleUser.IsChecked = false;
				chkSingleUser.IsEnabled = false;
				chkMultiUser.IsChecked = false;
				chkMultiUser.IsEnabled = false;
				chkHardwareLock.IsChecked = false;
				chkHardwareLock.IsEnabled = false;
				chkUnlimited.IsChecked = false;
				chkUnlimited.IsEnabled = false;
				chkEnterprise.IsChecked = false;
				chkEnterprise.IsEnabled = false;
			}
		}

		private bool IsLicenseSetFormValid()
		{
			int checkedAmount = 0;
			int intTryParseInt = 0;
			bool isFormValid = true;

			if (String.IsNullOrEmpty(txtLicenseSetName.Text))
			{
				txtLicenseSetName.Background = new SolidColorBrush(Colors.MistyRose);
				isFormValid = false;
			}
			else
			{
				txtLicenseSetName.Background = new SolidColorBrush(Colors.White);
			}

			if (chkMultiUser.IsChecked.HasValue && chkMultiUser.IsChecked.Value)
			{
				if (String.IsNullOrEmpty(txtMaxUsers.Text))
				{
					txtMaxUsers.Background = new SolidColorBrush(Colors.MistyRose);
					isFormValid = false;
				}
				else
				{
					txtMaxUsers.Background = new SolidColorBrush(Colors.White);
				}
			}

			if (String.IsNullOrEmpty(txtMaxUsers.Text) == false)
			{
				if (int.TryParse(txtMaxUsers.Text, out intTryParseInt) == false)
				{
					txtMaxUsers.Background = new SolidColorBrush(Colors.MistyRose);
					isFormValid = false;
				}
				else
				{
					txtMaxUsers.Background = new SolidColorBrush(Colors.White);
				}
			}

			if (chkSingleUser.IsChecked.HasValue && chkSingleUser.IsChecked.Value)
				checkedAmount++;

			if (chkMultiUser.IsChecked.HasValue && chkMultiUser.IsChecked.Value)
				checkedAmount++;

			if (chkHardwareLock.IsChecked.HasValue && chkHardwareLock.IsChecked.Value)
				checkedAmount++;

			if (chkUnlimited.IsChecked.HasValue && chkUnlimited.IsChecked.Value)
				checkedAmount++;

			if (chkEnterprise.IsChecked.HasValue && chkEnterprise.IsChecked.Value)
				checkedAmount++;

			if (checkedAmount == 0)
				isFormValid = false;

			return isFormValid;
		}

		private void ResetNewLicenseSetForm()
		{
			txtLicenseSetName.Text = String.Empty;

			chkSingleUser.IsChecked = false;
			chkMultiUser.IsChecked = false;
			chkHardwareLock.IsChecked = false;
			chkUnlimited.IsChecked = false;
			chkEnterprise.IsChecked = false;
		}

		private void SyncData()
		{
			//License.LicenseSets = licenseSets.ToList();
		}

		private static void LicensePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

		}

		private void btnRemoveLicenseSet_Click(object sender, RoutedEventArgs e)
		{
			if (gridLicenseSets.SelectedItem != null)
			{
				LicenseSet licSet = gridLicenseSets.SelectedItem as LicenseSet;

				if (MessageBox.Show(string.Format("Are you sure you want to delete the {0} license set?", licSet.Name), "Delete License Set", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
				{
					License.LicenseSets.Remove(License.LicenseSets.Where(x => x.Name == licSet.Name).First());
					//SyncData();
				}
			}
			else
			{
				MessageBox.Show("You must select a license set to remove.");
			}
		}

		private void btnAddLicenseSet_Click(object sender, RoutedEventArgs e)
		{
			if (IsLicenseSetFormValid())
			{
				var licSet = from ls in License.LicenseSets
										 where ls.Name == txtLicenseSetName.Text.Trim()
										 select ls;

				if (licSet.Count() > 0)
				{
					MessageBox.Show(string.Format("LicenseSet name [{0}] is already in use in this project.", txtLicenseSetName.Text.Trim()));
					return;
				}

				if (((KeyGeneratorTypes)cboLicenseKeyType.SelectedItem) == KeyGeneratorTypes.StaticSmall && License.LicenseSets.Count > 1)
				{
					MessageBox.Show("The Small Static Key Generator only supports a maximum of 1 license set.");
					return;
				}

				LicenseSet l = new LicenseSet();
				l.Name = txtLicenseSetName.Text.Trim();

				LicenseKeyTypeFlag flag = new LicenseKeyTypeFlag();

				if (chkSingleUser.IsChecked.HasValue && chkSingleUser.IsChecked.Value)
					flag = LicenseKeyTypeFlag.SingleUser;

				if (chkMultiUser.IsChecked.HasValue && chkMultiUser.IsChecked.Value)
					flag = flag | LicenseKeyTypeFlag.MultiUser;

				if (chkHardwareLock.IsChecked.HasValue && chkHardwareLock.IsChecked.Value)
					flag = flag | LicenseKeyTypeFlag.HardwareLock;

				if (chkUnlimited.IsChecked.HasValue && chkUnlimited.IsChecked.Value)
					flag = flag | LicenseKeyTypeFlag.Unlimited;

				if (chkEnterprise.IsChecked.HasValue && chkEnterprise.IsChecked.Value)
					flag = flag | LicenseKeyTypeFlag.Enterprise;

				l.SupportedLicenseTypes = flag;

				if (String.IsNullOrEmpty(txtMaxUsers.Text) == false)
					l.MaxUsers = int.Parse(txtMaxUsers.Text);

				if (chkIsLicenseSetUpgradeOnly.IsChecked.HasValue)
					l.IsUpgradeOnly = chkIsLicenseSetUpgradeOnly.IsChecked.Value;
				else
					l.IsUpgradeOnly = false;

				License.LicenseSets.Add(l);
				License.RaisePropertyChanged("LicenseSets");

				ResetNewLicenseSetForm();
				SyncData();

			}
			else
			{
				MessageBox.Show("Please fix the errors in the New License Set form and try again.");
			}
		}

		private void cboLicenseKeyType_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (cboLicenseKeyType.SelectedValue != null)
			{
				SetLicenseCapibilites();
			}
		}

		private void btnAddService_Click(object sender, RoutedEventArgs e)
		{
			Commands.NewServiceCommand.Execute(this, this);
		}

		private void btnSetLicenseSetFeatures_Click(object sender, RoutedEventArgs e)
		{
			if (gridLicenseSets.SelectedItem != null)
			{
				
			}
			else
			{
				MessageBox.Show("You must select a license set to set features for.");
			}
		}
	}
}