using System.Windows.Controls;
using Infragistics.Windows.DataPresenter;
using WaveTech.Scutex.Framework;
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

			_productsService = ObjectLocator.GetInstance<IProductsService>();
			_eventAggregator = ObjectLocator.GetInstance<IEventAggregator>();

			_eventAggregator.AddListener<ProductsUpdatedEvent>(x => gridProducts.DataSource = _productsService.GetAllProducts());
		}

		public Product SelectedProduct
		{
			get
			{
				if (gridProducts.ActiveRecord != null)
				{
					DataRecord record = gridProducts.ActiveRecord as DataRecord;
					int productId = (int)record.Cells["ProductId"].Value;

					return _productsService.GetProductById(productId);
				}

				return null;
			}
		}
	}
}