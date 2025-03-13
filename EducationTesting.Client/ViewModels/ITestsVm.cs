using System.Collections.Generic;
using System.Windows.Input;
using EducationTesting.Client.Models.Tests;
using EducationTesting.Client.Models.Users;

namespace EducationTesting.Client.ViewModels
{
    public interface ITestsVm
    {
        string SearchText { get; set; }
        string DisciplineName { get; }
        string SubjectName { get; }
        IEnumerable<Test> Tests { get; }
        Roles Role { get; }
        ICommand AddCommand { get; }
        ICommand GoToCommand { get; }
        ICommand DeleteCommand { get; }
        ICommand GoBackCommand { get; }
        ICommand GoBackDisciplinesCommand { get; }
        ICommand GoToResultsCommand { get; }
        ICommand GoToPerformCommand { get; }
    }
}