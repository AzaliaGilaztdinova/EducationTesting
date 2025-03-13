using EducationTesting.Client.Models.Disciplines;
using EducationTesting.Client.Models.Subjects;
using EducationTesting.Client.Models.Tests;

namespace EducationTesting.Client.Stores
{
    public interface ITestsStore
    {
        Subject SelectedSubject { get; set; }
        Test SelectedTest { get; set; }
        Discipline SelectedDiscipline { get; set; }
    }
}