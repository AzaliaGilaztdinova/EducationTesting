using System.Collections.Generic;
using EducationTesting.Client.Models.Disciplines;

namespace EducationTesting.Client.Services
{
    public interface IDisciplinesService : IModelService<Discipline>
    {
        IEnumerable<Discipline> GetList();
    }
}