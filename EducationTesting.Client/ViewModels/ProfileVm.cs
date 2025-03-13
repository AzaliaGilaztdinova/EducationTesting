using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using EducationTesting.Client.Helpers;
using EducationTesting.Client.Models.Students;
using EducationTesting.Client.Models.Users;
using EducationTesting.Client.Services;
using EducationTesting.Client.Stores;
using Prism.Commands;

namespace EducationTesting.Client.ViewModels
{
    public class ProfileVm
    {
        private readonly Lazy<IUsersService> _usersService;
        public IAuthStore AuthStore { get; }
        public string Role => AuthStore.CurrentUser.Role.GetString();
        public string Class { get; }
        public Property<bool> PasswordChangedProp { get; set; } = new Property<bool>();

        public PasswordBox OldPasswordBox { get; } = new PasswordBox();
        public PasswordBox NewPasswordBox { get; } = new PasswordBox();
        public PasswordBox ConfirmPasswordBox { get; } = new PasswordBox();
        public Property<string> ErrorMessageProp { get; } = new Property<string>();

        public ICommand UpdatePasswordCommand { get; }

        public ProfileVm(IAuthStore authStore, Lazy<IUsersService> usersService)
        {
            _usersService = usersService;
            AuthStore = authStore;
            UpdatePasswordCommand = new DelegateCommand(UpdatePassword);
            Class = (authStore.CurrentUser as Student)?.Class ?? "Не определен";
        }

        private void UpdatePassword()
        {
            PasswordChangedProp.Value = false;

            try
            {
                if (Validate() == false)
                {
                    return;
                }

                var command = new UpdatePasswordCommand
                {
                    UserId = AuthStore.CurrentUser.Id,
                    NewPassword = NewPasswordBox.Password
                };

                _usersService.Value.UpdatePassword(command);

                // Показываем сообщение об успехе
                MessageBox.Show(
                    "Пароль успешно изменен!",
                    "Успех",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );

                PasswordChangedProp.Value = true;

                // Очищаем поля паролей
                OldPasswordBox.Password = string.Empty;
                NewPasswordBox.Password = string.Empty;
                ConfirmPasswordBox.Password = string.Empty;
            }
            catch (Exception ex)
            {
                // Показываем сообщение об ошибке
                MessageBox.Show(
                    $"Ошибка при изменении пароля: {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        private bool Validate()
        {
            ErrorMessageProp.Value = null;

            if (string.IsNullOrWhiteSpace(OldPasswordBox.Password) ||
                string.IsNullOrWhiteSpace(NewPasswordBox.Password) ||
                string.IsNullOrWhiteSpace(ConfirmPasswordBox.Password))
            {
                ErrorMessageProp.Value = "Заполните все поля";
                return false;
            }

            if (NewPasswordBox.Password != ConfirmPasswordBox.Password)
            {
                ErrorMessageProp.Value = "Пароли не совпадают";
                return false;
            }

            var command = new ValidateOldPasswordCommand
            {
                UserId = AuthStore.CurrentUser.Id,
                OldPassword = OldPasswordBox.Password
            };

            if (_usersService.Value.ValidateOldPassword(command) == false)
            {
                ErrorMessageProp.Value = "Текущий пароль неверен";
                return false;
            }

            return true;
        }
    }
}