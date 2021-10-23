using System;
using System.Globalization;
using MvvmCross.Converters;

namespace LeadersOfDigital.Converters
{
    public class StringToNullDateConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => DateTime.TryParse((string)value, out var date) ? date : null;
    }
}