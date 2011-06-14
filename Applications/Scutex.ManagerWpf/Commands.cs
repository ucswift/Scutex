using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Manager.Forms;
using WaveTech.Scutex.Manager.Windows;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Events;
using WaveTech.Scutex.Model.Interfaces.Framework;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Manager
{
	public class Commands
	{
		#region Command Routers
		public static readonly RoutedUICommand SaveCommand = new RoutedUICommand("Save", "SaveCommand", typeof(MainWindow),
				new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.S, ModifierKeys.Control, "Ctrl+S") }));

		public static readonly RoutedUICommand NewCommand = new RoutedUICommand("New", "NewCommand", typeof(MainWindow),
				new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.N, ModifierKeys.Control, "Ctrl+N") }));

		public static readonly RoutedUICommand OpenCommand = new RoutedUICommand("Open", "OpenCommand", typeof(MainWindow),
				new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.O, ModifierKeys.Control, "Ctrl+O") }));

		public static readonly RoutedUICommand NewServiceCommand = new RoutedUICommand("NewService", "NewServiceCommand", typeof(MainWindow),
				new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.T, ModifierKeys.Control, "Ctrl+T") }));

		public static readonly RoutedUICommand RemoveServiceCommand = new RoutedUICommand("RemoveService", "RemoveServiceCommand", typeof(MainWindow),
				new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.R, ModifierKeys.Control, "Ctrl+R") }));

		public static readonly RoutedUICommand DownloadLogsCommand = new RoutedUICommand("DownloadLogs", "DownloadLogsCommand", typeof(MainWindow),
				new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.E, ModifierKeys.Control, "Ctrl+E") }));

		public static readonly RoutedUICommand UploadKeysCommand = new RoutedUICommand("UploadKeys", "UploadKeysCommand", typeof(MainWindow),
				new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.U, ModifierKeys.Control, "Ctrl+UK") }));

		public static readonly RoutedUICommand HomeCommand = new RoutedUICommand("Home", "HomeCommand", typeof(MainWindow),
				new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.E, ModifierKeys.Control, "Ctrl+H") }));

		public static readonly RoutedUICommand UploadProductCommand = new RoutedUICommand("UploadProduct", "UploadProductCommand", typeof(MainWindow),
				new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.E, ModifierKeys.Control, "Ctrl+R") }));

		public static readonly RoutedUICommand ValidateProjectCommand = new RoutedUICommand("ValidateProject", "ValidateProjectCommand", typeof(MainWindow),
		new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.E, ModifierKeys.Control, "Ctrl+V") }));

		public static readonly RoutedUICommand ShowServiceLogsCommand = new RoutedUICommand("ShowServiceLogs", "ShowServiceLogsCommand", typeof(MainWindow),
				new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.E, ModifierKeys.Control, "Ctrl+SE") }));

		public static readonly RoutedUICommand RefreshCommand = new RoutedUICommand("Refresh", "RefreshCommand", typeof(MainWindow),
				new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.E, ModifierKeys.Control, "Ctrl+U") }));

		public static readonly RoutedUICommand SoftwareCodeCommand = new RoutedUICommand("SoftwareCode", "SoftwareCodeCommand", typeof(MainWindow),
				new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.E, ModifierKeys.Control, "Ctrl+SC") }));

		public static readonly RoutedUICommand HelpCommand = new RoutedUICommand("Help", "HelpCommand", typeof(MainWindow),
				new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.E, ModifierKeys.Control, "Ctrl+H") }));

		public static readonly RoutedUICommand AboutCommand = new RoutedUICommand("About", "AboutCommand", typeof(MainWindow),
				new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.E, ModifierKeys.Control, "Ctrl+A") }));

		public static readonly RoutedUICommand GenerateKeysCommand = new RoutedUICommand("Generate", "GenerateKeysCommand", typeof(MainWindow),
				new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.E, ModifierKeys.Control, "Ctrl+G") }));

		public static readonly RoutedUICommand ProductsCommand = new RoutedUICommand("Products", "ProductsCommand", typeof(MainWindow),
		new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.E, ModifierKeys.Control, "Ctrl+P") }));

		public static readonly RoutedUICommand CloseProjectCommand = new RoutedUICommand("CloseProject", "CloseProjectCommand", typeof(MainWindow),
		new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.E, ModifierKeys.Control, "Ctrl+C") }));

		public static readonly RoutedUICommand ViewServicesCommand = new RoutedUICommand("ViewServices", "ViewServicesCommand", typeof(MainWindow),
		new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.E, ModifierKeys.Control, "Ctrl+VS") }));

		public static readonly RoutedUICommand DemoCommand = new RoutedUICommand("Demo", "DemoCommand", typeof(MainWindow),
		new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.E, ModifierKeys.Control, "Ctrl+D") }));

		public static readonly RoutedUICommand NewProductsCommand = new RoutedUICommand("NewProducts", "NewProductsCommand", typeof(MainWindow),
				new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.T, ModifierKeys.Control, "Ctrl+NP") }));

		public static readonly RoutedUICommand EditProductCommand = new RoutedUICommand("EditProduct", "EditProductCommand", typeof(MainWindow),
				new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.T, ModifierKeys.Control, "Ctrl+PE") }));

		public static readonly RoutedUICommand RemoveProductCommand = new RoutedUICommand("RemoveProduct", "RemoveProductCommand", typeof(MainWindow),
				new InputGestureCollection(new KeyGesture[] { new KeyGesture(Key.T, ModifierKeys.Control, "Ctrl+PD") }));
		#endregion Command Routers

		#region Windows
		private static ProductsWindow _productsWindow;
		private static GenerationWindow _generationWindow;
		private static CodeWindow _codeWindow;
		private static OpenProjectWindow _openProjectWindow;
		private static NewServiceWindow _newServiceWindow;
		private static UploadProductsWindow _uploadProductsWindow;
		private static UploadKeysWindow _upoloadKeysWindow;
		private static ServiceLogWindow _serviceLogWindow;
		private static ViewServicesWindow _viewServicesWindow;
		private static NewProductWindow _newProductWindow;
		#endregion Windows

		#region Constructor
		static Commands()
		{
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(SaveCommand, SaveProject));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(NewCommand, NewProject));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(OpenCommand, OpenProject));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(NewServiceCommand, NewService));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(RemoveServiceCommand, RemoveService));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(DownloadLogsCommand, DownloadLogs));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(UploadKeysCommand, UploadKeys));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(HomeCommand, Home));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(UploadProductCommand, UploadProduct));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(ValidateProjectCommand, ValidateProject));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(ShowServiceLogsCommand, ShowServiceLogs));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(RefreshCommand, Refresh));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(SoftwareCodeCommand, SoftwareCode));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(HelpCommand, Help));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(AboutCommand, About));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(GenerateKeysCommand, GenerateKeys));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(ProductsCommand, ShowProducts));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(CloseProjectCommand, CloseProject));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(ViewServicesCommand, ViewServices));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(DemoCommand, Demo));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(NewProductsCommand, NewProduct));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(EditProductCommand, EditProduct));
			CommandManager.RegisterClassCommandBinding(typeof(MainWindow), new CommandBinding(RemoveProductCommand, RemoveProduct));
		}
		#endregion Constructor

		#region Private Event Handlers
		private static void SaveProject(object sender, ExecutedRoutedEventArgs e)
		{
			if (UIContext.License != null)
			{

				ILicenseService licenseService = ObjectLocator.GetInstance<ILicenseService>();
				IEventAggregator eventAggregator = ObjectLocator.GetInstance<IEventAggregator>();
				IValidationService validationService = ObjectLocator.GetInstance<IValidationService>();

				ValidationResult result = validationService.IsLicenseValidForSaving(UIContext.License);

				if (result.IsValid)
				{
					if (UIContext.License.LicenseId == 0 && licenseService.IsLicenseProjectNameInUse(UIContext.License.Name))
					{
						MessageBox.Show(string.Format("License project name [{0}] is already in use.", UIContext.License.Name));
						return;
					}

					UIContext.License = licenseService.SaveLicense(UIContext.License);
					eventAggregator.SendMessage<LicenseSavedEvent>();
				}
				else
				{
					StringBuilder sb = new StringBuilder();

					sb.Append("Your Licensing Project has the following errors: \r\n");
					sb.Append("\r\n");

					foreach (var v in result.ValidationErrors)
					{
						sb.Append(v + "\r\n");
					}

					MessageBox.Show(sb.ToString());
				}
			}
			else
			{
				MessageBox.Show("You must have an open licensing project to save.");
			}
		}

		private static void NewProject(object sender, ExecutedRoutedEventArgs e)
		{
			UIContext.SetNewLicense();

			MainWindow mainWindow = (MainWindow)sender;
			mainWindow.Initalize();
		}

		private static void OpenProject(object sender, ExecutedRoutedEventArgs e)
		{
			MainWindow mainWindow = (MainWindow)sender;

			if (_openProjectWindow == null)
			{
				_openProjectWindow = new OpenProjectWindow(mainWindow);
				_openProjectWindow.Show();
			}
			else
			{
				_openProjectWindow.Close();
				_openProjectWindow = new OpenProjectWindow(mainWindow);
				_openProjectWindow.Show();
			}
		}

		private static void CloseProject(object sender, ExecutedRoutedEventArgs e)
		{
			MainWindow mainWindow = (MainWindow)sender;
			mainWindow.root.Content = null;
			UIContext.License = null;

			WelcomeScreenForm welcomeScreenForm = new WelcomeScreenForm();
			mainWindow.root.Content = welcomeScreenForm;
		}

		private static void SoftwareCode(object sender, ExecutedRoutedEventArgs e)
		{
			MainWindow mainWindow = (MainWindow)sender;

			if (IsLicenseStateValid())
			{
				if (_codeWindow == null)
				{
					_codeWindow = new CodeWindow(mainWindow);
					_codeWindow.Show();
				}
				else
				{
					_codeWindow.Close();
					_codeWindow = new CodeWindow(mainWindow);
					_codeWindow.Show();
				}
			}
		}

		private static void NewService(object sender, ExecutedRoutedEventArgs e)
		{
			MainWindow mainWindow = (MainWindow)sender;

			if (_newServiceWindow == null)
			{
				_newServiceWindow = new NewServiceWindow(mainWindow);
				_newServiceWindow.Show();
			}
			else
			{
				_newServiceWindow.Close();
				_newServiceWindow = new NewServiceWindow(mainWindow);
				_newServiceWindow.Show();
			}
		}

		private static void RemoveService(object sender, ExecutedRoutedEventArgs e)
		{

		}

		private static void UploadProduct(object sender, ExecutedRoutedEventArgs e)
		{
			MainWindow mainWindow = (MainWindow)sender;

			if (_uploadProductsWindow == null)
			{
				_uploadProductsWindow = new UploadProductsWindow(mainWindow);
				_uploadProductsWindow.Show();
			}
			else
			{
				_uploadProductsWindow.Close();
				_uploadProductsWindow = new UploadProductsWindow(mainWindow);
				_uploadProductsWindow.Show();
			}
		}

		private static void DownloadLogs(object sender, ExecutedRoutedEventArgs e)
		{
			MainWindow mainWindow = (MainWindow)sender;

			if (_serviceLogWindow == null)
			{
				_serviceLogWindow = new ServiceLogWindow(mainWindow);
				_serviceLogWindow.Show();
			}
			else
			{
				_serviceLogWindow.Close();
				_serviceLogWindow = new ServiceLogWindow(mainWindow);
				_serviceLogWindow.Show();
			}
		}

		private static void UploadKeys(object sender, ExecutedRoutedEventArgs e)
		{
			MainWindow mainWindow = (MainWindow)sender;

			if (_upoloadKeysWindow == null)
			{
				_upoloadKeysWindow = new UploadKeysWindow(mainWindow);
				_upoloadKeysWindow.Show();
			}
			else
			{
				_upoloadKeysWindow.Close();
				_upoloadKeysWindow = new UploadKeysWindow(mainWindow);
				_upoloadKeysWindow.Show();
			}
		}


		private static void ValidateProject(object sender, ExecutedRoutedEventArgs e)
		{
			if (UIContext.License != null)
			{
				IValidationService validationService = ObjectLocator.GetInstance<IValidationService>();

				ValidationResult result = validationService.IsLicenseStateValid(UIContext.License);

				if (result.IsValid == false)
				{
					StringBuilder sb = new StringBuilder();

					sb.Append("Your Licensing Project has the following validation errors: \r\n");
					sb.Append("\r\n");

					foreach (var v in result.ValidationErrors)
					{
						sb.Append(v + "\r\n");
					}

					MessageBox.Show(sb.ToString());
				}
				else
				{
					MessageBox.Show("Your Licensing Project is valid!");
				}
			}
			else
			{
				MessageBox.Show("You must have an open licensing project to validate.");
			}
		}

		private static void ShowServiceLogs(object sender, ExecutedRoutedEventArgs e)
		{

		}

		private static void ViewServices(object sender, ExecutedRoutedEventArgs e)
		{
			MainWindow mainWindow = (MainWindow)sender;

			if (_viewServicesWindow == null)
			{
				_viewServicesWindow = new ViewServicesWindow(mainWindow);
				_viewServicesWindow.Show();
			}
			else
			{
				_viewServicesWindow.Close();
				_viewServicesWindow = new ViewServicesWindow(mainWindow);
				_viewServicesWindow.Show();
			}
		}

		private static void Home(object sender, ExecutedRoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.wtdt.com/Products/Scutex.aspx");
		}

		private static void About(object sender, ExecutedRoutedEventArgs e)
		{
			MainWindow mainWindow = (MainWindow)sender;

			AboutBox about = new AboutBox(mainWindow);
			about.ShowDialog();
		}

		private static void Help(object sender, ExecutedRoutedEventArgs e)
		{
			string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
			path = path.Replace("file:\\", "");

			System.Diagnostics.Process.Start(string.Format("{0}\\ScutexDocumentation.chm", path));
		}

		private static void GenerateKeys(object sender, ExecutedRoutedEventArgs e)
		{
			MainWindow mainWindow = (MainWindow)sender;

			if (IsLicenseStateValid())
			{
				if (_generationWindow == null)
				{
					_generationWindow = new GenerationWindow(mainWindow);
					_generationWindow.Show();
				}
				else
				{
					_generationWindow.Close();
					_generationWindow = new GenerationWindow(mainWindow);
					_generationWindow.Show();
				}
			}
		}

		private static void Refresh(object sender, ExecutedRoutedEventArgs e)
		{

		}

		private static void ShowProducts(object sender, ExecutedRoutedEventArgs e)
		{
			MainWindow mainWindow = (MainWindow)sender;

			if (_productsWindow == null)
			{
				_productsWindow = new ProductsWindow(mainWindow);
				_productsWindow.Show();
			}
			else
			{
				_productsWindow.Close();
				_productsWindow = new ProductsWindow(mainWindow);
				_productsWindow.Show();
			}
		}

		private static void Demo(object sender, ExecutedRoutedEventArgs e)
		{
			if (UIContext.License != null)
			{
				DemoHostHelper helper = new DemoHostHelper();
				helper.CleanPreviousHost();

				string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
				path = path.Replace("file:\\", "");

				ClientLicense cl = new ClientLicense(UIContext.License);

				IClientLicenseService clientLicenseService = ObjectLocator.GetInstance<IClientLicenseService>();
				clientLicenseService.SaveClientLicense(cl, path + @"\sxu.dll");

				IHashingProvider hashingProvider = ObjectLocator.GetInstance<IHashingProvider>();
				IEncodingService encodingService = ObjectLocator.GetInstance<IEncodingService>();

				string dllCheckHash = encodingService.Encode(hashingProvider.HashFile(Directory.GetCurrentDirectory() + "\\lib\\WaveTech.Scutex.Licensing.dll"));
				string publicKey = encodingService.Encode(UIContext.License.KeyPair.PublicKey);

				try
				{
					File.Copy(Directory.GetCurrentDirectory() + "\\lib\\WaveTech.Scutex.Licensing.dll", Directory.GetCurrentDirectory() + "\\WaveTech.Scutex.Licensing.dll");
				}
				catch { }

				helper.CreateAssembly(publicKey, dllCheckHash);

				helper.ExecuteAssembly();
				helper = null;
			}
			else
			{
				MessageBox.Show("You must have an open licensing project to view the demo trial form.");
			}
		}

		private static bool IsLicenseStateValid()
		{
			if (UIContext.License != null)
			{
				if (UIContext.License.KeyGeneratorType != KeyGeneratorTypes.None)
				{
					if (UIContext.License.LicenseSets.Count > 0)
					{
						if (UIContext.License.Product != null)
						{
							return true;
						}
						else
						{
							MessageBox.Show("You must select a product for your project.");
							return false;
						}
					}
					else
					{
						MessageBox.Show("You must have a least one license set defined.");
						return false;
					}
				}
				else
				{
					MessageBox.Show("You must select a key generator type for your project.");
					return false;
				}
			}
			else
			{
				MessageBox.Show("You must have an open licensing project to continue.");
				return false;
			}
		}

		#region Products Menu
		private static void NewProduct(object sender, ExecutedRoutedEventArgs e)
		{
			MainWindow mainWindow = (MainWindow)sender;

			if (_newProductWindow == null)
			{
				_newProductWindow = new NewProductWindow(mainWindow);
				_newProductWindow.Show();
			}
			else
			{
				_newProductWindow.Close();
				_newProductWindow = new NewProductWindow(mainWindow);
				_newProductWindow.Show();
			}
		}

		private static void EditProduct(object sender, ExecutedRoutedEventArgs e)
		{
			MainWindow mainWindow = (MainWindow)sender;

			if (mainWindow.ProductsScreen != null)
			{
				if (mainWindow.ProductsScreen.SelectedProduct != null)
				{
					
				}
				else
				{
					MessageBox.Show("You must select a product first.");
				}
			}
		}

		private static void RemoveProduct(object sender, ExecutedRoutedEventArgs e)
		{
			MainWindow mainWindow = (MainWindow)sender;

			if (mainWindow.ProductsScreen != null)
			{
				if (mainWindow.ProductsScreen.SelectedProduct != null)
				{

				}
				else
				{
					MessageBox.Show("You must select a product first.");
				}
			}
		}
		#endregion Products Menu
		#endregion Private Event Handlers
	}
}