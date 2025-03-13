using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace EducationTesting.Client.Helpers
{
    /// <summary>
    /// Конвертер для привязки видимости к ролям
    /// </summary>
    public class RoleToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return Visibility.Collapsed;

            var enumValue = value.ToString();
            var targetValues = parameter.ToString().Split(',');

            return targetValues.Any(targetValue =>
                enumValue.Equals(targetValue.Trim(), StringComparison.InvariantCultureIgnoreCase))
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}