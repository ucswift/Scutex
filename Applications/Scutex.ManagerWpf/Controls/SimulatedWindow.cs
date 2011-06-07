using System.Windows;
using System.Windows.Controls;
using WaveTech.Scutex.Manager.Classes;

namespace WaveTech.Scutex.Manager.Controls
{
	internal class SimulatedWindow : ContentControl
	{
		internal SimulatedWindow()
		{
		}

		static SimulatedWindow()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(SimulatedWindow), new FrameworkPropertyMetadata(typeof(SimulatedWindow)));
		}
	}

	internal class SimulatedWindow_Root
			: LocalizationRoot
	{
		internal string Title_MyWindow
		{
			get { return "test"; }
		}
	}
}
