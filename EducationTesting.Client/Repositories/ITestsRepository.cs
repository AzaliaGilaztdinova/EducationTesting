using System.Collections.Generic;
using EducationTesting.Client.Models.Tests;

namespace EducationTesting.Client.Repositories
{
    public interface ITestsRepository : IRepository<Test>
    {
        IEnumerable<Test> GetListBySubjectId(string subjectId);
    }
}