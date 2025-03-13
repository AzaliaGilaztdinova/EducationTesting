using System.Collections.Generic;
using EducationTesting.Client.Models.Tests;
using EducationTesting.Client.Repositories;

namespace EducationTesting.Client.Services
{

    public class TestsResultsService : ITestsResultsService
    {
        private readonly ITestsResultsRepository _repository;

        public TestsResultsService(ITestsResultsRepository repository) => _repository = repository;

        public TestResult Get(string id) => _repository.Get(id);

        public void Create(TestResult item) => _repository.Create(item);

        public void Update(TestResult item) => _repository.Update(item);

        public void Delete(string id) => _repository.Delete(id);

        public IEnumerable<TestResult> GetListByStudentAndTestId(string studentId, string testId) =>
            _repository.GetListByStudentAndTestId(studentId, testId);
    }
}