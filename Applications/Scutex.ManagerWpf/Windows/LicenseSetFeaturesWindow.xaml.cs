using System.Windows;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model;
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

		public LicenseSetFeaturesWindow(Window parent, Product product, LicenseSet licenseSet)
			: this(parent)
		{
			_licenseSet = licenseSet;
			_product = product;

			gridProductFeatures.DataContext =
				ObjectLocator.GetInstance<IFeaturesService>().GetFeaturesForProduct(_product.ProductId);
		}
	}
}