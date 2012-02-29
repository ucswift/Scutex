using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Events;
using WaveTech.Scutex.Model.Interfaces.Framework;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Manager.Windows
{
	public partial class ServiceInitializationWindow : Window
	{
		private Service _service;

		public ServiceInitializationWindow()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);
		}

		public ServiceInitializationWindow(Window parent, Service service)
			: this()
		{
			this.Owner = parent;
			_service = service;

			lblServiceName.Text = service.Name;
			lblServiceClientUrl.Text = service.ClientUrl;
			lblServiceMgmtUrl.Text = service.ManagementUrl;
		}

		private void btnInitalize_Click(object sender, RoutedEventArgs e)
		{
			
		}

		private void InitializeService()
		{
			BackgroundWorker worker = new BackgroundWorker();


			worker.DoWork += delegate(object s, DoWorkEventArgs args)
			{
				object[] data = args.Argument as object[];
				int resultCode = 0;

				IServicesService _servicesService = ObjectLocator.GetInstance<IServicesService>();
				bool result;

				try
				{
					result = _servicesService.InitializeService(_service);
				}
				catch (System.ServiceModel.EndpointNotFoundException enf)
				{
					resultCode = 50;
					result = false;
				}
				catch
				{
					throw;
				}

				if (!result)
					resultCode = 10;

				bool testResult = false;

				try
				{
					_service.Initialized = true;
					_servicesService.SaveService(_service);
					testResult = _servicesService.TestService(_service);
				}
				catch (System.ServiceModel.EndpointNotFoundException enf)
				{
					resultCode = 50;
					result = false;
				}
				catch
				{
					throw;
				}

				if (!testResult)
				{
					resultCode = 20;
				}
				else
				{
					_service.Tested = true;
					_servicesService.SaveService(_service);
				}

				args.Result = resultCode;
			};

			worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
			{
				int resultCode = (int)args.Result;

				if (resultCode == 50)
				{
					MessageBox.Show("Cannot locate one or more of the services at the supplied urls. Please check the urls and try again.");
				}
				else if (resultCode == 20)
				{
					MessageBox.Show("Failed to test the service.");
				}
				else if (resultCode == 10)
				{
					MessageBox.Show("Failed to initialize the service.");
				}
				else
				{
					MessageBox.Show("Service has successfully been initialized and tested.");
				}

				IEventAggregator eventAggregator = ObjectLocator.GetInstance<IEventAggregator>();
				eventAggregator.SendMessage<ServicesUpdatedEvent>();


			};

			worker.RunWorkerAsync(new object[]
				                      	{
				                      		_service
				                      	});
		}

		private void btnTestOnly_Click(object sender, RoutedEventArgs e)
		{
			lblInitializingService.Text = "Skipped";
			lblInitializingService.Foreground = new SolidColorBrush(Colors.LightSlateGray);

			lblVerifyingServiceInitializion.Text = "Skipped";
			lblVerifyingServiceInitializion.Foreground = new SolidColorBrush(Colors.LightSlateGray);
		}
	}
}
