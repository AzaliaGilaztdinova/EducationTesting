using System.Windows;

namespace EducationTesting.Client
{
    public class MessageBoxProvider : IMessageBoxProvider
    {
        public MessageBoxResult Show(string message, string caption, MessageBoxButton button, MessageBoxImage icon) =>
            MessageBox.Show(message, caption, button, icon);
    }
}