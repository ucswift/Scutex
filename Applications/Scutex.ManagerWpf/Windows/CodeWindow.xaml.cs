using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using Infragistics.Windows.Ribbon;
using WaveTech.Scutex.Framework;
using WaveTech.Scutex.Manager.Classes;
using WaveTech.Scutex.Model;
using WaveTech.Scutex.Model.Interfaces.Providers;
using WaveTech.Scutex.Model.Interfaces.Services;
using MessageBox = System.Windows.Forms.MessageBox;

namespace WaveTech.Scutex.Manager.Windows
{
	/// <summary>
	/// Interaction logic for CodeWindow.xaml
	/// </summary>
	public partial class CodeWindow : XamRibbonWindow
	{
		#region Consturctors
		public CodeWindow()
		{
			InitializeComponent();

			WindowHelper.CheckAndApplyTheme(this);

			rdoCodeTypeCSharp.IsChecked = true;
			PrintAttribute();
		}

		/// <summary>
		/// Constructor that takes a parent for this GenerationWindow window.
		/// </summary>
		/// <param name="parent">Parent window for this dialog.</param>
		public CodeWindow(Window parent)
			: this()
		{
			this.Owner = parent;
		}
		#endregion Consturctors

		#region Private Methods
		private void PrintAttribute()
		{
			IHashingProvider hashingProvider = ObjectLocator.GetInstance<IHashingProvider>();
			IEncodingService encodingService = ObjectLocator.GetInstance<IEncodingService>();

			string dllCheckHash = encodingService.Encode(hashingProvider.HashFile(Directory.GetCurrentDirectory() + "\\lib\\WaveTech.Scutex.Licensing.dll"));
			string publicKey = encodingService.Encode(UIContext.License.KeyPair.PublicKey);

			if (rdoCodeTypeCSharp.IsChecked.HasValue && rdoCodeTypeCSharp.IsChecked.Value)
				txtAttribute.Text = string.Format("[assembly: License(\"{0}\",\"{1}\")]", publicKey, dllCheckHash);
			else
				txtAttribute.Text = string.Format("<Assembly: License(\"{0}\",\"{1}\")>", publicKey, dllCheckHash);

			txtParam1.Text = publicKey;
			txtParam2.Text = dllCheckHash;
		}
		#endregion Private Methods

		#region Private Event Handlers
		private void btnGetDataFile_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.FileName = "sxu.dll";
			dialog.DefaultExt = ".dll";
			dialog.Filter = "DLL Files (.dll)|*.dll";

			dialog.ShowDialog();
			if (!String.IsNullOrEmpty(dialog.FileName))
			{
				ClientLicense cl = new ClientLicense(UIContext.License);

				string fileName = System.IO.Path.GetFileName(dialog.FileName);

				if (fileName != "sxu.dll")
				{
					MessageBox.Show("This version of Scutex only supports a data file with the name of sxu.dll.");
					return;
				}

				IClientLicenseService clientLicenseService = ObjectLocator.GetInstance<IClientLicenseService>();
				clientLicenseService.SaveClientLicense(cl, dialog.FileName);
			}
		}

		private void rdoCodeTypeCSharp_Click(object sender, RoutedEventArgs e)
		{
			PrintAttribute();
		}

		private void rdoCodeTypeVB_Click(object sender, RoutedEventArgs e)
		{
			PrintAttribute();
		}

		private void cboProjectType_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			if (cboProjectType != null && grpAttribute != null)
			{
				if (cboProjectType.SelectedIndex == 0)
				{
					grpAttribute.Visibility = Visibility.Visible;
					grpCCW.Visibility = Visibility.Collapsed;
				}
				else
				{
					grpCCW.Visibility = Visibility.Visible;
					grpAttribute.Visibility = Visibility.Collapsed;
				}
			}
		}

		private void btnGetCCWFile_Click(object sender, RoutedEventArgs e)
		{
			SaveFileDialog dialog = new SaveFileDialog();
			dialog.FileName = "ScutexLicensingCCW.dll";
			dialog.DefaultExt = ".dll";
			dialog.Filter = "DLL Files (.dll)|*.dll";

			dialog.ShowDialog();
			if (!String.IsNullOrEmpty(dialog.FileName))
			{
				string assemblyPath = Directory.GetCurrentDirectory() + "\\lib\\WaveTech.Scutex.Licensing.dll";

				IHashingProvider hashingProvider = ObjectLocator.GetInstance<IHashingProvider>();
				IEncodingService encodingService = ObjectLocator.GetInstance<IEncodingService>();

				string dllCheckHash = encodingService.Encode(hashingProvider.HashFile(assemblyPath));
				string publicKey = encodingService.Encode(UIContext.License.KeyPair.PublicKey);

				IComApiWrappingService comApiWrappingService = ObjectLocator.GetInstance<IComApiWrappingService>();
				comApiWrappingService.CreateComWrapper(dialog.FileName, assemblyPath, publicKey, dllCheckHash);
			}
		}
		#endregion Private Event Handlers
	}
}