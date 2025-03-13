using EducationTesting.Client.Models.Disciplines;

namespace EducationTesting.Client.Stores
{
    public class SubjectsStore : ISubjectsStore
    {
        public Discipline SelectedDiscipline { get; set; }
    }
}