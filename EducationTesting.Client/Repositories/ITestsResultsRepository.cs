using System.Collections.Generic;
using EducationTesting.Client.Models.Tests;

namespace EducationTesting.Client.Repositories
{
    public interface ITestsResultsRepository : IRepository<TestResult>
    {
        IEnumerable<TestResult> GetListByStudentAndTestId(string studentId, string testId);
    }
}