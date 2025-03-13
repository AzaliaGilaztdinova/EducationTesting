using System.Data;

namespace EducationTesting.Client.Database
{
    /// <summary>
    /// Провайдер подключения к БД
    /// </summary>
    public interface IDbConnectionProvider
    {
        /// <summary>
        /// Подключение к базе данных
        /// </summary>
        IDbConnection Connection { get; }
    }
}