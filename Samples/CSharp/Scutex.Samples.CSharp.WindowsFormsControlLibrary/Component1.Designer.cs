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
																												PublicKey = "",
																												DllHash = ""
		                                                 	};

		public static LicensingManager License = new LicensingManager(options);

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

			License = new LicensingManager(this);
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
				Component1.License.Validate(InteractionModes.Component);
		}
	}

}