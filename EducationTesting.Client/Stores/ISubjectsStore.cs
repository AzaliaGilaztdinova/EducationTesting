using EducationTesting.Client.Models.Disciplines;

namespace EducationTesting.Client.Stores
{
    public interface ISubjectsStore
    {
        Discipline SelectedDiscipline { get; set; }
    }
}