using System;
using System.Globalization;
using MvvmCross.Converters;

namespace Converters
{
    public class IfEqualToParameterConverter : MvxValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != parameter.GetType())
            {
                return false;
            }

            if (value is Enum)
            {
                return Enum.GetName(value.GetType(), value) == Enum.GetName(value.GetType(), parameter);
            }
            
            // TODO Add more types

            return value == parameter;
        }
    }
}
