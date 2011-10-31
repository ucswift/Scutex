using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Manager.Windows
{
	/// <summary>
	/// Interaction logic for ServiceLogWindow.xaml
	/// </summary>
	public partial class ServiceLogWindow : Window
	{
		#region Dependency Properties
		public static readonly DependencyProperty ServiceProperty =
		DependencyProperty.Register("Service", typeof(Service), typeof(ServiceLogWindow),
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

		#region Constructors
		public ServiceLogWindow()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);
		}

		/// <summary>
		/// Constructor that takes a parent for this GenerationWindow window.
		/// </summary>
		/// <param name="parent">Parent window for this dialog.</param>
		public ServiceLogWindow(Window parent)
			: this()
		{
			this.Owner = parent;
		}
		#endregion Constructors

		#region Private Event Handlers
		private void btnRefresh_Click(object sender, RoutedEventArgs e)
		{
			if (Service != null)
			{
				BackgroundWorker worker = new BackgroundWorker();
				loadingAnimation.Visibility = Visibility.Visible;

				worker.DoWork += delegate(object s, DoWorkEventArgs args)
				{
					object[] data = args.Argument as object[];

					IReportingService reportingService = ObjectLocator.GetInstance<IReportingService>();

					List<ActivationLog> activationLogs = reportingService.GetAllServiceActivationLogs((Service)data[0]);
					List<LicenseActivation> licenseActivation = reportingService.GetAllServiceLicenseActivations((Service)data[0]);

					object[] returnData = new object[2];
					returnData[0] = activationLogs;
					returnData[1] = licenseActivation;

					args.Result = returnData;
				};

				worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
				{
					object[] result = args.Result as object[];

					gridActivationLogs.ItemsSource = (List<ActivationLog>)result[0];
					gridLicenseActiviations.ItemsSource = (List<LicenseActivation>)result[1];

					loadingAnimation.Visibility = Visibility.Collapsed;
				};

				worker.RunWorkerAsync(new object[]
				                      	{
				                      		Service
				                      	});

			}
			else
			{
				MessageBox.Show("You need to select a service before refreshing the data.");
			}
		}
		#endregion Private Event Handlers
	}
}