using System.Windows;
using System.Windows.Controls;
using WaveTech.Scutex.Model;

namespace WaveTech.Scutex.Manager.Forms
{
	/// <summary>
	/// Interaction logic for ProjectForm.xaml
	/// </summary>
	public partial class ProjectForm : UserControl
	{
		public static readonly DependencyProperty LicenseProperty =
		DependencyProperty.Register("License", typeof(License), typeof(ProjectForm),
		new FrameworkPropertyMetadata(
				null,
				FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
				LicensePropertyChanged));

		public ProjectForm()
		{
			InitializeComponent();
		}

		public License License
		{
			get { return (License)GetValue(LicenseProperty); }
			set { SetValue(LicenseProperty, value); }
		}

		private static void LicensePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

		}
	}
}