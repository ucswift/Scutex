using System.Windows;
using System.Windows.Controls;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Events;
using WaveTech.Scutex.Model.Interfaces.Framework;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Manager.Screens
{
	/// <summary>
	/// Interaction logic for ServicesScreen.xaml
	/// </summary>
	public partial class ServicesScreen : UserControl
	{
		private readonly IEventAggregator _eventAggregator;
		private readonly IServicesService _servicesService;

		public ServicesScreen()
		{
			InitializeComponent();

			_servicesService = ObjectLocator.GetInstance<IServicesService>();
			_eventAggregator = ObjectLocator.GetInstance<IEventAggregator>();

			_eventAggregator.AddListener<ServicesUpdatedEvent>(x => gridServices.ItemsSource = _servicesService.GetAllServices());
		}

		public Service SelectedService
		{
			get
			{
				if (gridServices.SelectedItem != null)
				{
					Service prod = gridServices.SelectedItem as Service;
					return _servicesService.GetServiceById(prod.ServiceId);
				}

				return null;
			}
		}

		public void StartSpinner()
		{
			loadingAnimation.Visibility = Visibility.Visible;
		}

		public void StopSpinner()
		{
			loadingAnimation.Visibility = Visibility.Collapsed;
		}
	}
}