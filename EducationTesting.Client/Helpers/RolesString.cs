using EducationTesting.Client.Models.Users;

namespace EducationTesting.Client.Helpers
{
    /// <summary>
    /// Строковое представление ролей
    /// </summary>
    public static class RolesString
    {
        private const string Admin = "Администратор";
        private const string Teacher = "Учитель";
        private const string Student = "Ученик";
        private const string Undefined = "Не определено";

        /// <summary>
        /// Получить строковое представление роли
        /// </summary>
        /// <param name="role">Роль пользователя</param>
        /// <returns>Строковое представление роли</returns>
        public static string GetString(this Roles role)
        {
            switch (role)
            {
                case Roles.Admin:
                    return Admin;
                case Roles.Teacher:
                    return Teacher;
                case Roles.Student:
                    return Student;
                default:
                    return Undefined;
            }
        }
    }
}