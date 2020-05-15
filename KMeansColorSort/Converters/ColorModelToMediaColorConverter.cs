using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using KMeansColorSort.Models;

namespace KMeansColorSort.Converters
{
    class ColorModelToMediaColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ColorModel color)
                return new System.Windows.Media.Color() { R = (byte)color.R, G = (byte)color.G, B = (byte)color.B, A = (byte)color.A };

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
