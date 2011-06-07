using System.Windows;
using System.Windows.Controls;

namespace WaveTech.Scutex.Manager.UserControls
{
	/// <summary>
	/// Interaction logic for LoadingAnimation.xaml
	/// </summary>
	public partial class LoadingAnimation : UserControl
	{
		#region Dependency Properties
		public static readonly DependencyProperty WindowHeightProperty =
	DependencyProperty.Register("WindowHeight", typeof(int?), typeof(LoadingAnimation),
	new FrameworkPropertyMetadata(
			null,
			FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
			WindowHeightPropertyChanged));

		public int? WindowHeight
		{
			get { return (int?)GetValue(WindowHeightProperty); }
			set { SetValue(WindowHeightProperty, value); }
		}

		private static void WindowHeightPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

		}

		public static readonly DependencyProperty WindowWidthProperty =
	DependencyProperty.Register("WindowWidth", typeof(int?), typeof(LoadingAnimation),
	new FrameworkPropertyMetadata(
			null,
			FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
			WindowHeightPropertyChanged));

		public int? WindowWidth
		{
			get { return (int?)GetValue(WindowWidthProperty); }
			set { SetValue(WindowWidthProperty, value); }
		}

		private static void WindowWidthPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{

		}
		#endregion Dependency Properties

		public LoadingAnimation()
		{
			InitializeComponent();
		}
	}
}
