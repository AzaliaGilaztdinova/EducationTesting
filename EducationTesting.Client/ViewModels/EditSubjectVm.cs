using System;
using System.Windows;
using EducationTesting.Client.Models.Subjects;
using EducationTesting.Client.Services;
using NUnit.Framework.Interfaces;

namespace EducationTesting.Client.ViewModels
{
    public class EditSubjectVm : EditItemVm<Subject>
    {
        private readonly ISubjectsService _service;

        public EditSubjectVm(Subject item, ISubjectsService service, Action goBack) : base(item, goBack) =>
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
                    MessageBox.Show("Тема успешно создана!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _service.Update(Item);
                    MessageBox.Show("Тема успешно отредактирована!", "Успех",
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

        private bool Validate() => true;
    }
}