using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Events;
using WaveTech.Scutex.Model.Interfaces.Framework;
using WaveTech.Scutex.Model.Interfaces.Services;
using MessageBox = System.Windows.MessageBox;

namespace WaveTech.Scutex.Manager.Windows
{
	public partial class ServiceInitializationWindow : Window
	{
		private IEventAggregator _eventAggregator;
		private Service _service;
		private int progress;
		private bool _testOnly;
		private Storyboard _story;
		private IServicesService _servicesService;

		public ServiceInitializationWindow()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);

			_eventAggregator = ObjectLocator.GetInstance<IEventAggregator>();
			_eventAggregator.AddListener<ServiceTestingEvent>(x => ServiceTestingAndInitialization(x));

			_servicesService = ObjectLocator.GetInstance<IServicesService>();
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
			_testOnly = false;
			_eventAggregator.SendMessage<ServiceTestingEvent>();
		}

		private void ServiceTestingAndInitialization(ServiceTestingEvent e)
		{
			if (_testOnly)
			{
				lblInitializingService.Text = "Skipped";
				lblInitializingService.Foreground = new SolidColorBrush(Colors.LightSlateGray);

				lblVerifyingServiceInitializion.Text = "Skipped";
				lblVerifyingServiceInitializion.Foreground = new SolidColorBrush(Colors.LightSlateGray);
			}

			switch (progress)
			{
				case 0:
					TestMgmtServiceUrl();
					break;
				case 1:
					TestClientServiceUrl();
					break;
				case 2:
					TestMgmtServiceFileSystem();
					break;
				case 3:
					TestClientServiceFileSystem();
					break;
				case 4:
					TestMgmtServiceDatabase();
					break;
				case 5:
					TestClientServiceDatabase();
					break;
				case 6:
					if (!_testOnly)
						InitializeService();
					break;
				case 7:
					if (!_testOnly)
						TestService();
					break;
			}
		}

		private void btnTestOnly_Click(object sender, RoutedEventArgs e)
		{
			_testOnly = true;
			_eventAggregator.SendMessage<ServiceTestingEvent>();
		}

		#region Service Testing Methods
		private void TestMgmtServiceUrl()
		{
			BackgroundWorker worker = new BackgroundWorker();
			WorkingAnimation(lblMgmtServiceUrlCheck);

			worker.DoWork += delegate(object s, DoWorkEventArgs args)
			{
				object[] data = args.Argument as object[];
				bool result = false;

				try
				{
					result = _servicesService.TestManagementServiceUrl(_service);
				}
				catch
				{
					result = false;
				}

				args.Result = result;
			};

			worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
			{
				bool result = (bool)args.Result;
				progress++;

				_story.Stop(this);
				_story = null;

				if (result)
				{
					lblMgmtServiceUrlCheck.Foreground = new SolidColorBrush(Colors.DarkGreen);
					lblMgmtServiceUrlCheck.Text = "Success";
					lblMgmtServiceUrlCheck.FontWeight = FontWeights.Bold;

					imgMgmtServiceUrlCheck.Source = new BitmapImage(new Uri(@"pack://application:,,/img/Service_Test_Pass.png", UriKind.RelativeOrAbsolute));
					imgMgmtServiceUrlCheck.Cursor = Cursors.Arrow;
					imgMgmtServiceUrlCheck.MouseUp -= imgMgmtServiceUrlCheck_MouseUp;
				}
				else
				{
					lblMgmtServiceUrlCheck.Foreground = new SolidColorBrush(Colors.Red);
					lblMgmtServiceUrlCheck.Text = "Failure";
					lblMgmtServiceUrlCheck.FontWeight = FontWeights.Bold;

					imgMgmtServiceUrlCheck.Source = new BitmapImage(new Uri(@"pack://application:,,/img/Service_Test_Failure.png", UriKind.RelativeOrAbsolute));
					imgMgmtServiceUrlCheck.Cursor = Cursors.Hand;
					imgMgmtServiceUrlCheck.MouseUp += imgMgmtServiceUrlCheck_MouseUp;
				}

				_eventAggregator.SendMessage<ServiceTestingEvent>();
			};

			worker.RunWorkerAsync(new object[]
			                          {
			                            _service
			                          });
		}

		private void TestClientServiceUrl()
		{
			BackgroundWorker worker = new BackgroundWorker();
			WorkingAnimation(lblClientServiceUrlCheck);

			worker.DoWork += delegate(object s, DoWorkEventArgs args)
			{
				object[] data = args.Argument as object[];
				bool result = false;

				try
				{
					result = _servicesService.TestClientServiceUrl(_service);
				}
				catch
				{
					result = false;
				}

				args.Result = result;
			};

			worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
			{
				bool result = (bool)args.Result;
				progress++;

				_story.Stop(this);
				_story = null;

				if (result)
				{
					lblClientServiceUrlCheck.Foreground = new SolidColorBrush(Colors.DarkGreen);
					lblClientServiceUrlCheck.Text = "Success";
					lblClientServiceUrlCheck.FontWeight = FontWeights.Bold;

					imgClientServiceUrlCheck.Source = new BitmapImage(new Uri(@"pack://application:,,/img/Service_Test_Pass.png", UriKind.RelativeOrAbsolute));
					imgClientServiceUrlCheck.Cursor = Cursors.Arrow;
					imgClientServiceUrlCheck.MouseUp -= imgClientServiceUrlCheck_MouseUp;
				}
				else
				{
					lblClientServiceUrlCheck.Foreground = new SolidColorBrush(Colors.Red);
					lblClientServiceUrlCheck.Text = "Failure";
					lblClientServiceUrlCheck.FontWeight = FontWeights.Bold;

					imgClientServiceUrlCheck.Source = new BitmapImage(new Uri(@"pack://application:,,/img/Service_Test_Failure.png", UriKind.RelativeOrAbsolute));
					imgClientServiceUrlCheck.Cursor = Cursors.Hand;
					imgClientServiceUrlCheck.MouseUp -= imgClientServiceUrlCheck_MouseUp;
				}

				_eventAggregator.SendMessage<ServiceTestingEvent>();
			};

			worker.RunWorkerAsync(new object[]
			                          {
			                            _service
			                          });
		}

		private void TestMgmtServiceFileSystem()
		{
			BackgroundWorker worker = new BackgroundWorker();
			WorkingAnimation(lblMgmtServiceFileCheck);

			worker.DoWork += delegate(object s, DoWorkEventArgs args)
			{
				object[] data = args.Argument as object[];
				bool result = false;

				try
				{
					result = _servicesService.TestManagementServiceFileSystem(_service);
				}
				catch
				{
					result = false;
				}

				args.Result = result;
			};

			worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
			{
				bool result = (bool)args.Result;
				progress++;

				_story.Stop(this);
				_story = null;

				if (result)
				{
					lblMgmtServiceFileCheck.Foreground = new SolidColorBrush(Colors.DarkGreen);
					lblMgmtServiceFileCheck.Text = "Success";
					lblMgmtServiceFileCheck.FontWeight = FontWeights.Bold;

					imgMgmtServiceFileCheck.Source = new BitmapImage(new Uri(@"pack://application:,,/img/Service_Test_Pass.png", UriKind.RelativeOrAbsolute));
					imgMgmtServiceFileCheck.Cursor = Cursors.Arrow;
					imgMgmtServiceFileCheck.MouseUp -= imgMgmtServiceFileCheck_MouseUp;
				}
				else
				{
					lblMgmtServiceFileCheck.Foreground = new SolidColorBrush(Colors.Red);
					lblMgmtServiceFileCheck.Text = "Failure";
					lblMgmtServiceFileCheck.FontWeight = FontWeights.Bold;

					imgMgmtServiceFileCheck.Source = new BitmapImage(new Uri(@"pack://application:,,/img/Service_Test_Failure.png", UriKind.RelativeOrAbsolute));
					imgMgmtServiceFileCheck.Cursor = Cursors.Hand;
					imgMgmtServiceFileCheck.MouseUp += imgMgmtServiceFileCheck_MouseUp;
				}

				_eventAggregator.SendMessage<ServiceTestingEvent>();
			};

			worker.RunWorkerAsync(new object[]
			                          {
			                            _service
			                          });
		}

		private void TestClientServiceFileSystem()
		{
			BackgroundWorker worker = new BackgroundWorker();
			WorkingAnimation(lblClientServiceFileCheck);

			worker.DoWork += delegate(object s, DoWorkEventArgs args)
			{
				object[] data = args.Argument as object[];
				bool result = false;

				try
				{
					result = _servicesService.TestClientServiceFileSystem(_service);
				}
				catch
				{
					result = false;
				}

				args.Result = result;
			};

			worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
			{
				bool result = (bool)args.Result;
				progress++;

				_story.Stop(this);
				_story = null;

				if (result)
				{
					lblClientServiceFileCheck.Foreground = new SolidColorBrush(Colors.DarkGreen);
					lblClientServiceFileCheck.Text = "Success";
					lblClientServiceFileCheck.FontWeight = FontWeights.Bold;

					imgClientServiceFileCheck.Source = new BitmapImage(new Uri(@"pack://application:,,/img/Service_Test_Pass.png", UriKind.RelativeOrAbsolute));
					imgClientServiceFileCheck.Cursor = Cursors.Arrow;
					imgClientServiceFileCheck.MouseUp -= imgClientServiceFileCheck_MouseUp;
				}
				else
				{
					lblClientServiceFileCheck.Foreground = new SolidColorBrush(Colors.Red);
					lblClientServiceFileCheck.Text = "Failure";
					lblClientServiceFileCheck.FontWeight = FontWeights.Bold;

					imgClientServiceFileCheck.Source = new BitmapImage(new Uri(@"pack://application:,,/img/Service_Test_Failure.png", UriKind.RelativeOrAbsolute));
					imgClientServiceFileCheck.Cursor = Cursors.Hand;
					imgClientServiceFileCheck.MouseUp += imgClientServiceFileCheck_MouseUp;
				}

				_eventAggregator.SendMessage<ServiceTestingEvent>();
			};

			worker.RunWorkerAsync(new object[]
			                          {
			                            _service
			                          });
		}

		private void TestMgmtServiceDatabase()
		{
			BackgroundWorker worker = new BackgroundWorker();
			WorkingAnimation(lblMgmtServiceDbCheck);

			worker.DoWork += delegate(object s, DoWorkEventArgs args)
			{
				object[] data = args.Argument as object[];
				bool result = false;

				try
				{
					result = _servicesService.TestManagementServiceDatabase(_service);
				}
				catch
				{
					result = false;
				}

				args.Result = result;
			};

			worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
			{
				bool result = (bool)args.Result;
				progress++;

				_story.Stop(this);
				_story = null;

				if (result)
				{
					lblMgmtServiceDbCheck.Foreground = new SolidColorBrush(Colors.DarkGreen);
					lblMgmtServiceDbCheck.Text = "Success";
					lblMgmtServiceDbCheck.FontWeight = FontWeights.Bold;

					imgMgmtServiceDbCheck.Source = new BitmapImage(new Uri(@"pack://application:,,/img/Service_Test_Pass.png", UriKind.RelativeOrAbsolute));
					imgMgmtServiceDbCheck.Cursor = Cursors.Arrow;
					imgMgmtServiceDbCheck.MouseUp -= imgMgmtServiceDbCheck_MouseUp;
				}
				else
				{
					var service = _servicesService.GetServiceById(_service.ServiceId);

					if (service.Initialized)
					{
						lblMgmtServiceDbCheck.Foreground = new SolidColorBrush(Colors.Orange);
						lblMgmtServiceDbCheck.Text = "Skipped";
						lblMgmtServiceDbCheck.FontWeight = FontWeights.Bold;

						imgMgmtServiceDbCheck.Source = new BitmapImage(new Uri(@"pack://application:,,/img/Service_Test_Skip.png", UriKind.RelativeOrAbsolute));
						imgMgmtServiceDbCheck.Cursor = Cursors.Hand;
						imgMgmtServiceDbCheck.MouseUp += imgAlreadyInitialized_MouseUp;
					}
					else
					{
						lblMgmtServiceDbCheck.Foreground = new SolidColorBrush(Colors.Red);
						lblMgmtServiceDbCheck.Text = "Failure";
						lblMgmtServiceDbCheck.FontWeight = FontWeights.Bold;

						imgMgmtServiceDbCheck.Source = new BitmapImage(new Uri(@"pack://application:,,/img/Service_Test_Failure.png", UriKind.RelativeOrAbsolute));
						imgMgmtServiceDbCheck.Cursor = Cursors.Hand;
						imgMgmtServiceDbCheck.MouseUp += imgMgmtServiceDbCheck_MouseUp;
					}

				}

				_eventAggregator.SendMessage<ServiceTestingEvent>();
			};

			worker.RunWorkerAsync(new object[]
			                          {
			                            _service
			                          });
		}

		private void TestClientServiceDatabase()
		{
			BackgroundWorker worker = new BackgroundWorker();
			WorkingAnimation(lblClientServiceDbCheck);

			worker.DoWork += delegate(object s, DoWorkEventArgs args)
			{
				object[] data = args.Argument as object[];
				bool result = false;

				try
				{
					result = _servicesService.TestManagementServiceDatabase(_service);
				}
				catch
				{
					result = false;
				}

				args.Result = result;
			};

			worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
			{
				bool result = (bool)args.Result;
				progress++;

				_story.Stop(this);
				_story = null;

				if (result)
				{
					lblClientServiceDbCheck.Foreground = new SolidColorBrush(Colors.DarkGreen);
					lblClientServiceDbCheck.Text = "Success";
					lblClientServiceDbCheck.FontWeight = FontWeights.Bold;

					imgClientServiceDbCheck.Source = new BitmapImage(new Uri(@"pack://application:,,/img/Service_Test_Pass.png", UriKind.RelativeOrAbsolute));
					imgClientServiceDbCheck.Cursor = Cursors.Arrow;
					imgClientServiceDbCheck.MouseUp -= imgClientServiceDbCheck_MouseUp;
				}
				else
				{
					var service = _servicesService.GetServiceById(_service.ServiceId);

					if (service.Initialized)
					{
						lblClientServiceDbCheck.Foreground = new SolidColorBrush(Colors.Orange);
						lblClientServiceDbCheck.Text = "Skipped";
						lblClientServiceDbCheck.FontWeight = FontWeights.Bold;

						imgClientServiceDbCheck.Source = new BitmapImage(new Uri(@"pack://application:,,/img/Service_Test_Skip.png", UriKind.RelativeOrAbsolute));
						imgClientServiceDbCheck.Cursor = Cursors.Hand;
						imgClientServiceDbCheck.MouseUp += imgAlreadyInitialized_MouseUp;
					}
					else
					{
						lblClientServiceDbCheck.Foreground = new SolidColorBrush(Colors.Red);
						lblClientServiceDbCheck.Text = "Failure";
						lblClientServiceDbCheck.FontWeight = FontWeights.Bold;

						imgClientServiceDbCheck.Source =
							new BitmapImage(new Uri(@"pack://application:,,/img/Service_Test_Failure.png", UriKind.RelativeOrAbsolute));
						imgClientServiceDbCheck.Cursor = Cursors.Hand;
						imgClientServiceDbCheck.MouseUp += imgClientServiceDbCheck_MouseUp;
					}
				}

				_eventAggregator.SendMessage<ServiceTestingEvent>();
			};

			worker.RunWorkerAsync(new object[]
			                          {
			                            _service
			                          });
		}
		#endregion Service Testing Methods

		#region Service Initialization Methods
		private void InitializeService()
		{
			if (_testOnly)
			{
				progress++;

				_story.Stop(this);
				_story = null;

				return;
			}

			BackgroundWorker worker = new BackgroundWorker();
			WorkingAnimation(lblInitializingService);

			worker.DoWork += delegate(object s, DoWorkEventArgs args)
			{
				object[] data = args.Argument as object[];
				int resultCode = 0;

				IServicesService _servicesService = ObjectLocator.GetInstance<IServicesService>();
				bool result = false;

				try
				{
					var service = _servicesService.GetServiceById(_service.ServiceId);

					if (!service.Initialized)
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

				try
				{
					var service = _servicesService.GetServiceById(_service.ServiceId);

					if (!service.Initialized)
					{
						_service.Initialized = true;
						_servicesService.SaveService(_service);
					}
					else
					{
						resultCode = 500;
					}
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

				args.Result = resultCode;
			};

			worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
			{
				int resultCode = (int)args.Result;
				bool failed = false;

				if (resultCode == 50)
				{
					failed = true;
					MessageBox.Show("Cannot locate one or more of the services at the supplied urls. Please check the urls and try again.");
				}
				else if (resultCode == 10)
				{
					failed = true;
					MessageBox.Show("Failed to initialize the service.");
				}
				else if (resultCode == 500)
				{
					lblInitializingService.Foreground = new SolidColorBrush(Colors.Orange);
					lblInitializingService.Text = "Skipped";
					lblInitializingService.FontWeight = FontWeights.Bold;

					imgInitializingService.Source = new BitmapImage(new Uri(@"pack://application:,,/img/Service_Test_Skip.png", UriKind.RelativeOrAbsolute));
					imgInitializingService.Cursor = Cursors.Hand;
					imgInitializingService.MouseUp += imgAlreadyInitialized_MouseUp;
				}
				else
				{
					lblInitializingService.Foreground = new SolidColorBrush(Colors.DarkGreen);
					lblInitializingService.Text = "Success";
					lblInitializingService.FontWeight = FontWeights.Bold;

					imgInitializingService.Source = new BitmapImage(new Uri(@"pack://application:,,/img/Service_Test_Pass.png", UriKind.RelativeOrAbsolute));
					imgInitializingService.Cursor = Cursors.Arrow;
					imgInitializingService.MouseUp -= imgInitializingService_MouseUp;
				}

				if (failed)
				{
					lblInitializingService.Foreground = new SolidColorBrush(Colors.Red);
					lblInitializingService.Text = "Failure";
					lblInitializingService.FontWeight = FontWeights.Bold;

					imgInitializingService.Source = new BitmapImage(new Uri(@"pack://application:,,/img/Service_Test_Failure.png", UriKind.RelativeOrAbsolute));
					imgInitializingService.Cursor = Cursors.Hand;
					imgInitializingService.MouseUp += imgInitializingService_MouseUp;
				}

				progress++;

				_story.Stop(this);
				_story = null;

				_eventAggregator.SendMessage<ServiceTestingEvent>();

				IEventAggregator eventAggregator = ObjectLocator.GetInstance<IEventAggregator>();
				eventAggregator.SendMessage<ServicesUpdatedEvent>();
			};

			worker.RunWorkerAsync(new object[]
				                      	{
				                      		_service
				                      	});
		}

		private void TestService()
		{
			if (_testOnly)
			{
				progress++;

				_story.Stop(this);
				_story = null;

				return;
			}

			BackgroundWorker worker = new BackgroundWorker();
			WorkingAnimation(lblVerifyingServiceInitializion);

			worker.DoWork += delegate(object s, DoWorkEventArgs args)
			{
				object[] data = args.Argument as object[];
				IServicesService _servicesService = ObjectLocator.GetInstance<IServicesService>();
				bool result;

				try
				{
					result = _servicesService.TestService(_service);
				}
				catch (Exception ex)
				{
					result = false;
				}


				if (result)
				{
					_service.Tested = true;
					_servicesService.SaveService(_service);
				}

				args.Result = result;
			};

			worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
			{
				bool result = (bool)args.Result;

				if (result)
				{
					lblVerifyingServiceInitializion.Foreground = new SolidColorBrush(Colors.DarkGreen);
					lblVerifyingServiceInitializion.Text = "Success";
					lblVerifyingServiceInitializion.FontWeight = FontWeights.Bold;

					imgVerifyingServiceInitializion.Source = new BitmapImage(new Uri(@"pack://application:,,/img/Service_Test_Pass.png", UriKind.RelativeOrAbsolute));
					imgVerifyingServiceInitializion.Cursor = Cursors.Arrow;
					imgVerifyingServiceInitializion.MouseUp -= imgVerifyingServiceInitializion_MouseUp;
				}
				else
				{
					lblVerifyingServiceInitializion.Foreground = new SolidColorBrush(Colors.Red);
					lblVerifyingServiceInitializion.Text = "Failure";
					lblVerifyingServiceInitializion.FontWeight = FontWeights.Bold;

					imgVerifyingServiceInitializion.Source = new BitmapImage(new Uri(@"pack://application:,,/img/Service_Test_Failure.png", UriKind.RelativeOrAbsolute));
					imgVerifyingServiceInitializion.Cursor = Cursors.Hand;
					imgVerifyingServiceInitializion.MouseUp += imgVerifyingServiceInitializion_MouseUp;
				}

				progress++;

				_story.Stop(this);
				_story = null;

				_eventAggregator.SendMessage<ServiceTestingEvent>();

				IEventAggregator eventAggregator = ObjectLocator.GetInstance<IEventAggregator>();
				eventAggregator.SendMessage<ServicesUpdatedEvent>();
			};

			worker.RunWorkerAsync(new object[]
				                      	{
				                      		_service
				                      	});
		}
		#endregion Service Initialization Methods

		private void WorkingAnimation(TextBlock control)
		{
			if (_story != null)
				_story.Stop(this);

			_story = new Storyboard();
			_story.FillBehavior = FillBehavior.HoldEnd;
			_story.RepeatBehavior = RepeatBehavior.Forever;

			StringAnimationUsingKeyFrames stringAnimation = new StringAnimationUsingKeyFrames();
			stringAnimation.Duration = TimeSpan.FromSeconds(2);
			stringAnimation.FillBehavior = FillBehavior.Stop;
			stringAnimation.RepeatBehavior = RepeatBehavior.Forever;
			Storyboard.SetTargetName(stringAnimation, control.Name);
			Storyboard.SetTargetProperty(stringAnimation, new PropertyPath(TextBlock.TextProperty));

			DiscreteStringKeyFrame kf1 = new DiscreteStringKeyFrame("Working", TimeSpan.FromSeconds(0));
			DiscreteStringKeyFrame kf2 = new DiscreteStringKeyFrame(".Working.", TimeSpan.FromSeconds(0.5));
			DiscreteStringKeyFrame kf3 = new DiscreteStringKeyFrame("..Working..", TimeSpan.FromSeconds(1));
			DiscreteStringKeyFrame kf4 = new DiscreteStringKeyFrame("...Working...", TimeSpan.FromSeconds(1.5));

			stringAnimation.KeyFrames.Add(kf1);
			stringAnimation.KeyFrames.Add(kf2);
			stringAnimation.KeyFrames.Add(kf3);
			stringAnimation.KeyFrames.Add(kf4);

			_story.Children.Add(stringAnimation);

			DoubleAnimation doubleAnimation = new DoubleAnimation(-2.5, 3.5, TimeSpan.FromSeconds(5.4));
			Storyboard.SetTargetName(doubleAnimation, control.Name + "_gs2");
			Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(GradientStop.OffsetProperty));
			doubleAnimation.RepeatBehavior = RepeatBehavior.Forever;

			_story.Children.Add(doubleAnimation);
			_story.Begin(this, true);
		}

		#region Error Help Message Displayers
		private void imgMgmtServiceUrlCheck_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			MessageBox.Show(string.Format("Scutex was unable to contact the Management service at the following Url ({0}). Please try the following to ensure the service is working:" + Environment.NewLine +
																		"\t 1.) Check to ensure that the Url is correct" + Environment.NewLine +
																		"\t 2.) Ensure the path maps to the correct directory" + Environment.NewLine +
																		"\t 3.) Check that the .svc files are accessible" + Environment.NewLine +
																		"\t 4.) Check that .Net 4 is installed on the web server" + Environment.NewLine +
																		"\t 5.) Check that WCF is enabled on the web server", _service.ManagementUrl));
		}

		private void imgClientServiceUrlCheck_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			MessageBox.Show(string.Format("Scutex was unable to contact the Client service at the following Url ({0}). Please try the following to ensure the service is working:" + Environment.NewLine +
																		"\t 1.) Check to ensure that the Url is correct" + Environment.NewLine +
																		"\t 2.) Ensure the path maps to the correct directory" + Environment.NewLine +
																		"\t 3.) Check that the .svc files are accessible" + Environment.NewLine +
																		"\t 4.) Check that .Net 4 is installed on the web server" + Environment.NewLine +
																		"\t 5.) Check that WCF is enabled on the web server", _service.ClientUrl));
		}

		private void imgMgmtServiceFileCheck_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			MessageBox.Show(string.Format("Scutex was unable to verify Read/Write ability on the Management service at Url ({0}). Please try the following to ensure the service is working:" + Environment.NewLine +
																		"\t 1.) Verify that the worker process running the WCF services is a normal windows account" + Environment.NewLine +
																		"\t 2.) Ensure the Windows account running the worker process has access to the file system" + Environment.NewLine +
																		"\t 3.) Check that account has read/write/delete at the directory level and below", _service.ManagementUrl));
		}

		private void imgClientServiceFileCheck_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			MessageBox.Show(string.Format("Scutex was unable to verify Read/Write ability on the Client service at Url ({0}). Please try the following to ensure the service is working:" + Environment.NewLine +
																		"\t 1.) Verify that the worker process running the WCF services is a normal windows account" + Environment.NewLine +
																		"\t 2.) Ensure the Windows account running the worker process has access to the file system" + Environment.NewLine +
																		"\t 3.) Check that account has read/write/delete at the directory level and below", _service.ClientUrl));
		}

		private void imgMgmtServiceDbCheck_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			MessageBox.Show(string.Format("Scutex was unable to verify database connectivity on the Management service at Url ({0}). Please try the following to ensure the service is working:" + Environment.NewLine +
																		"\t 1.) Verify that the database has been created." + Environment.NewLine +
																		"\t 2.) Verify that the tables exists" + Environment.NewLine +
																		"\t 3.) Ensure that the connection string has been set in the web.config" + Environment.NewLine +
																		"\t 4.) Verify that account used to connect to the database has db_owner", _service.ClientUrl));
		}

		private void imgClientServiceDbCheck_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			MessageBox.Show(string.Format("Scutex was unable to verify database connectivity on the Client service at Url ({0}). Please try the following to ensure the service is working:" + Environment.NewLine +
																		"\t 1.) Verify that the database has been created." + Environment.NewLine +
																		"\t 2.) Verify that the tables exists" + Environment.NewLine +
																		"\t 3.) Ensure that the connection string has been set in the web.config" + Environment.NewLine +
																		"\t 4.) Verify that account used to connect to the database has db_owner", _service.ClientUrl));
		}

		private void imgInitializingService_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			MessageBox.Show(string.Format("Scutex was unable to verify database connectivity on the Client service at Url ({0}). Please try the following to ensure the service is working:" + Environment.NewLine +
																		"\t 1.) Verify that the database has been created." + Environment.NewLine +
																		"\t 2.) Verify that the tables exists" + Environment.NewLine +
																		"\t 3.) Ensure that the connection string has been set in the web.config" + Environment.NewLine +
																		"\t 4.) Verify that account used to connect to the database has db_owner", _service.ClientUrl));
		}

		private void imgVerifyingServiceInitializion_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			MessageBox.Show(string.Format("Scutex was unable to verify database connectivity on the Client service at Url ({0}). Please try the following to ensure the service is working:" + Environment.NewLine +
																		"\t 1.) Verify that the database has been created." + Environment.NewLine +
																		"\t 2.) Verify that the tables exists" + Environment.NewLine +
																		"\t 3.) Ensure that the connection string has been set in the web.config" + Environment.NewLine +
																		"\t 4.) Verify that account used to connect to the database has db_owner", _service.ClientUrl));
		}

		private void imgAlreadyInitialized_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			MessageBox.Show(string.Format("Scutex was unable to perform this task as the service is already initialized."));
		}
		#endregion Error Help Message Displayers
	}
}
