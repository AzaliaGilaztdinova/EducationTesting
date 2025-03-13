using System.Collections.Generic;
using EducationTesting.Client.Models.Disciplines;
using EducationTesting.Client.Repositories;

namespace EducationTesting.Client.Services
{
    public class DisciplinesService : IDisciplinesService
    {
        private readonly IDisciplinesRepository _repository;

        public DisciplinesService(IDisciplinesRepository repository) => _repository = repository;

        public Discipline Get(string id) => _repository.Get(id);

        public IEnumerable<Discipline> GetList() => _repository.GetList();

        public void Create(Discipline item) => _repository.Create(item);

        public void Update(Discipline item) => _repository.Update(item);

        public void Delete(string id) => _repository.Delete(id);
    }
}