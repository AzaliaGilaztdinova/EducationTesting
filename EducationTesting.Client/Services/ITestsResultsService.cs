using System.Collections.Generic;
using System.Threading.Tasks;
using EducationTesting.Client.Models.Tests;

namespace EducationTesting.Client.Services
{
    public interface ITestsResultsService : IModelService<TestResult>
    {
        IEnumerable<TestResult> GetListByStudentAndTestId(string studentId, string testId);
    }
}