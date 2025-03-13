using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EducationTesting.Client.Models.Students;
using EducationTesting.Client.Services;
using EducationTesting.Client.Stores;
using Prism.Commands;
using Prism.Mvvm;

namespace EducationTesting.Client.ViewModels
{
    public class StudentsVm : BindableBase
    {
        private readonly IStudentsService _service;
        private readonly IMainLayoutNavStore _mainLayoutNavStore;
        private readonly Lazy<StudentsVm> _studentsVmLazy;
        private readonly Lazy<IClassesService> _classesVmLazy;
        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (value == _searchText) return;
                _searchText = value;
                RaisePropertyChanged(nameof(Students));
            }
        }

        public IEnumerable<Student> Students =>
            _service.GetList().Where(s =>
                string.IsNullOrWhiteSpace(SearchText) || s.LastName.ToLower().Contains(SearchText.ToLower()) ||
                s.FirstName.ToLower().Contains(SearchText.ToLower()) ||
                s.MiddleName.ToLower().Contains(SearchText.ToLower()) ||
                s.Class.ToLower().Contains(SearchText.ToLower()));

        public ICommand AddCommand { get; }
        public ICommand GoToCommand { get; }
        public ICommand DeleteCommand { get; }

        public StudentsVm(IStudentsService service, IMainLayoutNavStore mainLayoutNavStore,
            Lazy<StudentsVm> studentsVmLazy, Lazy<IClassesService> classesVmLazy)
        {
            _service = service;
            _mainLayoutNavStore = mainLayoutNavStore;
            _studentsVmLazy = studentsVmLazy;
            _classesVmLazy = classesVmLazy;
            AddCommand = new DelegateCommand(Add);
            GoToCommand = new DelegateCommand<string>(GoTo);
            DeleteCommand = new DelegateCommand<Student>(Delete);
        }

        private void Add() => _mainLayoutNavStore.CurrentVmProp.Value =
            new EditStudentVm(new Student(), _service, _classesVmLazy.Value,
                () => _mainLayoutNavStore.CurrentVmProp.Value = _studentsVmLazy.Value);

        private void GoTo(string id)
        {
            var item = _service.Get(id);
            _mainLayoutNavStore.CurrentVmProp.Value =
                new EditStudentVm(item, _service, _classesVmLazy.Value,
                    () => _mainLayoutNavStore.CurrentVmProp.Value = _studentsVmLazy.Value);
        }

        private void Delete(Student item)
        {
            _mainLayoutNavStore.CurrentVmProp.Value = new ConfirmationVm(
                $"Удалить ученика '{item.LastName} {item.FirstName}'?",
                () =>
                {
                    try
                    {
                        _service.Delete(item.Id);

                        // Обновляем список студентов
                        RaisePropertyChanged(nameof(Students));

                        // Показываем сообщение об успехе
                        MessageBox.Show(
                            "Ученик успешно удален!",
                            "Успех",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                        );
                    }
                    catch (Exception ex)
                    {
                        // Показываем сообщение об ошибке
                        MessageBox.Show(
                            $"Ошибка при удалении: {ex.Message}",
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                    }
                    finally
                    {
                        // Возвращаемся к исходному ViewModel
                        _mainLayoutNavStore.CurrentVmProp.Value = _studentsVmLazy.Value;
                    }
                },
                () => _mainLayoutNavStore.CurrentVmProp.Value = _studentsVmLazy.Value
            );
        }
    }
}