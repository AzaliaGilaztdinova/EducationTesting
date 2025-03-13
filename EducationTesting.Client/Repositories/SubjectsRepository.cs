using System.Collections.Generic;
using System.Data;
using Dapper;
using EducationTesting.Client.Database;
using EducationTesting.Client.Models.Subjects;

namespace EducationTesting.Client.Repositories
{
    /// <summary>
    /// Репозиторий для работы с темами.
    /// </summary>
    public class SubjectsRepository : ISubjectsRepository
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
        public SubjectsRepository(IDbConnectionProvider provider)
        {
            _provider = provider;
            InitializeTable(); // Инициализация таблиц при создании репозитория
        }

        /// <summary>
        /// Получение темы по ее идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор темы.</param>
        /// <returns>Данные темы, включая название и описание.</returns>
        public Subject Get(string id)
        {
            using (var connection = Connection)
                return connection.QuerySingle<Subject>(@"
                    SELECT
                        s.Id,
                        s.DisciplineId,
                        d.Name AS DisciplineName,
                        s.Name,
                        s.Description
                    FROM Subjects s
                        INNER JOIN Disciplines d ON s.DisciplineId = d.Id
                    WHERE
                        s.Id = @id
                ", new { id });
        }

        /// <summary>
        /// Получение списка тем по идентификатору предмета.
        /// </summary>
        /// <param name="disciplineId">Идентификатор предмета.</param>
        /// <returns>Список тем, связанных с указанным предметом.</returns>
        public IEnumerable<Subject> GetListByDisciplineId(string disciplineId)
        {
            using (var connection = Connection)
                return connection.Query<Subject>(@"
                    SELECT
                        s.Id,
                        s.DisciplineId,
                        d.Name AS DisciplineName,
                        s.Name,
                        s.Description
                    FROM Subjects s
                        INNER JOIN Disciplines d ON s.DisciplineId = d.Id
                    WHERE
                        s.DisciplineId = @disciplineId
                    ORDER BY DisciplineName
                ", new { disciplineId });
        }

        /// <summary>
        /// Создание новой темы.
        /// </summary>
        /// <param name="item">Данные темы для создания.</param>
        public void Create(Subject item)
        {
            using (var connection = Connection)
                connection.Execute(@"
                    INSERT INTO Subjects (Id, DisciplineId, Name, Description)
                    VALUES (@Id, @DisciplineId, @Name, @Description)
                ", item);
        }

        /// <summary>
        /// Обновление данных темы.
        /// </summary>
        /// <param name="item">Данные темы для обновления.</param>
        public void Update(Subject item)
        {
            using (var connection = Connection)
                connection.Execute(@"
                    UPDATE Subjects
                    SET
                        DisciplineId = @DisciplineId,
                        Name = @Name,
                        Description = @Description
                    WHERE
                        Id = @Id
                ", item);
        }

        /// <summary>
        /// Удаление темы по ее идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор темы.</param>
        public void Delete(string id)
        {
            using (var connection = Connection)
                connection.Execute(@"
                    DELETE FROM Subjects
                    WHERE Id = @id
                ", new { id });
        }

        /// <summary>
        /// Инициализация таблицы тем в базе данных.
        /// </summary>
        private void InitializeTable()
        {
            using (var connection = Connection)
                connection.Execute(@"
                    CREATE TABLE IF NOT EXISTS Subjects
                    (
                        Id           TEXT NOT NULL PRIMARY KEY,
                        DisciplineId TEXT NOT NULL,
                        Name         TEXT NOT NULL,
                        Description  TEXT,
                        FOREIGN KEY (DisciplineId) REFERENCES Disciplines (Id)
                    );
                ");
        }
    }
}