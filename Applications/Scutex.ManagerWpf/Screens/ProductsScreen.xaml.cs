using System.Collections.Generic;
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
	/// Interaction logic for ProductsScreen.xaml
	/// </summary>
	public partial class ProductsScreen : UserControl
	{
		private readonly IEventAggregator _eventAggregator;
		private readonly IProductsService _productsService;

		public ProductsScreen()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);

			_productsService = ObjectLocator.GetInstance<IProductsService>();
			_eventAggregator = ObjectLocator.GetInstance<IEventAggregator>();

			_eventAggregator.AddListener<ProductsUpdatedEvent>(x => gridProducts.ItemsSource = _productsService.GetAllProducts());
		}

		public Product SelectedProduct
		{
			get
			{
				if (gridProducts.SelectedItem != null)
				{
					Product prod = gridProducts.SelectedItem as Product;
					return _productsService.GetProductById(prod.ProductId);
				}

				return null;
			}
		}
	}
}