using System.Windows;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model;

namespace WaveTech.Scutex.Manager.Windows
{
	/// <summary>
	/// Interaction logic for LicenseSetFeaturesWindow.xaml
	/// </summary>
	public partial class LicenseSetFeaturesWindow : Window
	{
		private readonly LicenseSet _licenseSet;

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

		public LicenseSetFeaturesWindow(Window parent, LicenseSet licenseSet)
			: this(parent)
		{
			_licenseSet = licenseSet;
		}
	}
}