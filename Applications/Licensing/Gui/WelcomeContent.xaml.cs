using System;
using System.Windows.Controls;

namespace WaveTech.Scutex.Licensing.Gui
{
	/// <summary>
	/// Interaction logic for WelcomeContent.xaml
	/// </summary>
	public partial class WelcomeContent : UserControl
	{
		private string _productName;

		public WelcomeContent()
		{
			InitializeComponent();

			lblProductName.Text = _productName;
		}

		public WelcomeContent(string productName)
			: this()
		{
			_productName = productName;

			try
			{
				lblProductName.Text = _productName;
			}
			catch (Exception)
			{ }
		}
	}
}