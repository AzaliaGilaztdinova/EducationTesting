using System;
using System.Windows;
using EducationTesting.Client.Models.Classes;
using EducationTesting.Client.Services;
using NUnit.Framework;

namespace EducationTesting.Client.ViewModels
{
    public class EditClassVm : EditItemVm<Class>
    {
        private readonly IClassesService _service;

        public EditClassVm(Class item, IClassesService service, Action goBack) : base(item, goBack) =>
            _service = service;
        private bool Validate()
        {
            ErrorMessageProp.Value = null;

            if (string.IsNullOrWhiteSpace(Item.Name))
            {
                ErrorMessageProp.Value = "Введите название класса";
                return false;
            }
            return true;
        }
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
                    MessageBox.Show("Класс успешно создан!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _service.Update(Item);
                    MessageBox.Show("Класс успешно отредактирован!", "Успех",
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
    }
}