using System.Collections.Generic;
using EducationTesting.Client.Models.Teachers;
using EducationTesting.Client.Models.Users;

namespace EducationTesting.Client.Services
{
    public interface ITeachersService
    {
        Teacher Get(string id);
        IEnumerable<Teacher> GetList();
        void Create(CreateTeacherCommand command);
        void Update(Teacher item);
        void UpdatePassword(UpdatePasswordCommand command);
        void Delete(string id);
        bool LoginExists(string login);
    }
}