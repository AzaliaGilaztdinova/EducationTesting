using System.Collections.Generic;
using EducationTesting.Client.Models.Teachers;
using EducationTesting.Client.Models.Users;
using EducationTesting.Client.Repositories;

namespace EducationTesting.Client.Services
{
    public class TeachersService : ITeachersService
    {
        private readonly ITeachersRepository _teachersRepository;
        private readonly IUsersRepository _usersRepository;

        public TeachersService(ITeachersRepository teachersRepository, IUsersRepository usersRepository)
        {
            _teachersRepository = teachersRepository;
            _usersRepository = usersRepository;
        }

        public Teacher Get(string id) => _teachersRepository.Get(id);

        public IEnumerable<Teacher> GetList() => _teachersRepository.GetList();

        public void Create(CreateTeacherCommand command) => _teachersRepository.Create(command);

        public void Update(Teacher item) => _teachersRepository.Update(item);

        public void UpdatePassword(UpdatePasswordCommand command) => _usersRepository.UpdatePassword(command);

        public void Delete(string id) => _usersRepository.Delete(id);

        public bool LoginExists(string login) => _usersRepository.LoginExists(login);
    }
}