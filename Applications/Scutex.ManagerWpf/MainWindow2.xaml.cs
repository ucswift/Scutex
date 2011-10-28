using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
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
	/// Interaction logic for MainWindow2.xaml
	/// </summary>
	public partial class MainWindow2 : RibbonWindow
	{
		private IEventAggregator _eventAggregator;

		public MainWindow2()
		{
			InitializeComponent();

			//ribbon.DataContext = this;
			//DataContext = this;

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

			this.Title = "Scutex Licensing Manager";

			try
			{
				Bootstrapper.Configure();

				_eventAggregator = ObjectLocator.GetInstance<IEventAggregator>();
				_eventAggregator.AddListener<ProductsUpdatedEvent>(x => RefreshData());
				_eventAggregator.AddListener<LicenseSavedEvent>(x => SetRecentItemsAndRefresh());
				_eventAggregator.AddListener<ServicesUpdatedEvent>(x => RefreshData());

				Initalize();
				SetRecentItems();
				VerifyFirstTimeRun();

				WelcomeScreenForm welcomeScreenForm = new WelcomeScreenForm();
				root.Content = welcomeScreenForm;
			}
			catch { }
		}

		public void Initalize()
		{
			RefreshData();

			root.Content = null;

			if (UIContext.License == null)
			{
				root.Content = null;
			}
			else
			{
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

				foreach (var license in lics)
				{
					TabItem tb = new TabItem();
					Fluent.Button b = new Fluent.Button();
					b.Content = license.Name;
					b.Name = license.LicenseId.ToString();
					b.Click += new System.Windows.RoutedEventHandler(btn_Click);

					tb.Content = b;
					//ButtonTool btn = new ButtonTool();
					//btn.Caption = license.Name;
					//btn.Id = license.LicenseId.ToString();
					//btn.Click += new System.Windows.RoutedEventHandler(btn_Click);

					//this.myRibbon.ApplicationMenu.RecentItems.Add(btn);
				}

				//this.myRibbon.ApplicationMenu.UpdateLayout();
			}
			catch { }
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

					//FirstTimeWizard ftw = new FirstTimeWizard(this);
					//ftw.Show();
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

		void btn_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			//ButtonTool button = sender as ButtonTool;

			//ILicenseService licenseService = ObjectLocator.GetInstance<ILicenseService>();
			//UIContext.License = licenseService.GetLicenseById(int.Parse(button.Id));
			//Initalize();
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

		private void myRibbon_RibbonTabItemSelected(object sender, Infragistics.Windows.Ribbon.Events.RibbonTabItemSelectedEventArgs e)
		{
			if (root != null)
			{
				root.Content = null;

				if (e.NewSelectedRibbonTabItem.Name == "HomeTab")
				{
					if (UIContext.License != null)
					{
						ProjectForm projectForm = new ProjectForm();
						projectForm.License = UIContext.License;
						root.Content = projectForm;
					}
					else
					{
						WelcomeScreenForm welcomeScreenForm = new WelcomeScreenForm();
						root.Content = welcomeScreenForm;
					}
				}
				else if (e.NewSelectedRibbonTabItem.Name == "ProductsTab")
				{
					ProductsScreen productsScreen = new ProductsScreen();
					root.Content = productsScreen;
				}
				else if (e.NewSelectedRibbonTabItem.Name == "ServicesTab")
				{

				}
			}
		}

		private void ribbon_SelectedTabChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (e.AddedItems != null && e.AddedItems.Count == 1)
			{
				
			}
		}
	}
}
