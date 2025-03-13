using System.Data;
using Dapper;
using EducationTesting.Client.Database;
using EducationTesting.Client.Models.Users;

namespace EducationTesting.Client.Repositories
{
    /// <summary>
    /// Репозиторий для работы с пользователями.
    /// </summary>
    public class UsersRepository : IUsersRepository
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
        public UsersRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
            InitializeTable(); // Инициализация таблицы при создании репозитория
        }

        /// <summary>
        /// Аутентификация пользователя.
        /// </summary>
        /// <param name="command">Команда аутентификации, содержащая логин и пароль.</param>
        /// <returns>Данные пользователя, если аутентификация успешна, иначе null.</returns>
        public User Auth(AuthUserCommand command)
        {
            using (var connection = Connection)
                return connection.QuerySingleOrDefault<User>(@"
                    SELECT Id,
                           Login,
                           Firstname,
                           Lastname,
                           MiddleName,
                           Role
                    FROM Users
                    WHERE Login = @Login
                      AND Password = @Password
                ", command);
        }

        /// <summary>
        /// Проверка старого пароля пользователя.
        /// </summary>
        /// <param name="command">Команда проверки старого пароля, содержащая идентификатор пользователя и старый пароль.</param>
        /// <returns>True, если старый пароль верный, иначе False.</returns>
        public bool ValidateOldPassword(ValidateOldPasswordCommand command)
        {
            using (var connection = Connection)
                return connection.ExecuteScalar<bool>(@"
                    SELECT EXISTS(SELECT TRUE FROM Users WHERE Id = @UserId AND Password = @OldPassword)
                ", command);
        }

        /// <summary>
        /// Обновление пароля пользователя.
        /// </summary>
        /// <param name="command">Команда обновления пароля, содержащая идентификатор пользователя и новый пароль.</param>
        public void UpdatePassword(UpdatePasswordCommand command)
        {
            using (var connection = Connection)
                connection.Execute(@"
                    UPDATE Users
                    SET Password = @NewPassword
                    WHERE Id = @UserId;
                ", command);
        }

        /// <summary>
        /// Удаление пользователя по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        public void Delete(string id)
        {
            using (var connection = Connection)
                connection.Execute(@"
                    DELETE FROM Users
                    WHERE Id = @id;
                ", new { id });
        }

        /// <summary>
        /// Проверка существования логина в базе данных.
        /// </summary>
        /// <param name="login">Логин для проверки.</param>
        /// <returns>True, если логин существует, иначе False.</returns>
        public bool LoginExists(string login)
        {
            using (var connection = Connection)
                return connection.ExecuteScalar<bool>(@"
                    SELECT EXISTS(SELECT TRUE FROM Users WHERE Login = @login)
                ", new { login });
        }

        /// <summary>
        /// Инициализация таблицы пользователей в базе данных.
        /// </summary>
        private void InitializeTable()
        {
            const string Sql = @"
                CREATE TABLE IF NOT EXISTS Users
                (
                    Id         TEXT    NOT NULL PRIMARY KEY,
                    Login      TEXT    NOT NULL UNIQUE,
                    Password   TEXT    NOT NULL,
                    FirstName  TEXT    NOT NULL,
                    LastName   TEXT    NOT NULL,
                    MiddleName TEXT,
                    Role       INTEGER NOT NULL
                );
            ";
            using (var connection = Connection)
                connection.Execute(Sql);
        }
    }
}