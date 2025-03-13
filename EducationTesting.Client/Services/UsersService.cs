using System;
using EducationTesting.Client.Models.Users;
using EducationTesting.Client.Repositories;
using EducationTesting.Client.Stores;

namespace EducationTesting.Client.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly Lazy<IStudentsRepository> _studentsRepositoryLazy;
        private readonly Lazy<ITeachersRepository> _teachersRepositoryLazy;
        private readonly IAuthStore _authStore;

        public UsersService(IUsersRepository usersRepository, Lazy<IStudentsRepository> studentsRepositoryLazy,
            Lazy<ITeachersRepository> teachersRepositoryLazy,
            IAuthStore authStore)
        {
            _usersRepository = usersRepository;
            _studentsRepositoryLazy = studentsRepositoryLazy;
            _teachersRepositoryLazy = teachersRepositoryLazy;
            _authStore = authStore;
        }

        public bool Auth(AuthUserCommand command)
        {
            var found = _usersRepository.Auth(command);

            if (found == null)
            {
                return false;
            }

            switch (found.Role)
            {
                case Roles.Student:
                    _authStore.CurrentUser = _studentsRepositoryLazy.Value.Get(found.Id);
                    break;
                case Roles.Teacher:
                    _authStore.CurrentUser = _teachersRepositoryLazy.Value.Get(found.Id);
                    break;
                case Roles.Admin:
                default:
                    _authStore.CurrentUser = found;
                    break;
            }

            return true;
        }

        public void UpdatePassword(UpdatePasswordCommand command) => _usersRepository.UpdatePassword(command);

        public bool ValidateOldPassword(ValidateOldPasswordCommand command) =>
            _usersRepository.ValidateOldPassword(command);
    }
}