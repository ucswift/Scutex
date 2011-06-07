using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Infragistics.Windows.DataPresenter;
using Infragistics.Windows.Ribbon;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Services;
using License = WaveTech.Scutex.Model.License;

namespace WaveTech.Scutex.Manager.Windows
{
	/// <summary>
	/// Interaction logic for UploadProductsWindow.xaml
	/// </summary>
	public partial class UploadProductsWindow : XamRibbonWindow
	{
		#region Private Readonly Members
		private readonly IServicesService _servicesService;
		private readonly ILicenseSetService _licenseSetService;
		private readonly ILicenseService _licenseService;
		#endregion Private Readonly Members

		#region Dependency Properties
		public static readonly DependencyProperty ServiceProperty =
				DependencyProperty.Register("Service", typeof(Service), typeof(UploadProductsWindow),
				new FrameworkPropertyMetadata(
						null,
						FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
						ServicePropertyChanged));

		public Service Service
		{
			get { return (Service)GetValue(ServiceProperty); }
			set { SetValue(ServiceProperty, value); }
		}

		private static void ServicePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

		}
		#endregion Dependency Properties

		#region Private Members
		private Dictionary<License, List<LicenseSet>> servicesData;
		#endregion Private Members

		#region Constructors
		public UploadProductsWindow()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);

			_servicesService = ObjectLocator.GetInstance<IServicesService>();
			_licenseSetService = ObjectLocator.GetInstance<ILicenseSetService>();
			_licenseService = ObjectLocator.GetInstance<ILicenseService>();
		}

		/// <summary>
		/// Constructor that takes a parent for this GenerationWindow window.
		/// </summary>
		/// <param name="parent">Parent window for this dialog.</param>
		public UploadProductsWindow(Window parent)
			: this()
		{
			this.Owner = parent;
		}
		#endregion Constructors

		#region Private Methods
		private void UpdateServiceGrid()
		{
			servicesData = _servicesService.GetServiceLicenses((Service)cboServices.SelectedValue);

			gridRemoteServices.DataSource = DataConverters.ConvertAllLicensesSetsToDisplay(servicesData);
		}

		private bool DoesLicenseSetExistOnService()
		{
			if (servicesData != null)
			{
				DataRecord record = gridLocalServices.ActiveRecord as DataRecord;
				int licenseSetId = (int)record.Cells["LicenseSetId"].Value;
				int licenseId = (int)record.Cells["LicenseId"].Value;

				var licenseSets = from l in servicesData
													where l.Key.LicenseId == licenseId
													select l.Value;

				var sets = licenseSets.FirstOrDefault();

				if (sets != null)
				{
					int count = sets.Where(x => x.LicenseSetId == licenseSetId).Count();

					if (count > 0)
						return true;
				}
			}

			return false;
		}
		#endregion Private Methods

		#region Private Event Handlers
		private void cboServices_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (cboServices.SelectedValue != null)
			{
				BackgroundWorker worker = new BackgroundWorker();
				loadingAnimation.Visibility = Visibility.Visible;

				worker.DoWork += delegate(object s, DoWorkEventArgs args)
				{
					object[] data = args.Argument as object[];

					IServicesService servicesService = ObjectLocator.GetInstance<IServicesService>();
					Dictionary<License, List<LicenseSet>> _servicesData = servicesService.GetServiceLicenses((Service)data[0]);

					args.Result = _servicesData;
				};

				worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
				{
					servicesData = (Dictionary<License, List<LicenseSet>>)args.Result;
					gridRemoteServices.DataSource = DataConverters.ConvertAllLicensesSetsToDisplay(servicesData);

					loadingAnimation.Visibility = Visibility.Collapsed;
				};

				worker.RunWorkerAsync(new object[]
				                      	{
				                      		cboServices.SelectedValue
				                      	});
			}
		}

		private void btnUploadLicenseSets_Click(object sender, RoutedEventArgs e)
		{
			if (cboServices.SelectedValue != null)
			{
				if (gridLocalServices.ActiveRecord != null)
				{
					if (DoesLicenseSetExistOnService() == false)
					{
						BackgroundWorker worker = new BackgroundWorker();
						loadingAnimation.Visibility = Visibility.Visible;

						DataRecord record = gridLocalServices.ActiveRecord as DataRecord;
						int licenseSetId = (int)record.Cells["LicenseSetId"].Value;

						worker.DoWork += delegate(object s, DoWorkEventArgs args)
						{
							object[] data = args.Argument as object[];

							IServicesService servicesService = ObjectLocator.GetInstance<IServicesService>();
							ILicenseSetService licenseSetService = ObjectLocator.GetInstance<ILicenseSetService>();
							ILicenseService licenseService = ObjectLocator.GetInstance<ILicenseService>();

							LicenseSet licenseSet = licenseSetService.GetLiceseSetById((int)data[0]);
							License license = licenseService.GetLicenseById((int)data[0]);

							List<LicenseSet> sets = new List<LicenseSet>();
							sets.Add(licenseSet);

							servicesService.AddProductToService(license, sets, data[1] as Service);
							Dictionary<License, List<LicenseSet>> _servicesData = servicesService.GetServiceLicenses((Service)data[1]);

							args.Result = _servicesData;
						};

						worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
						{
							servicesData = (Dictionary<License, List<LicenseSet>>)args.Result;
							gridRemoteServices.DataSource = DataConverters.ConvertAllLicensesSetsToDisplay(servicesData);

							loadingAnimation.Visibility = Visibility.Collapsed;
						};

						worker.RunWorkerAsync(new object[]
				                      	{
				                      		licenseSetId,
																	cboServices.SelectedValue
				                      	});
					}
					else
					{
						MessageBox.Show("The License Set you selected already exists in the service");
					}
				}
				else
				{
					MessageBox.Show("You must select a License/LicenseSet to upload");
				}
			}
			else
			{
				MessageBox.Show("You must select a service to upload to.");
			}
		}
		#endregion Private Event Handlers
	}
}