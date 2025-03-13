using System.Windows;

namespace EducationTesting.Client
{
    public interface IMessageBoxProvider
    {
        MessageBoxResult Show(string message, string caption, MessageBoxButton button, MessageBoxImage icon);
    }
}