using System.Collections.Generic;
using System.Data;
using Dapper;
using EducationTesting.Client.Database;
using EducationTesting.Client.Models.Disciplines;

namespace EducationTesting.Client.Repositories
{
    /// <summary>
    /// Репозиторий для работы с предметами (Disciplines).
    /// </summary>
    public class DisciplinesRepository : IDisciplinesRepository
    {
        private readonly IDbConnectionProvider _provider;

        /// <summary>
        /// Подключение к базе данных.
        /// </summary>
        private IDbConnection Connection => _provider.Connection;

        /// <summary>
        /// Конструктор репозитория.
        /// </summary>
        /// <param name="provider">Провайдер подключения к базе данных.</param>
        public DisciplinesRepository(IDbConnectionProvider provider)
        {
            _provider = provider;
            InitializeTable(); // Инициализация таблиц при создании репозитория
        }

        /// <summary>
        /// Получение предмета по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор предмета.</param>
        /// <returns>Данные предмета, включая название и описание.</returns>
        public Discipline Get(string id)
        {
            using (var connection = Connection)
                return connection.QuerySingle<Discipline>(@"
                    SELECT
                        Id,
                        Name,
                        Description
                    FROM
                        Disciplines
                    WHERE
                        Id = @id;
                ", new { id });
        }

        /// <summary>
        /// Получение списка всех предметов.
        /// </summary>
        /// <returns>Список всех предметов.</returns>
        public IEnumerable<Discipline> GetList()
        {
            using (var connection = Connection)
                return connection.Query<Discipline>(@"
                    SELECT
                        Id,
                        Name,
                        Description
                    FROM
                        Disciplines;
                ");
        }

        /// <summary>
        /// Создание нового предмета.
        /// </summary>
        /// <param name="item">Данные предмета для создания.</param>
        public void Create(Discipline item)
        {
            using (var connection = Connection)
                connection.Execute(@"
                    INSERT INTO Disciplines (Id, Name, Description) 
                    VALUES (@Id, @Name, @Description);
                ", item);
        }

        /// <summary>
        /// Обновление данных предмета.
        /// </summary>
        /// <param name="item">Данные предмета для обновления.</param>
        public void Update(Discipline item)
        {
            using (var connection = Connection)
                connection.Execute(@"
                    UPDATE Disciplines
                    SET
                        Name = @Name,
                        Description = @Description
                    WHERE
                        Id = @Id;
                ", item);
        }

        /// <summary>
        /// Удаление предмета по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор предмета.</param>
        public void Delete(string id)
        {
            using (var connection = Connection)
                connection.Execute(@"
                    DELETE FROM Disciplines
                    WHERE Id = @id;
                ", new { id });
        }

        /// <summary>
        /// Инициализация таблицы предметов в базе данных.
        /// </summary>
        private void InitializeTable()
        {
            using (var connection = Connection)
                connection.Execute(@"
                    CREATE TABLE IF NOT EXISTS Disciplines
                    (
                        Id          TEXT NOT NULL PRIMARY KEY,
                        Name        TEXT NOT NULL,
                        Description TEXT
                    );
                ");
        }
    }
}