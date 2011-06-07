using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Infragistics.Windows.Ribbon;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Manager.Windows
{
	/// <summary>
	/// Interaction logic for UploadKeysWindow.xaml
	/// </summary>
	public partial class UploadKeysWindow : XamRibbonWindow
	{
		#region Private Readonly Members
		private readonly IServicesService _servicesService;
		private readonly ILicenseSetService _licenseSetService;
		private readonly ILicenseService _licenseService;
		private readonly IKeyService _keyService;
		#endregion Private Readonly Members

		#region Private Members
		private List<string> _localKeys;
		private List<string> _serviceKeys;
		#endregion Private Members

		#region Dependency Properties
		public static readonly DependencyProperty ServiceProperty =
				DependencyProperty.Register("Service", typeof(Service), typeof(UploadKeysWindow),
				new FrameworkPropertyMetadata(
						null,
						FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
						ServicePropertyChanged));

		public static readonly DependencyProperty LicenseSetProperty =
				DependencyProperty.Register("LicenseSet", typeof(UploadProductDisplayData), typeof(UploadKeysWindow),
				new FrameworkPropertyMetadata(
						null,
						FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
						LicenseSetPropertyChanged));
		#endregion Dependency Properties

		#region Constructors
		public UploadKeysWindow()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);

			_servicesService = ObjectLocator.GetInstance<IServicesService>();
			_licenseSetService = ObjectLocator.GetInstance<ILicenseSetService>();
			_licenseService = ObjectLocator.GetInstance<ILicenseService>();
			_keyService = ObjectLocator.GetInstance<IKeyService>();
		}

		/// <summary>
		/// Constructor that takes a parent for this GenerationWindow window.
		/// </summary>
		/// <param name="parent">Parent window for this dialog.</param>
		public UploadKeysWindow(Window parent)
			: this()
		{
			this.Owner = parent;
		}
		#endregion Constructors

		#region Public Properties
		public Service Service
		{
			get { return (Service)GetValue(ServiceProperty); }
			set { SetValue(ServiceProperty, value); }
		}

		internal UploadProductDisplayData LicenseSet
		{
			get { return (UploadProductDisplayData)GetValue(LicenseSetProperty); }
			set { SetValue(LicenseSetProperty, value); }
		}
		#endregion Public Properties

		#region Static Event Handlers
		private static void ServicePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

		}

		private static void LicenseSetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

		}
		#endregion Static Event Handlers

		#region Private Event Handlers
		private void cboServices_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			GetDataFromService();
		}

		private void cboLicenseSets_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			GetDataFromService();
		}
		#endregion Private Event Handlers

		private void GetDataFromService()
		{
			if (Service != null && LicenseSet != null)
			{
				_serviceKeys = _servicesService.GetServiceLicenseKeysForSet(_licenseSetService.GetLiceseSetById(LicenseSet.LicenseSetId), Service);
				_localKeys = _keyService.GetAllHashedLicenseKeysByLicenseSet(LicenseSet.LicenseSetId);

				UpdateData();
			}
		}

		private void UpdateData()
		{
			if (_serviceKeys != null)
				lblServiceKeyCount.Text = _serviceKeys.Count.ToString();

			if (_localKeys != null)
				lblLocalKeyCount.Text = _localKeys.Count.ToString();
		}

		private void btnSynchronize_Click(object sender, RoutedEventArgs e)
		{
			if (Service != null && LicenseSet != null)
			{
				BackgroundWorker worker = new BackgroundWorker();
				loadingAnimation.Visibility = Visibility.Visible;

				List<string> keysToUpload = new List<string>();

				foreach (string k in _localKeys)
				{
					if (_serviceKeys.Contains(k) == false)
						keysToUpload.Add(k);
				}

				worker.DoWork += delegate(object s, DoWorkEventArgs args)
				{
					object[] data = args.Argument as object[];

					IServicesService servicesService = ObjectLocator.GetInstance<IServicesService>();
					ILicenseSetService licenseSetService = ObjectLocator.GetInstance<ILicenseSetService>();

					servicesService.AddLicenseKeysToService(licenseSetService.GetLiceseSetById((int)data[2]), (Service)data[1], (List<string>)data[0]);
					List<string> serviceKeys = servicesService.GetServiceLicenseKeysForSet(licenseSetService.GetLiceseSetById((int)data[2]), (Service)data[1]);

					args.Result = serviceKeys;
				};

				worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
				{
					_serviceKeys = (List<string>)args.Result;

					UpdateData();
					loadingAnimation.Visibility = Visibility.Collapsed;
				};

				worker.RunWorkerAsync(new object[]
				                      	{
				                      		keysToUpload,
																	Service,
																	LicenseSet.LicenseSetId
				                      	});
			}
		}
	}
}