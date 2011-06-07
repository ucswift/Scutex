using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Infragistics.Windows.Ribbon;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Generators;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Services;
using WaveTech.Scutex.Services;
using LicenseSet = WaveTech.Scutex.Model.LicenseSet;
using MessageBox = System.Windows.MessageBox;

namespace WaveTech.Scutex.Manager.Windows
{
	/// <summary>
	/// Interaction logic for GenerationWindow.xaml
	/// </summary>
	public partial class GenerationWindow : XamRibbonWindow
	{
		#region Dependency Properties
		public static readonly DependencyProperty LicenseKeysProperty =
	DependencyProperty.Register("License", typeof(BindingList<string>), typeof(GenerationWindow),
	new FrameworkPropertyMetadata(
			null,
			FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
			LicenseKeysPropertyChanged));

		public BindingList<string> LicenseKeys
		{
			get { return (BindingList<string>)GetValue(LicenseKeysProperty); }
			set { SetValue(LicenseKeysProperty, value); }
		}

		private static void LicenseKeysPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

		}
		#endregion Dependency Properties

		#region Constructors
		public GenerationWindow()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);

			SetLicenseCapibilites();
		}

		/// <summary>
		/// Constructor that takes a parent for this GenerationWindow window.
		/// </summary>
		/// <param name="parent">Parent window for this dialog.</param>
		public GenerationWindow(Window parent)
			: this()
		{
			this.Owner = parent;
		}
		#endregion Constrctors

		#region Private Members
		private void ResetForm()
		{
			txtKeysToGenerate.Text = String.Empty;
			cboLicenseSet.SelectedItem = null;
			//lstLicenseKeys.Items.Clear();
			cboUnlimited.IsChecked = false;
			cboSingleUser.IsChecked = false;
			cboHardwareLock.IsChecked = false;
			cboEnterprise.IsChecked = false;
			cboMultiUser.IsChecked = false;

			LicenseKeys = new BindingList<string>();
			lstLicenseKeys.ItemsSource = LicenseKeys;
		}

		private void SetLicenseCapibilites()
		{
			LicenseCapability capibilities = null;

			if (UIContext.License != null)
			{
				switch (UIContext.License.KeyGeneratorType)
				{
					case KeyGeneratorTypes.StaticSmall:
						IKeyGenerator keygen = ObjectLocator.GetInstance<IKeyGenerator>(InstanceNames.SmallKeyGenerator);
						capibilities = keygen.GetLicenseCapability();
						break;
					default:
						break;
				}
			}

			if (capibilities != null)
			{
				if (capibilities.SupportedLicenseKeyTypes.IsSet(LicenseKeyTypeFlag.SingleUser))
				{
					cboSingleUser.IsEnabled = true;
				}
				else
				{
					cboSingleUser.IsChecked = false;
					cboSingleUser.IsEnabled = false;
				}

				if (capibilities.SupportedLicenseKeyTypes.IsSet(LicenseKeyTypeFlag.MultiUser))
				{
					cboMultiUser.IsEnabled = true;
				}
				else
				{
					cboMultiUser.IsChecked = false;
					cboMultiUser.IsEnabled = false;
				}

				if (capibilities.SupportedLicenseKeyTypes.IsSet(LicenseKeyTypeFlag.HardwareLock))
				{
					cboHardwareLock.IsEnabled = true;
				}
				else
				{
					cboHardwareLock.IsChecked = false;
					cboHardwareLock.IsEnabled = false;
				}

				if (capibilities.SupportedLicenseKeyTypes.IsSet(LicenseKeyTypeFlag.Unlimited))
				{
					cboUnlimited.IsEnabled = true;
				}
				else
				{
					cboUnlimited.IsChecked = false;
					cboUnlimited.IsEnabled = false;
				}

				if (capibilities.SupportedLicenseKeyTypes.IsSet(LicenseKeyTypeFlag.Enterprise))
				{
					cboEnterprise.IsEnabled = true;
				}
				else
				{
					cboEnterprise.IsChecked = false;
					cboEnterprise.IsEnabled = false;
				}
			}
		}

		private void SetLicenseSetCapibilites()
		{
			if (cboLicenseSet.SelectedValue != null)
			{
				if (((LicenseSet)cboLicenseSet.SelectedValue).SupportedLicenseTypes.IsSet(LicenseKeyTypeFlag.SingleUser))
				{
					cboSingleUser.IsEnabled = true;
				}
				else
				{
					cboSingleUser.IsChecked = false;
					cboSingleUser.IsEnabled = false;
				}

				if (((LicenseSet)cboLicenseSet.SelectedValue).SupportedLicenseTypes.IsSet(LicenseKeyTypeFlag.MultiUser))
				{
					cboMultiUser.IsEnabled = true;
				}
				else
				{
					cboMultiUser.IsChecked = false;
					cboMultiUser.IsEnabled = false;
				}

				if (((LicenseSet)cboLicenseSet.SelectedValue).SupportedLicenseTypes.IsSet(LicenseKeyTypeFlag.HardwareLock))
				{
					cboHardwareLock.IsEnabled = true;
				}
				else
				{
					cboHardwareLock.IsChecked = false;
					cboHardwareLock.IsEnabled = false;
				}

				if (((LicenseSet)cboLicenseSet.SelectedValue).SupportedLicenseTypes.IsSet(LicenseKeyTypeFlag.Unlimited))
				{
					cboUnlimited.IsEnabled = true;
				}
				else
				{
					cboUnlimited.IsChecked = false;
					cboUnlimited.IsEnabled = false;
				}

				if (((LicenseSet)cboLicenseSet.SelectedValue).SupportedLicenseTypes.IsSet(LicenseKeyTypeFlag.Enterprise))
				{
					cboEnterprise.IsEnabled = true;
				}
				else
				{
					cboEnterprise.IsChecked = false;
					cboEnterprise.IsEnabled = false;
				}
			}
		}

		private string GetGeneratorName()
		{
			switch (UIContext.License.KeyGeneratorType)
			{
				case KeyGeneratorTypes.StaticSmall:
					return InstanceNames.SmallKeyGenerator;

				default:
					return null;
			}
		}

		private bool IsGenerationFormValid()
		{
			int selection = 0;

			if (String.IsNullOrEmpty(txtKeysToGenerate.Text))
				return false;

			if (cboLicenseSet.SelectedValue == null)
				return false;

			if (cboSingleUser.IsChecked.HasValue && cboSingleUser.IsChecked.Value)
				selection++;

			if (cboMultiUser.IsChecked.HasValue && cboMultiUser.IsChecked.Value)
				selection++;

			if (cboHardwareLock.IsChecked.HasValue && cboHardwareLock.IsChecked.Value)
				selection++;

			if (cboUnlimited.IsChecked.HasValue && cboUnlimited.IsChecked.Value)
				selection++;

			if (cboEnterprise.IsChecked.HasValue && cboEnterprise.IsChecked.Value)
				selection++;

			if (selection == 0)
				return false;

			return true;
		}
		#endregion Private Members

		#region Event Handlers
		private void btnGenerateKeys_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			if (IsGenerationFormValid())
			{
				loadingAnimation.Visibility = Visibility.Visible;

				BackgroundWorker worker = new BackgroundWorker();
				LicenseGenerationOptions licenseGenerationOptions = new LicenseGenerationOptions();

				if (cboSingleUser.IsChecked.HasValue && cboSingleUser.IsChecked.Value)
					licenseGenerationOptions.LicenseKeyType = LicenseKeyTypes.SingleUser;
				else if (cboMultiUser.IsChecked.HasValue && cboMultiUser.IsChecked.Value)
					licenseGenerationOptions.LicenseKeyType = LicenseKeyTypes.MultiUser;
				else if (cboHardwareLock.IsChecked.HasValue && cboHardwareLock.IsChecked.Value)
					licenseGenerationOptions.LicenseKeyType = LicenseKeyTypes.HardwareLock;
				else if (cboUnlimited.IsChecked.HasValue && cboUnlimited.IsChecked.Value)
					licenseGenerationOptions.LicenseKeyType = LicenseKeyTypes.Unlimited;
				else if (cboEnterprise.IsChecked.HasValue && cboEnterprise.IsChecked.Value)
					licenseGenerationOptions.LicenseKeyType = LicenseKeyTypes.Enterprise;

				worker.DoWork += delegate(object s, DoWorkEventArgs args)
													{
														object[] data = args.Argument as object[];

														IKeyGenerator keygen = ObjectLocator.GetInstance<IKeyGenerator>((string)data[2]);
														ILicenseActiviationProvider licenseActiviationProvider = ObjectLocator.GetInstance<ILicenseActiviationProvider>();
														IPackingService packingService = ObjectLocator.GetInstance<IPackingService>();
														IClientLicenseService clientLicenseService = ObjectLocator.GetInstance<IClientLicenseService>();

														LicenseKeyService service = new LicenseKeyService(keygen, packingService, clientLicenseService);
														List<string> keys = service.GenerateLicenseKeys(UIContext.License.KeyPair.PrivateKey,
																																						UIContext.License,
																																						(LicenseGenerationOptions)data[0],
																																						int.Parse(data[1].ToString()));

														args.Result = keys;
													};

				worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
																			{
																				LicenseKeys = new BindingList<string>((List<string>)args.Result);
																				lstLicenseKeys.ItemsSource = LicenseKeys;
																				loadingAnimation.Visibility = Visibility.Collapsed;
																			};

				worker.RunWorkerAsync(new object[]
				                      	{
				                      		licenseGenerationOptions,
				                      		txtKeysToGenerate.Text,
				                      		GetGeneratorName()
				                      	});
			}
			else
			{
				MessageBox.Show("Please select a license set, license type to generate and a amount.");
			}
		}

		private void btnSaveAndExport_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			if (lstLicenseKeys.Items.Count > 0)
			{
				SaveFileDialog dialog = new SaveFileDialog();
				dialog.DefaultExt = ".txt";
				dialog.Filter = "Text Files (.txt)|*.txt";

				dialog.ShowDialog();
				if (!String.IsNullOrEmpty(dialog.FileName))
				{
					loadingAnimation.Visibility = Visibility.Visible;
					BackgroundWorker worker = new BackgroundWorker();

					worker.DoWork += delegate(object s, DoWorkEventArgs args)
					{
						object[] data = args.Argument as object[];

						IKeyService keyService = ObjectLocator.GetInstance<IKeyService>();
						keyService.SaveLicenseKeysForLicenseSet((LicenseSet)data[0], data[1] as List<string>);

						IKeyExportService keyExportService = ObjectLocator.GetInstance<IKeyExportService>();
						keyExportService.ExportKeysToFile(data[2].ToString(), data[3].ToString(),
																							((LicenseSet)data[0]).Name, data[1] as List<string>);

						System.Diagnostics.Process.Start(dialog.FileName);
					};

					worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
					{
						ResetForm();
						loadingAnimation.Visibility = Visibility.Collapsed;
					};

					worker.RunWorkerAsync(new object[]
				                      	{
				                      		cboLicenseSet.SelectedValue,
				                      		LicenseKeys.ToList(),
				                      		dialog.FileName,
																	UIContext.License.Name
				                      	});
				}
			}
			else
			{
				MessageBox.Show("No license keys have been generated.");
			}
		}

		private void cboLicenseSet_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			SetLicenseSetCapibilites();
		}
		#endregion Event Handlers
	}
}