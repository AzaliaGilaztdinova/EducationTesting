using System.Collections.Generic;
using EducationTesting.Client.Models.Students;
using EducationTesting.Client.Models.Users;

namespace EducationTesting.Client.Services
{
    public interface IStudentsService
    {
        Student Get(string id);
        IEnumerable<Student> GetList();
        void Create(CreateStudentCommand command);
        void Update(Student item);
        void UpdatePassword(UpdatePasswordCommand command);
        void Delete(string id);
        bool LoginExists(string login);
    }
}