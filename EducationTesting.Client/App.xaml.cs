using System;
using System.Windows;
using EducationTesting.Client.DependencyInjection;
using EducationTesting.Client.ViewModels;
using EducationTesting.Client.Views;
using Microsoft.Extensions.DependencyInjection;

namespace EducationTesting.Client
{
    public partial class App
    {
        private readonly IServiceProvider _serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetService<MainWindow>();
            mainWindow.DataContext = _serviceProvider.GetService<MainVm>();
            mainWindow.Show();
        }

        private static void ConfigureServices(IServiceCollection services)
            => services
                .AddSingleton<MainWindow>()
                .AddViewModels()
                .AddServices()
                .AddStores()
                .AddRepositories();
    }
}