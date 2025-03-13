using System.Collections.Generic;
using System.Data;
using Dapper;
using EducationTesting.Client.Database;
using EducationTesting.Client.Models.Teachers;
using EducationTesting.Client.Models.Users;

namespace EducationTesting.Client.Repositories
{
    /// <summary>
    /// Репозиторий для работы с учителями.
    /// </summary>
    public class TeachersRepository : ITeachersRepository
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
        public TeachersRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        /// <summary>
        /// Получение учителя по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор учителя.</param>
        /// <returns>Данные учителя.</returns>
        public Teacher Get(string id)
        {
            using (var connection = Connection)
                return connection.QuerySingle<Teacher>(@"
                    SELECT * FROM Users WHERE Id = @id;
                ", new { id });
        }

        /// <summary>
        /// Получение списка всех учителей.
        /// </summary>
        /// <returns>Список учителей, отсортированный по фамилии.</returns>
        public IEnumerable<Teacher> GetList()
        {
            using (var connection = Connection)
                return connection.Query<Teacher>(@"
                    SELECT * FROM Users WHERE Role = @Role ORDER BY LastName;
                ", new { Role = Roles.Teacher });
        }

        /// <summary>
        /// Создание нового учителя.
        /// </summary>
        /// <param name="command">Команда создания учителя, содержащая данные для вставки.</param>
        public void Create(CreateTeacherCommand command)
        {
            using (var connection = Connection)
                connection.Execute(@"
                    INSERT INTO Users (Id, Login, Password, FirstName, LastName, MiddleName, Role)
                    VALUES (@Id, @Login, @Password, @FirstName, @LastName, @MiddleName, @Role);
                ", command);
        }

        /// <summary>
        /// Обновление данных учителя.
        /// </summary>
        /// <param name="item">Данные учителя для обновления.</param>
        public void Update(Teacher item)
        {
            using (var connection = Connection)
                connection.Execute(@"
                    UPDATE Users
                    SET FirstName = @FirstName,
                        LastName = @LastName,
                        MiddleName = @MiddleName,
                        Role = @Role
                    WHERE Id = @Id;
                ", item);
        }
    }
}