using System.Data;
using Microsoft.Data.Sqlite;

namespace EducationTesting.Client.Database
{
    /// <summary>
    /// Провайдер подключения к базе данных
    /// </summary>
    public class DbConnectionProvider : IDbConnectionProvider
    {
        public IDbConnection Connection =>
            new SqliteConnection("Data Source=EducationTesting.db");
    }
}