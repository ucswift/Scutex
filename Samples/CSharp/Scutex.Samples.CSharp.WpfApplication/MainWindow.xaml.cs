using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WaveTech.Scutex.Licensing;
using WaveTech.Scutex.Model;

namespace Scutex.Samples.CSharp.WpfApplication
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			LicensingManager licensingManager = new LicensingManager(null);
			ScutexLicense license = licensingManager.Validate(InteractionModes.Gui);


			bool wasPressed = license.InterfaceInteraction.TryButtonClicked;
			Console.WriteLine(Guid.NewGuid().ToString());
			Console.WriteLine("Test Product");
			Console.WriteLine();

			Console.WriteLine(String.Format("Is Product Licensed: {0}", license.IsLicensed));
			Console.WriteLine(String.Format("Is Trial Valid: {0}", license.IsTrialValid));
			Console.WriteLine(String.Format("Is Trial Expired: {0}", license.IsTrialExpired));
			Console.WriteLine(String.Format("Is Trial Fault Reason: {0}", license.TrialFaultReason));
			Console.WriteLine(String.Format("Is Trial Remaining: {0}", license.TrialRemaining));
			Console.WriteLine(String.Format("Is Trial Elapsed: {0}", license.TrialUsed));
			Console.WriteLine(String.Format("Is Trial First Run: {0}", license.WasTrialFristRun));

			Console.WriteLine();
			Console.WriteLine("Press ENTER to exit.");
			Console.ReadLine();
		}
	}
}
