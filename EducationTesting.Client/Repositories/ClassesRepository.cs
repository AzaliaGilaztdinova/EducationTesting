using System.Collections.Generic;
using System.Data;
using Dapper;
using EducationTesting.Client.Database;
using EducationTesting.Client.Models.Classes;

namespace EducationTesting.Client.Repositories
{
    /// <summary>
    /// Репозиторий для работы с классами.
    /// </summary>
    public class ClassesRepository : IClassesRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        /// <summary>
        /// Подключение к базе данных.
        /// </summary>
        private IDbConnection Connection => _connectionProvider.Connection;

        /// <summary>
        /// Конструктор репозитория.
        /// </summary>
        /// <param name="connectionProvider">Провайдер подключения к базе данных.</param>
        public ClassesRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
            InitializeTable(); // Инициализация таблиц при создании репозитория
        }

        /// <summary>
        /// Получение списка всех классов.
        /// </summary>
        /// <returns>Список классов, отсортированный по названию.</returns>
        public IEnumerable<Class> GetList()
        {
            const string Sql = @"
                SELECT Id,
                       Name
                FROM Classes
                ORDER BY Name;
            ";
            using (var connection = Connection)
                return connection.Query<Class>(Sql);
        }

        /// <summary>
        /// Получение класса по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор класса.</param>
        /// <returns>Данные класса, если найден, иначе null.</returns>
        public Class Get(string id)
        {
            const string Sql = @"
                SELECT Id,
                       Name
                FROM Classes
                WHERE Id = @id;
            ";
            using (var connection = Connection)
                return connection.QuerySingleOrDefault<Class>(Sql, new { id });
        }

        /// <summary>
        /// Создание нового класса.
        /// </summary>
        /// <param name="item">Данные класса для создания.</param>
        public void Create(Class item)
        {
            using (var connection = Connection)
                connection.Execute(@"
                INSERT INTO Classes (Id, Name)
                VALUES (@Id, @Name);
            ", item);
        }

        /// <summary>
        /// Обновление данных класса.
        /// </summary>
        /// <param name="item">Данные класса для обновления.</param>
        public void Update(Class item)
        {
            const string Sql = @"
                UPDATE Classes SET Name = @Name WHERE Id = @Id;
            ";
            using (var connection = Connection)
                connection.Execute(Sql, item);
        }

        /// <summary>
        /// Удаление класса по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор класса.</param>
        public void Delete(string id)
        {
            const string Sql = @"
                DELETE FROM Classes WHERE Id = @id;
            ";
            using (var connection = Connection)
                connection.Execute(Sql, new { id });
        }

        /// <summary>
        /// Инициализация таблиц классов и учеников в базе данных.
        /// </summary>
        private void InitializeTable()
        {
            const string Sql = @"
                CREATE TABLE IF NOT EXISTS Classes
                (
                    Id   TEXT NOT NULL PRIMARY KEY,
                    Name TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Students
                (
                    UserId  TEXT NOT NULL PRIMARY KEY,
                    ClassId TEXT NOT NULL,
                    FOREIGN KEY (UserId) REFERENCES Users (Id) ON DELETE CASCADE ON UPDATE CASCADE,
                    FOREIGN KEY (ClassId) REFERENCES Classes (Id) ON DELETE CASCADE ON UPDATE CASCADE
                );
            ";
            using (var connection = Connection)
                connection.Execute(Sql);
        }
    }
}