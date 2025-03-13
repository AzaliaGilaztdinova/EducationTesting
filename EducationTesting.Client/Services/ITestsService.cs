using System.Collections.Generic;
using EducationTesting.Client.Models.Tests;

namespace EducationTesting.Client.Services
{
    public interface ITestsService : IModelService<Test>
    {
        IEnumerable<Test> GetListBySubjectId(string subjectId);
    }
}