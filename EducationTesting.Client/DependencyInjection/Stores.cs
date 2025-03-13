using EducationTesting.Client.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace EducationTesting.Client.DependencyInjection
{
    /// <summary>
    /// Регистрация хранилищ
    /// </summary>
    public static class Stores
    {
        /// <summary>
        /// Регистрация хранилищ
        /// </summary>
        /// <param name="services">Сервисы</param>
        /// <returns>Сервисы</returns>
        public static IServiceCollection AddStores(this IServiceCollection services)
            => services
                .AddSingleton<IMainNavStore, MainNavStore>()
                .AddSingleton<IMainLayoutNavStore, MainLayoutNavStore>()
                .AddSingleton<ISubjectsStore, SubjectsStore>()
                .AddSingleton<ITestsStore, TestsStore>()
                .AddSingleton<IAuthStore, AuthStore>();
    }
}