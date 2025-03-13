using System.Collections.Generic;
using EducationTesting.Client.Models.Subjects;
using EducationTesting.Client.Repositories;

namespace EducationTesting.Client.Services
{
    public class SubjectsService : ISubjectsService
    {
        private readonly ISubjectsRepository _repository;

        public SubjectsService(ISubjectsRepository repository) => _repository = repository;

        public Subject Get(string id) => _repository.Get(id);

        public void Create(Subject item) => _repository.Create(item);

        public void Update(Subject item) => _repository.Update(item);

        public void Delete(string id) => _repository.Delete(id);

        public IEnumerable<Subject> GetListByDisciplineId(string disciplineId) =>
            _repository.GetListByDisciplineId(disciplineId);
    }
}