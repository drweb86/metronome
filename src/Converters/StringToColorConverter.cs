using System;
using System.Windows.Data;
using System.Windows.Media;

namespace Metronome.Converters
{
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;
            else
                return ColorConverter.ConvertFromString((string) value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;
            else
                return value.ToString();
        }
    }
}