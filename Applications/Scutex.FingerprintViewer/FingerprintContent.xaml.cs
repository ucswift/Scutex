using System.Windows.Controls;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Services;

namespace WaveTech.Scutex.FingerprintViewer
{
	public partial class FingerprintContent : UserControl
	{
		public FingerprintContent()
		{
			InitializeComponent();

			IHardwareFingerprintService fingerprintService = ObjectLocator.GetInstance<IHardwareFingerprintService>();
			txtFingerprint.Text = fingerprintService.GetHardwareFingerprint(FingerprintTypes.Default);
		}
	}
}