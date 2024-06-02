using Avalonia.Data.Converters;
using Avalonia.Data;
using System;

namespace Fractals;

public class ComparisonConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        return value?.Equals(parameter);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, System.Globalization.CultureInfo culture)
    {
        return value?.Equals(true) == true ? parameter : BindingOperations.DoNothing;
    }
}