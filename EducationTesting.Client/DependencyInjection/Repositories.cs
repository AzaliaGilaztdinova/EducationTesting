using EducationTesting.Client.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EducationTesting.Client.DependencyInjection
{
    /// <summary>
    /// Регистрация репозиториев
    /// </summary>
    public static class Repositories
    {
        /// <summary>
        /// Регистрация репозиториев
        /// </summary>
        /// <param name="services">Сервисы</param>
        /// <returns>Сервисы</returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
            => services
                .AddSingleton<IUsersRepository, UsersRepository>()
                .AddSingleton<IStudentsRepository, StudentsRepository>()
                .AddSingleton<ITeachersRepository, TeachersRepository>()
                .AddSingleton<IDisciplinesRepository, DisciplinesRepository>()
                .AddSingleton<ISubjectsRepository, SubjectsRepository>()
                .AddSingleton<ITestsRepository, TestsRepository>()
                .AddSingleton<ITestsResultsRepository, TestsResultsRepository>()
                .AddSingleton<IClassesRepository, ClassesRepository>();
    }
}