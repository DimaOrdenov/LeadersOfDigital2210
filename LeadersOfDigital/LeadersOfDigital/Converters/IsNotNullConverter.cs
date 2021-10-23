using System;
using System.Globalization;
using MvvmCross.Converters;

namespace LeadersOfDigital.Converters
{
    public class IsNotNullConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null;
        }
    }
}
