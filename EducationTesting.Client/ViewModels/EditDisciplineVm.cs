using System;
using System.Windows;
using EducationTesting.Client.Models.Disciplines;
using EducationTesting.Client.Services;

namespace EducationTesting.Client.ViewModels
{
    public class EditDisciplineVm : EditItemVm<Discipline>
    {
        private readonly IDisciplinesService _service;

        public EditDisciplineVm(Discipline item, IDisciplinesService service, Action goBack) :
            base(item, goBack) =>
            _service = service;

        protected override void Save()
        {
            if (Validate() is false) return;

            try
            {
                bool isNew = Item.Id is null;

                if (isNew)
                {
                    Item.Id = Guid.NewGuid().ToString();
                    _service.Create(Item);
                    MessageBox.Show("Предмет успешно создан!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _service.Update(Item);
                    MessageBox.Show("Предмет успешно отредактирован!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool Validate()
        {
            ErrorMessageProp.Value = null;
            var isNullOrWhiteSpace = string.IsNullOrWhiteSpace(Item.Name)
                                   || string.IsNullOrWhiteSpace(Item.Description);

            if (isNullOrWhiteSpace)
            {
                ErrorMessageProp.Value = "Заполните все поля";
                return false;
            }
            return true;
        }
    }
}
