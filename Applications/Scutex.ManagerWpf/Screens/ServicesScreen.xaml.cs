using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

			WindowHelper.CheckAndApplyTheme(this);

			_servicesService = ObjectLocator.GetInstance<IServicesService>();
			_eventAggregator = ObjectLocator.GetInstance<IEventAggregator>();

			_eventAggregator.AddListener<ProductsUpdatedEvent>(x => gridServices.ItemsSource = _servicesService.GetAllServices());
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
	}
}