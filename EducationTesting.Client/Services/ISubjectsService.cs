using System.Collections.Generic;
using EducationTesting.Client.Models.Subjects;

namespace EducationTesting.Client.Services
{
    public interface ISubjectsService : IModelService<Subject>
    {
        IEnumerable<Subject> GetListByDisciplineId(string disciplineId);
    }
}