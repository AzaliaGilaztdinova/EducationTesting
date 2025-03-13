using System.Collections.Generic;
using EducationTesting.Client.Models.Classes;
using EducationTesting.Client.Repositories;

namespace EducationTesting.Client.Services
{
    public class ClassesService : IClassesService
    {
        private readonly IClassesRepository _repository;

        public ClassesService(IClassesRepository repository) => _repository = repository;

        public IEnumerable<Class> GetList() => _repository.GetList();

        public Class Get(string id) => _repository.Get(id);

        public void Create(Class item) => _repository.Create(item);

        public void Update(Class item) => _repository.Update(item);

        public void Delete(string id) => _repository.Delete(id);
    }
}