using System;
using System.Globalization;
using System.Windows.Data;
using EducationTesting.Client.Models.Tests;

namespace EducationTesting.Client.Helpers
{
    public class QuestionTypeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is QuestionType questionType)
            {
                return questionType.GetString();
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}