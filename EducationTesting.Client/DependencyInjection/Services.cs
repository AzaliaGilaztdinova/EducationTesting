using System;
using EducationTesting.Client.Database;
using EducationTesting.Client.Helpers;
using EducationTesting.Client.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EducationTesting.Client.DependencyInjection
{
    /// <summary>
    /// Регистрация сервисов
    /// </summary>
    public static class Services
    {
        /// <summary>
        /// Регистрация сервисов
        /// </summary>
        /// <param name="services">Сервисы</param>
        /// <returns>Сервисы</returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
            => services
                .AddSingleton<IDbConnectionProvider, DbConnectionProvider>()
                .AddTransient(typeof(Lazy<>), typeof(Lazier<>))
                .AddTransient<IUsersService, UsersService>()
                .AddTransient<IStudentsService, StudentsService>()
                .AddTransient<ITeachersService, TeachersService>()
                .AddTransient<IDisciplinesService, DisciplinesService>()
                .AddTransient<ISubjectsService, SubjectsService>()
                .AddTransient<ITestsService, TestsService>()
                .AddTransient<ITestsResultsService, TestsResultsService>()
                .AddTransient<IClassesService, ClassesService>();
    }
}