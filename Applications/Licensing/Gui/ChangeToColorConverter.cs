using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WaveTech.Scutex.Licensing.Gui
{
	[ValueConversion(typeof(string), typeof(SolidColorBrush))]
	internal class ChangeToColorConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			String newVal = (String)value;
			if (newVal.IndexOf("-") == -1)
			{
				Color col = new Color();
				col.R = 99;
				col.G = 196;
				col.B = 3;
				col.A = 255;
				return new SolidColorBrush(col);
			}
			else
			{
				Color col = new Color();
				col.R = 255;
				col.G = 43;
				col.B = 43;
				col.A = 255;
				return new SolidColorBrush(col);
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}