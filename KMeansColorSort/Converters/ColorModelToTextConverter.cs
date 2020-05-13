using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using KMeansColorSort.Models;

namespace KMeansColorSort.Converters
{
    class ColorModelToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ColorModel model)
                return $"RGB: ({model.R}, {model.G}, {model.B}) HSV: ({model.Hue}, {model.Saturation}, {model.Lightness}) Band: {model.Band}";

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
