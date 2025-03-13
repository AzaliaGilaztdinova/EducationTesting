using EducationTesting.Client.Models.Users;

namespace EducationTesting.Client.Repositories
{
    /// <summary>
    /// Репозиторий пользователей
    /// </summary>
    public interface IUsersRepository
    {
        /// <summary>
        /// Получить пользователя
        /// </summary>
        /// <param name="command">Команда</param>
        /// <returns>Пользователь</returns>
        User Auth(AuthUserCommand command);

        void UpdatePassword(UpdatePasswordCommand command);

        bool ValidateOldPassword(ValidateOldPasswordCommand command);
        
        void Delete(string id);
        
        bool LoginExists(string login);
    }
}