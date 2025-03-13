using System.Collections.Generic;
using EducationTesting.Client.Models.Tests;
using EducationTesting.Client.Repositories;

namespace EducationTesting.Client.Services
{
    public class TestsService : ITestsService
    {
        private readonly ITestsRepository _repository;

        public TestsService(ITestsRepository repository) => _repository = repository;

        public Test Get(string id) => _repository.Get(id);

        public void Create(Test item) => _repository.Create(item);

        public void Update(Test item) => _repository.Update(item);

        public void Delete(string id) => _repository.Delete(id);

        public IEnumerable<Test> GetListBySubjectId(string subjectId) => _repository.GetListBySubjectId(subjectId);
    }
}