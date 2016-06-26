using System;
using System.Windows.Data;
using Metronome.Services;

namespace Metronome.Converters
{
    public class BitsPerMinuteConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return Binding.DoNothing;
            
            return BitsPerMinuteHelper.GetTempoMarking((int)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
