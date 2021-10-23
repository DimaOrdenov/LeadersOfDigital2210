using System;
using System.Globalization;
using MvvmCross.Converters;

namespace Converters
{
    public class StringFormatConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (parameter is not string format)
            {
                return value.ToString();
            }

            return string.Format(format, value);
        }
    }
}
