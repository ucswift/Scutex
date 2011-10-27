using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using WaveTech.Scutex.Licensing;
using WaveTech.Scutex.Model;

namespace Scutex.Samples.CSharp.WindowsFormsControlLibrary
{
	[LicenseProvider(typeof(ScutexLicenseProvider))]
	[DesignerAttribute(typeof(Component1tDesigner), typeof(IDesigner))]
	partial class Component1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		private static LicensingManagerOptions options = new LicensingManagerOptions
		                                                 	{
		                                                 		DataFileLocation = @"C:\Temp\Scutex\License.lic",
																												PublicKey = "31|32|36|32|30|34|33|37|31|35|34|35|34|36|36|34|37|32|37|39|39|36|31|35|33|32|36|34|39|36|37|30|38|34|36|30|38|31|32|34|31|36|34|31|30|35|33|39|31|31|38|32|34|30|32|36|37|32|37|37|35|32|31|31|31|33|33|32|33|34|34|31|30|36|36|31|38|39|39|34|31|35|32|38|33|38|30|31|39|33|39|32|37|38|38|30|39|38|30|31|30|34|38|38|30|35|31|38|32|38|35|37|31|31|31|34|32|35|33|33|37|32|30|32|37|30|32|37|35|35|31|32|39|36|37|39|35|34|35|30|37|39|34|39|30|30|35|35|35|35|37|36|39|35|39|34|34|36|39|39|38|36|36|31|30|31|31|30|31|38|32|39|32|37|35|36|37|34|31|39|36|37|32|36|35|32|35|34|37|31|38|31|38|33|38|34|34|39|39|35|34|30|34|36|34|30|36|39|35|30|34|35|35|37|31|39|37|31|37|32|34|37|30|32|33|37|37|33|39|32|36|33|7c|36|35|35|33|37",
																												DllHash = "43|44|2d|36|42|2d|31|45|2d|30|30|2d|42|43|2d|45|32|2d|41|33|2d|31|33|2d|31|33|2d|34|38|2d|39|35|2d|44|32|2d|46|44|2d|43|42|2d|33|45|2d|35|36|2d|38|34|2d|33|30|2d|42|32|2d|46|46",
																												KillOnError = false
		                                                 	};

		public static LicensingManager LicensingManager = new LicensingManager(null, options);
		public static ScutexComponentLicense License;

		public void ValidateLicense()
		{
			ScutexLicenseProvider.SetLicensingManager(LicensingManager);
			ScutexComponentLicense lic = (ScutexComponentLicense)LicenseManager.Validate(typeof (Component1), this);

			if (lic != null)
				License = lic;
			else
			{
				MessageBox.Show("License Invalid");
			}


		}

		public void SetLicense()
		{
			
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}

		#endregion
	}

	public class Component1tDesigner : System.ComponentModel.Design.ComponentDesigner
	{
		public Component1tDesigner()
		{
		}

		// This method provides an opportunity to perform processing when a designer is initialized.
		// The component parameter is the component that the designer is associated with.
		public override void Initialize(System.ComponentModel.IComponent component)
		{
			// Always call the base Initialize method in an override of this method.
			base.Initialize(component);
		}

		// This method is invoked when the associated component is double-clicked.
		public override void DoDefaultAction()
		{
			MessageBox.Show("The event handler for the default action was invoked.");
		}

		// This method provides designer verbs.
		public override System.ComponentModel.Design.DesignerVerbCollection Verbs
		{
			get
			{
				return new DesignerVerbCollection(new DesignerVerb[] { 
										new DesignerVerb("License Registration", new EventHandler(this.OnRegister))
								});
			}
		}

		private void OnRegister(object sender, EventArgs e)
		{
			Component1.LicensingManager.Validate(InteractionModes.Gui);
		}
	}

}