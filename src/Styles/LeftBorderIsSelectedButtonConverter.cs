using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Metronome.Styles
{
    class LeftBorderIsSelectedButtonConverter: IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2)
                return false;
            var currentUri = (string)values[0];
            var tag = (string)values[1];

            return currentUri == tag;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("LeftBorderIsSelectedButtonConverter::ConvertBack");
        }
    }
}
