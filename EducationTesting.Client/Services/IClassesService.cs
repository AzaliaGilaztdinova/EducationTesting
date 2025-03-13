using System.Collections.Generic;
using EducationTesting.Client.Models.Classes;

namespace EducationTesting.Client.Services
{
    public interface IClassesService : IModelService<Class>
    {
        IEnumerable<Class> GetList();
    }
}