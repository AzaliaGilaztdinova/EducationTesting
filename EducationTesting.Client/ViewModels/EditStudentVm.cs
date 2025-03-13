using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using EducationTesting.Client.Models.Classes;
using EducationTesting.Client.Models.Students;
using EducationTesting.Client.Models.Users;
using EducationTesting.Client.Services;

namespace EducationTesting.Client.ViewModels
{
    public class EditStudentVm : EditItemVm<Student>
    {
        private readonly IStudentsService _service;
        private readonly IClassesService _classesService;
        private readonly IGuidProvider _guidProvider;

        public PasswordBox PasswordBox { get; } = new PasswordBox();

        public IEnumerable<Class> Classes => _classesService.GetList();
        public bool IsNew => Item.Id is null;

        public EditStudentVm(Student item, IStudentsService service, IClassesService classesService, Action goBack, IGuidProvider guidProvider) :
            base(item, goBack)
        {
            _service = service;
            _classesService = classesService;
            _guidProvider = guidProvider;
        }

        protected override void Save()
        {
            if (Validate() is false) return;

            try
            {
                bool isNew = Item.Id is null;

                if (isNew)
                {
                    var command = new CreateStudentCommand
                    {
                        Id = _guidProvider.NewGuid(),
                        Login = Item.Login,
                        Password = PasswordBox.Password,
                        FirstName = Item.FirstName,
                        LastName = Item.LastName,
                        MiddleName = Item.MiddleName,
                        ClassId = Item.ClassId,
                        Role = Roles.Student
                    };

                    _service.Create(command);
                    MessageBox.Show("Ученик успешно создан!", "Успех",
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

                    MessageBox.Show("Данные ученика отредактированы!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                PasswordBox.Password = string.Empty; // Очистка поля пароля
            }
        }
        private bool Validate()
        {
            ErrorMessageProp.Value = null;
            var isNullOrWhiteSpace = (IsNew && string.IsNullOrWhiteSpace(Item.Login)) ||
                                     (IsNew && string.IsNullOrWhiteSpace(PasswordBox.Password)) ||
                                     string.IsNullOrWhiteSpace(Item.ClassId) ||
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