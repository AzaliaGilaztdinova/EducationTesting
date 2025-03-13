using System.Collections.Generic;
using EducationTesting.Client.Models.Classes;

namespace EducationTesting.Client.Repositories
{
    /// <summary>
    /// Репозиторий школьных классов
    /// </summary>
    public interface IClassesRepository : IRepository<Class>
    {
        IEnumerable<Class> GetList();
    }
}