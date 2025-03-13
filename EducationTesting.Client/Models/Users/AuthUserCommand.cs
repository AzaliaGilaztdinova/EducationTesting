namespace EducationTesting.Client.Models.Users
{
    /// <summary>
    /// Команда авторизации
    /// </summary>
    public class AuthUserCommand
    {
        private string _login;

        /// <summary>
        /// Логин пользователя
        /// </summary>
        public string Login
        {
            get => _login;
            set => _login = value.ToLower();
        }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        public string Password { get; set; }
    }
}