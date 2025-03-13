namespace EducationTesting.Client.Models.Users
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class User
    {
        private string _login;

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login
        {
            get => _login;
            set => _login = value.ToLower();
        }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Отчество пользователя
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Роль пользователя
        /// </summary>
        public Roles Role { get; set; }
    }
}