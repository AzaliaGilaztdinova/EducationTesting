using System.Collections.Generic;
using EducationTesting.Client.Models.Students;

namespace EducationTesting.Client.Repositories
{
    public interface IStudentsRepository
    {
        Student Get(string id);
        IEnumerable<Student> GetList();
        void Create(CreateStudentCommand command);
        void Update(Student command);
    }
}