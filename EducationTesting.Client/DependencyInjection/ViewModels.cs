using EducationTesting.Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace EducationTesting.Client.DependencyInjection
{
    /// <summary>
    /// Регистрация моделей представления
    /// </summary>
    public static class ViewModels
    {
        /// <summary>
        /// Регистрация моделей представления
        /// </summary>
        /// <param name="services">Сервисы</param>
        /// <returns>Сервисы</returns>
        public static IServiceCollection AddViewModels(this IServiceCollection services)
            => services
                .AddSingleton<MainVm>()
                .AddTransient<LoginVm>()
                .AddTransient<MainLayoutVm>()
                .AddTransient<ProfileVm>()
                .AddTransient<StudentsVm>()
                .AddTransient<SubjectsVm>()
                .AddTransient<TestsVm>()
                .AddTransient<DisciplinesVm>()
                .AddTransient<TeachersVm>()
                .AddTransient<TestPerformVm>()
                .AddTransient<TestResultsVm>()
                .AddTransient<ClassesVm>();
    }
}