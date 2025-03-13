using System.Collections.Generic;
using EducationTesting.Client.Models.Students;
using EducationTesting.Client.Models.Users;
using EducationTesting.Client.Repositories;

namespace EducationTesting.Client.Services
{
    public class StudentsService : IStudentsService
    {
        private readonly IStudentsRepository _studentsRepository;
        private readonly IUsersRepository _usersRepository;

        public StudentsService(IStudentsRepository studentsRepository, IUsersRepository usersRepository)
        {
            _studentsRepository = studentsRepository;
            _usersRepository = usersRepository;
        }

        public Student Get(string id) => _studentsRepository.Get(id);

        public IEnumerable<Student> GetList() => _studentsRepository.GetList();

        public void Create(CreateStudentCommand command) => _studentsRepository.Create(command);

        public void Update(Student item) => _studentsRepository.Update(item);
        
        public void UpdatePassword(UpdatePasswordCommand command) => _usersRepository.UpdatePassword(command);

        public void Delete(string id) => _usersRepository.Delete(id);
        
        public bool LoginExists(string login) => _usersRepository.LoginExists(login);
    }
}