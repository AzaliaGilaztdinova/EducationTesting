using System;
using System.Windows;
using System.Windows.Controls;
using EducationTesting.Client.Models.Teachers;
using EducationTesting.Client.Models.Users;
using EducationTesting.Client.Services;

namespace EducationTesting.Client.ViewModels
{
    public class EditTeacherVm : EditItemVm<Teacher>
    {
        private readonly ITeachersService _service;

        public PasswordBox PasswordBox { get; } = new PasswordBox();

        public bool IsNew => Item.Id is null;

        public EditTeacherVm(Teacher item, ITeachersService service, Action goBack) :
            base(item, goBack) =>
            _service = service;

        protected override void Save()
        {
            if (Validate() is false)
                return;

            try
            {
                bool isNew = Item.Id is null;

                if (isNew)
                {
                    var command = new CreateTeacherCommand()
                    {
                        Id = Guid.NewGuid().ToString(),
                        Login = Item.Login,
                        Password = PasswordBox.Password,
                        FirstName = Item.FirstName,
                        LastName = Item.LastName,
                        MiddleName = Item.MiddleName,
                        Role = Roles.Teacher
                    };

                    _service.Create(command);
                    MessageBox.Show("Учитель успешно создан!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _service.Update(Item);

                    if (!string.IsNullOrWhiteSpace(PasswordBox.Password))
                    {
                        var passwordCommand = new UpdatePasswordCommand
                        {
                            UserId = Item.Id,
                            NewPassword = PasswordBox.Password
                        };
                        _service.UpdatePassword(passwordCommand);
                    }

                    MessageBox.Show("Данные учителя отредактированы!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool Validate()
        {
            ErrorMessageProp.Value = null;
            var isNullOrWhiteSpace = (IsNew && string.IsNullOrWhiteSpace(Item.Login)) ||
                                     (IsNew && string.IsNullOrWhiteSpace(PasswordBox.Password)) ||
                                     string.IsNullOrWhiteSpace(Item.FirstName) ||
                                     string.IsNullOrWhiteSpace(Item.LastName);

            if (isNullOrWhiteSpace)
            {
                ErrorMessageProp.Value = "Заполните все поля";
                return false;
            }

            if (IsNew && _service.LoginExists(Item.Login))
            {
                ErrorMessageProp.Value = "Пользователь с таким логином уже существует";
                return false;
            }

            return true;
        }
    }
}