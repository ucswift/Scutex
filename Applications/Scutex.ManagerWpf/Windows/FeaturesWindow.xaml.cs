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
		private IFeaturesService _featuresService;

		public FeaturesWindow()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);
		}

		public FeaturesWindow(Window parent, Product product)
			: this(parent)
		{
			_product = product;
			lblProductName.Text = _product.Name;

			gridFeatures.ItemsSource = SelectedProduct.Features;
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
			if (_featuresService == null)
				_featuresService = ObjectLocator.GetInstance<IFeaturesService>();

			if (string.IsNullOrEmpty(txtFeatureName.Text))
			{
				MessageBox.Show("You must supply a feature name.");
				return;
			}

			Feature f = new Feature();
			f.Name = txtFeatureName.Text;
			f.Description = txtFeatureDescription.Text;
			f.ProductId = _product.ProductId;

			if (_featuresService.IsFeatureNameInUse(_product.ProductId, f.Name))
			{
				MessageBox.Show("Feature name is in use for this product, please try again");
				return;
			}
			else
			{
				_featuresService.SaveFeature(f);

				txtFeatureName.Text = "";
				txtFeatureDescription.Text = "";

				_product.Features = new NotifyList<Feature>(_featuresService.GetFeaturesForProduct(_product.ProductId));
				gridFeatures.ItemsSource = SelectedProduct.Features;
			}
		}

		private void btnRemoveSelected_Click(object sender, RoutedEventArgs e)
		{
			if (_featuresService == null)
				_featuresService = ObjectLocator.GetInstance<IFeaturesService>();

			if (gridFeatures.SelectedItem != null)
			{
				Feature feat = gridFeatures.SelectedItem as Feature;

				if (_featuresService.IsFeatureInUse(feat.FeatureId))
				{
					MessageBox.Show("Cannot delete feature, as it's in use in a license/edition set");
					return;
				}
				else
				{
					if (MessageBox.Show("Are you sure you want to delete this feature?", "Delete Feature", MessageBoxButton.YesNo) ==
					    MessageBoxResult.Yes)
					{
						_featuresService.DeleteFeatureById(feat.FeatureId);
						_product.Features = new NotifyList<Feature>(_featuresService.GetFeaturesForProduct(feat.ProductId));
						gridFeatures.ItemsSource = SelectedProduct.Features;
					}
				}
			}
		}
	}
}