using System.Windows;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Manager.Windows
{
	/// <summary>
	/// Interaction logic for FeaturesWindow.xaml
	/// </summary>
	public partial class FeaturesWindow : Window
	{
		private readonly Product _product;
		private IProductsService _productsService;

		public FeaturesWindow()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);
		}

		public FeaturesWindow(Window parent, Product product)
			: this(parent)
		{
			_product = product;
		}

		public Product SelectedProduct
		{
			get { return _product; }
		}

		/// <summary>
		/// Constructor that takes a parent for this GenerationWindow window.
		/// </summary>
		/// <param name="parent">Parent window for this dialog.</param>
		public FeaturesWindow(Window parent)
			: this()
		{
			this.Owner = parent;
		}

		private void btnAddFeature_Click(object sender, RoutedEventArgs e)
		{
			if (_productsService == null)
				_productsService = ObjectLocator.GetInstance<IProductsService>();

			Feature f = new Feature();
			f.Name = txtFeatureName.Text;
			f.Description = txtFeatureDescription.Text;
			f.ProductId = _product.ProductId;
		}

		private void btnRemoveSelected_Click(object sender, RoutedEventArgs e)
		{
			if (_productsService == null)
				_productsService = ObjectLocator.GetInstance<IProductsService>();

			if (gridFeatures.SelectedItem != null)
			{
				Feature feat = gridFeatures.SelectedItem as Feature;
				
			}
		}
	}
}