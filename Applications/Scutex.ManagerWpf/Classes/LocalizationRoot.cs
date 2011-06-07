using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace WaveTech.Scutex.Manager.Classes
{
	public class LocalizationRoot : UserControl
	{
		public LocalizationRoot()
		{
			this.Loaded += this.OnLoaded;
		}

		public void OnLoaded(object sender, RoutedEventArgs e)
		{
			if (!DesignerProperties.GetIsInDesignMode(this))
			{

			}
		}
	}
}