using System.Windows;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Manager.Windows
{
	/// <summary>
	/// Interaction logic for ViewServicesWindow.xaml
	/// </summary>
	public partial class ViewServicesWindow : Window
	{
		private IServicesService _servicesService;

		public ViewServicesWindow()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);

			_servicesService = ObjectLocator.GetInstance<IServicesService>();
			gridServices.ItemsSource = _servicesService.GetAllServices();
		}

		/// <summary>
		/// Constructor that takes a parent for this GenerationWindow window.
		/// </summary>
		/// <param name="parent">Parent window for this dialog.</param>
		public ViewServicesWindow(Window parent)
			: this()
		{
			this.Owner = parent;
		}
	}
}
