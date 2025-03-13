using System.Windows.Input;
using EducationTesting.Client.Helpers;
using EducationTesting.Client.Models.Tests;

namespace EducationTesting.Client.ViewModels
{
    public interface ITestPerformVm
    {
        Test Item { get; }
        Property<string> ErrorMessageProp { get; }
        ICommand SaveCommand { get; }
        ICommand CancelCommand { get; }
    }
}