using System;
using System.Windows.Controls;
using System.Windows.Input;
using EducationTesting.Client.Helpers;
using EducationTesting.Client.Models.Users;
using EducationTesting.Client.Services;
using EducationTesting.Client.Stores;
using Prism.Commands;
using Prism.Mvvm;

namespace EducationTesting.Client.ViewModels
{
    public class LoginVm : BindableBase
    {
        private readonly IMainNavStore _mainNavStore;
        private readonly Lazy<MainLayoutVm> _mainLayoutVmLazy;
        private readonly IUsersService _usersService;

        public Property<string> ErrorMessageProp { get; } = new Property<string>();
        public string Login { get; set; }

        public PasswordBox PasswordBox { get; } = new PasswordBox();

        public ICommand AuthCommand { get; }
        public ICommand OnPasswordVisibilityChangedCommand { get; }

        public LoginVm(IMainNavStore mainNavStore, Lazy<MainLayoutVm> mainLayoutVmLazy, IUsersService usersService)
        {
            _mainNavStore = mainNavStore;
            _mainLayoutVmLazy = mainLayoutVmLazy;
            _usersService = usersService;
            AuthCommand = new DelegateCommand(Auth);
            OnPasswordVisibilityChangedCommand = new DelegateCommand(OnPasswordVisibilityChanged);
        }

        private void OnPasswordVisibilityChanged() => RaisePropertyChanged(nameof(PasswordBox));

        private void Auth()
        {
            ErrorMessageProp.Value = null;

            if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                ErrorMessageProp.Value = "Заполните все поля";
                return;
            }

            var command = new AuthUserCommand
            {
                Login = Login,
                Password = PasswordBox.Password
            };

            if (!_usersService.Auth(command))
            {
                ErrorMessageProp.Value = "Неверный логин или пароль";
                return;
            }

            _mainNavStore.CurrentVmProp.Value = _mainLayoutVmLazy.Value;
        }
    }
}