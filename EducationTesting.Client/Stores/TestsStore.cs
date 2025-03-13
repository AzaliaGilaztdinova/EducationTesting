using EducationTesting.Client.Models.Disciplines;
using EducationTesting.Client.Models.Subjects;
using EducationTesting.Client.Models.Tests;

namespace EducationTesting.Client.Stores
{
    public class TestsStore : ITestsStore
    {
        public Subject SelectedSubject { get; set; }
        public Test SelectedTest { get; set; }
        public Discipline SelectedDiscipline { get; set; }
    }
}