using System;
using System.Windows.Data;

namespace WaveTech.Scutex.Manager.Converters
{
	public class GuidToStringConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value != null)
				return ((Guid) value).ToString();

			return "";
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value == null) 
				return (Guid)value;

			return Guid.Empty;
		}

		#endregion
	}
}