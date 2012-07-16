using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Fluent;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Manager.Forms;
using WaveTech.Scutex.Manager.Screens;
using WaveTech.Scutex.Manager.Wizards;
using WaveTech.Scutex.Model.Events;
using WaveTech.Scutex.Model.Interfaces.Framework;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Services;
using License = WaveTech.Scutex.Model.License;

namespace WaveTech.Scutex.Manager
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : RibbonWindow
	{
		private IEventAggregator _eventAggregator;
		private static ProductsScreen _productsScreen;
		private static ServicesScreen _servicesScreen;

		public MainWindow()
		{
			InitializeComponent();

			ribbon.DataContext = this;
			DataContext = this;

			try
			{ // Apparently this doesn't work in anything but Windows7
				IconBitmapDecoder ibd = new IconBitmapDecoder(
				new Uri(@"pack://application:,,/Scutex.ico", UriKind.RelativeOrAbsolute),
				BitmapCreateOptions.None,
				BitmapCacheOption.Default);
				Icon = ibd.Frames[0];
			}
			catch
			{
				try
				{
					IconBitmapDecoder ibd = new IconBitmapDecoder(
					new Uri(@"pack://application:,,/Scutex3.ico", UriKind.RelativeOrAbsolute),
					BitmapCreateOptions.None,
					BitmapCacheOption.Default);
					Icon = ibd.Frames[0];
				}
				catch { }
			}

			try
			{
				Bootstrapper.Configure();

				_eventAggregator = ObjectLocator.GetInstance<IEventAggregator>();
				_eventAggregator.AddListener<ProductsUpdatedEvent>(x => RefreshData());
				_eventAggregator.AddListener<LicenseSavedEvent>(x => SetRecentItemsAndRefresh());
				_eventAggregator.AddListener<ServicesUpdatedEvent>(x => RefreshData());
			}
			catch { }

			Initalize();
			SetRecentItems();
			VerifyFirstTimeRun();

			WelcomeScreenForm welcomeScreenForm = new WelcomeScreenForm();
			root.Content = welcomeScreenForm;
		}

		public void Initalize()
		{
			RefreshData();

			root.Content = null;
			
			if (UIContext.License == null)
			{
				root.Content = null;
				ribbon.ContextualGroups[0].Visibility = Visibility.Collapsed;
				var tab = ribbon.Tabs.Where(x => x.Name == "projectTabItem").FirstOrDefault();
				tab.Visibility = Visibility.Collapsed;
				ribbon.SelectedTabItem = tab;
			}
			else
			{
				ribbon.ContextualGroups[0].Visibility = Visibility.Visible;
				var tab = ribbon.Tabs.Where(x => x.Name == "projectTabItem").FirstOrDefault();
				tab.Visibility = Visibility.Visible;
				ribbon.SelectedTabItem = tab;

				ProjectForm projectForm = new ProjectForm();
				projectForm.License = UIContext.License;
				root.Content = projectForm;
			}
		}

		public void RefreshData()
		{
			foreach (string res in new string[] {
								"productsData",
								"latestLicensesData",
								"allActiveServicesData",
								"allLicensesData"
						})
			{
				ObjectDataProvider provider = Resources[res] as ObjectDataProvider;
				if (provider != null)
				{
					provider.InitialLoad();
					provider.Refresh();
				}
			}

		}

		public void SetRecentItemsAndRefresh()
		{
			RefreshData();
			SetRecentItems();
		}

		public void SetRecentItems()
		{
			try
			{
				recentProjects.Items.Clear();

				BindingList<License> lics = UIContext.GetLatestLicenses();

				SeparatorTabItem separator = new SeparatorTabItem();
				separator.Header = "Recent Projects";
				recentProjects.Items.Add(separator);

				foreach (var license in lics)
				{
					TabItem tb = new TabItem();
					tb.Header = license.Name;
					tb.Name = string.Format("License_{0}", license.LicenseId.ToString());
					tb.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(b_MouseDoubleClick);

					Grid g = new Grid();
					g.RowDefinitions.Add(new RowDefinition{ Height = new GridLength(1, GridUnitType.Auto) });
					g.RowDefinitions.Add(new RowDefinition());
					g.RowDefinitions.Add(new RowDefinition());
					g.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

					TextBlock tb1 = new TextBlock {Text = string.Format("Project Name: {0}", license.Name)};
					tb1.SetValue(Grid.RowProperty, 0);

					g.Children.Add(tb1);
					
					System.Windows.Controls.Button b = new System.Windows.Controls.Button();
					b.Content = "Open This Project";
					b.Name = string.Format("License_{0}", license.LicenseId.ToString());
					b.Click += new System.Windows.RoutedEventHandler(btn_Click);
					b.SetValue(Grid.RowProperty, 3);

					g.Children.Add(b);
					tb.Content = g;
					recentProjects.Items.Add(tb);
				}
			}
			catch (Exception ex) { }
		}

		private void VerifyFirstTimeRun()
		{
			try
			{
				string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
				path = path.Replace("file:\\", "");

				if (File.Exists(path + "\\LicensingManager.exe.config") == false)
				{
					this.Visibility = Visibility.Hidden;

					FirstTimeWizard ftw = new FirstTimeWizard(this);
					ftw.Show();
				}

				IDatabaseUpdateProvider databaseUpdateProvider = ObjectLocator.GetInstance<IDatabaseUpdateProvider>();
				databaseUpdateProvider.InitializeDatabase(ConfigFileHelper.GetConnectionString());
			}
			catch { }
		}

		public ProductsScreen ProductsScreen
		{
			get
			{
				if (root.Content.GetType() == typeof(ProductsScreen))
					return (ProductsScreen)root.Content;
				else
					return null;
			}
		}

		public ServicesScreen ServicesScreen
		{
			get
			{
				if (root.Content.GetType() == typeof(ServicesScreen))
					return (ServicesScreen)root.Content;
				else
					return null;
			}
		}

		void btn_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			System.Windows.Controls.Button button = sender as System.Windows.Controls.Button;

			int id = int.Parse(button.Name.Replace("License_", ""));

			ILicenseService licenseService = ObjectLocator.GetInstance<ILicenseService>();
			UIContext.License = licenseService.GetLicenseById(id);
			Initalize();
		}

		private void b_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			System.Windows.Controls.TabItem button = sender as System.Windows.Controls.TabItem;

			int id = int.Parse(button.Name.Replace("License_", ""));

			ILicenseService licenseService = ObjectLocator.GetInstance<ILicenseService>();
			UIContext.License = licenseService.GetLicenseById(id);
			Initalize();
		}

		public bool IsLicenseOpen
		{
			get
			{
				if (root.Content == null)
					return false;

				return true;
			}
		}

		private void btnMenuExit_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			Environment.Exit(0);
		}

		private void ribbon_SelectedTabChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (e.AddedItems != null && e.AddedItems.Count == 1)
			{
				if (((RibbonTabItem)e.AddedItems[0]).Name == "homeTabItem")
				{
					WelcomeScreenForm welcomeScreenForm = new WelcomeScreenForm();
					root.Content = welcomeScreenForm;
				}
				else if (((RibbonTabItem)e.AddedItems[0]).Name == "productsTabItem")
				{
					if (_productsScreen == null)
						_productsScreen = new ProductsScreen();

					root.Content = _productsScreen;
				}
				else if (((RibbonTabItem)e.AddedItems[0]).Name == "servicesTabItem")
				{
					if (_servicesScreen == null)
						_servicesScreen = new ServicesScreen();

					root.Content = _servicesScreen;
				}

				else if (((RibbonTabItem)e.AddedItems[0]).Name == "projectTabItem")
				{
					if (UIContext.License != null)
					{
						ProjectForm projectForm = new ProjectForm();
						projectForm.License = UIContext.License;
						root.Content = projectForm;
					}
				}
			}
		}
	}
}
