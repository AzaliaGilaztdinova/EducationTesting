using System.Collections.Generic;
using EducationTesting.Client.Models.Subjects;

namespace EducationTesting.Client.Repositories
{
    public interface ISubjectsRepository : IRepository<Subject>
    {
        IEnumerable<Subject> GetListByDisciplineId(string disciplineId);
    }
}