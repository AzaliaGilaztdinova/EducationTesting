using System.Collections.Generic;
using System.Data;
using Dapper;
using EducationTesting.Client.Database;
using EducationTesting.Client.Models.Students;
using EducationTesting.Client.Models.Users;

namespace EducationTesting.Client.Repositories
{
    /// <summary>
    /// Репозиторий для работы с учениками.
    /// </summary>
    public class StudentsRepository : IStudentsRepository
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
        public StudentsRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        /// <summary>
        /// Получение данных ученика по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор ученика.</param>
        /// <returns>Данные ученика, включая информацию о классе.</returns>
        public Student Get(string id)
        {
            using (var connection = Connection)
                return connection.QuerySingle<Student>(@"
                    SELECT u.Id,
                           u.Login,
                           u.Firstname,
                           u.Lastname,
                           u.MiddleName,
                           u.Role,
                           c.Name AS Class,
                           c.Id AS ClassId
                    FROM Users u
                    LEFT JOIN Students s ON u.Id = s.UserId
                    LEFT JOIN Classes c ON s.ClassId = c.Id
                    WHERE u.Id = @id;
                ", new { id });
        }

        /// <summary>
        /// Получение списка всех учеников.
        /// </summary>
        /// <returns>Список учеников, отсортированный по классу и фамилии.</returns>
        public IEnumerable<Student> GetList()
        {
            using (var connection = Connection)
                return connection.Query<Student>(@"
                    SELECT u.Id,
                           u.Login,
                           u.Firstname,
                           u.Lastname,
                           u.MiddleName,
                           u.Role,
                           c.Name AS Class,
                           c.Id AS ClassId
                    FROM Users u
                    LEFT JOIN Students s ON u.Id = s.UserId
                    LEFT JOIN Classes c ON s.ClassId = c.Id
                    WHERE u.Role = @Role
                    ORDER BY c.Name, u.Lastname;
                ", new { Role = Roles.Student });
        }

        /// <summary>
        /// Создание нового ученика.
        /// </summary>
        /// <param name="command">Данные для создания ученика.</param>
        public void Create(CreateStudentCommand command)
        {
            using (var connection = Connection)
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                // Вставляем данные в таблицу Users
                connection.Execute(@"
                    INSERT INTO Users (Id, Login, Password, FirstName, LastName, MiddleName, Role)
                    VALUES (@Id, @Login, @Password, @FirstName, @LastName, @MiddleName, @Role);
                ", command, transaction);

                // Вставляем данные в таблицу Students
                connection.Execute(@"
                    INSERT INTO Students (UserId, ClassId)
                    VALUES (@Id, @ClassId);
                ", command, transaction);

                // Фиксируем транзакцию
                transaction.Commit();
            }
        }

        /// <summary>
        /// Обновление данных ученика.
        /// </summary>
        /// <param name="item">Данные ученика для обновления.</param>
        public void Update(Student item)
        {
            using (var connection = Connection)
            {
                connection.Open();
                var transaction = connection.BeginTransaction();

                // Обновляем данные в таблице Users
                connection.Execute(@"
                    UPDATE Users
                    SET FirstName = @FirstName,
                        LastName = @LastName,
                        MiddleName = @MiddleName,
                        Role = @Role
                    WHERE Id = @Id;
                ", item, transaction);

                // Обновляем данные в таблице Students
                connection.Execute(@"
                    UPDATE Students
                    SET ClassId = @ClassId
                    WHERE UserId = @Id;
                ", item, transaction);

                // Фиксируем транзакцию
                transaction.Commit();
            }
        }
    }
}