using System.Collections.Generic;
using EducationTesting.Client.Models.Teachers;

namespace EducationTesting.Client.Repositories
{
    public interface ITeachersRepository
    {
        Teacher Get(string id);
        IEnumerable<Teacher> GetList();
        void Create(CreateTeacherCommand command);
        void Update(Teacher item);
    }
}