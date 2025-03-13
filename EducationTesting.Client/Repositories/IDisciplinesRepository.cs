using System.Collections.Generic;
using EducationTesting.Client.Models.Disciplines;

namespace EducationTesting.Client.Repositories
{
    public interface IDisciplinesRepository : IRepository<Discipline>
    {
        IEnumerable<Discipline> GetList();
    }
}