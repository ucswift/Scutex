namespace Scutex.Samples.CSharp.WindowsFormsControlLibrary
{
	partial class TestForm1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.component11 = new Scutex.Samples.CSharp.WindowsFormsControlLibrary.Component1(this.components);
			this.SuspendLayout();
			// 
			// TestForm1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.Name = "TestForm1";
			this.Text = "TestForm1";
			this.ResumeLayout(false);

		}

		#endregion

		private Component1 component11;
	}
}