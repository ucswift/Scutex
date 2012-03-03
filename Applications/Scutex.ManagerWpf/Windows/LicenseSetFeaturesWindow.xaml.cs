using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Events;
using WaveTech.Scutex.Model.Interfaces.Framework;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.Manager.Windows
{
	/// <summary>
	/// Interaction logic for LicenseSetFeaturesWindow.xaml
	/// </summary>
	public partial class LicenseSetFeaturesWindow : Window
	{
		private readonly LicenseSet _licenseSet;
		private readonly Product _product;
		private readonly ObservableCollection<Feature> _productFeatures;

		public LicenseSetFeaturesWindow()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);
		}

		/// <summary>
		/// Constructor that takes a parent for this GenerationWindow window.
		/// </summary>
		/// <param name="parent">Parent window for this dialog.</param>
		public LicenseSetFeaturesWindow(Window parent)
			: this()
		{
			this.Owner = parent;
		}

		public LicenseSet LicenseSet
		{
			get { return _licenseSet; }
		}

		public ObservableCollection<Feature> ProductFeatures
		{
			get { return _productFeatures; }
		}

		private bool DoesFeatureExistInSet()
		{
			Feature feature = gridProductFeatures.SelectedItem as Feature;

			var licenseSets = from f in _licenseSet.Features
												where f.Name == feature.Name
												select f;

			if (licenseSets.Count() > 0)
				return true;

			return false;
		}

		public LicenseSetFeaturesWindow(Window parent, Product product, LicenseSet licenseSet)
			: this(parent)
		{
			_licenseSet = licenseSet;
			_product = product;
			gridProductFeatures.ItemsSource = new ObservableCollection<Feature>(ObjectLocator.GetInstance<IFeaturesService>().GetFeaturesForProduct(_product.ProductId));

			lblTitle.Text = _product.Name + " - " + _licenseSet.Name;
		}

		private void btnAddFeature_OnClick(object sender, RoutedEventArgs e)
		{
			if (gridProductFeatures.SelectedItem != null)
			{
				if (!DoesFeatureExistInSet())
				{
					_licenseSet.Features.Add((Feature) gridProductFeatures.SelectedItem);

				}
				else
				{
					MessageBox.Show("Feature already exists in the License Set/Edition.");
				}
			}
		}

		private void btnRemoveFeature_Click(object sender, RoutedEventArgs e)
		{
			if (gridLicenseSetFeatures.SelectedItem != null)
			{
				_licenseSet.Features.Remove((Feature)gridLicenseSetFeatures.SelectedItem);
			}
		}

		private void UpdateSetFeaturesGrid()
		{
			
		}
	}
}