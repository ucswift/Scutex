using System.Windows;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model;

namespace WaveTech.Scutex.Manager.Windows
{
	/// <summary>
	/// Interaction logic for FeaturesWindow.xaml
	/// </summary>
	public partial class FeaturesWindow : Window
	{
		private readonly Product _product;

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

		}

		private void btnRemoveSelected_Click(object sender, RoutedEventArgs e)
		{

		}
	}
}