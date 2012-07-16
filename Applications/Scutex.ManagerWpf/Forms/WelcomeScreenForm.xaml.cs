using System.Windows.Controls;
using WaveTech.Scutex.Manager.Classes;

namespace WaveTech.Scutex.Manager.Forms
{
	/// <summary>
	/// Interaction logic for WelcomeScreenForm.xaml
	/// </summary>
	public partial class WelcomeScreenForm : UserControl
	{
		public WelcomeScreenForm()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);

			txtVersion.Text = string.Format("Version: {0}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
		}

		private void hplVideosClick_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.wtdt.com/Products/Scutex/ScutexVideos.aspx");
		}

		private void hplDocumentation_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.wtdt.com/Documentation/Scutex/contents.html");
		}

		private void hplForums_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://scutex.codeplex.com/discussions");
		}

		private void hplHelpDesk_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://scutex.codeplex.com/workitem/list/basic");
		}

		private void hplGitHub_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("https://github.com/wavetech/Scutex");
		}

		private void hplCodePlex_Click_1(object sender, System.Windows.RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://scutex.codeplex.com");
		}
	}
}