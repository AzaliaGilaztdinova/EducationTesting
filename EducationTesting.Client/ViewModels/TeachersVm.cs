using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EducationTesting.Client.Models.Teachers;
using EducationTesting.Client.Services;
using EducationTesting.Client.Stores;
using Prism.Commands;
using Prism.Mvvm;

namespace EducationTesting.Client.ViewModels
{
    public class TeachersVm : BindableBase
    {
        private readonly ITeachersService _service;
        private readonly IMainLayoutNavStore _mainLayoutNavStore;
        private readonly Lazy<TeachersVm> _teachersVmLazy;
        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (value == _searchText) return;
                _searchText = value;
                RaisePropertyChanged(nameof(Teachers));
            }
        }

        public IEnumerable<Teacher> Teachers =>
            _service.GetList().Where(t =>
                string.IsNullOrWhiteSpace(SearchText) || t.LastName.ToLower().Contains(SearchText.ToLower()) ||
                t.FirstName.ToLower().Contains(SearchText.ToLower()) ||
                t.MiddleName.ToLower().Contains(SearchText.ToLower()));

        public ICommand AddCommand { get; }
        public ICommand GoToCommand { get; }
        public ICommand DeleteCommand { get; }

        public TeachersVm(ITeachersService service, IMainLayoutNavStore mainLayoutNavStore,
            Lazy<TeachersVm> teachersVmLazy)
        {
            _service = service;
            _mainLayoutNavStore = mainLayoutNavStore;
            _teachersVmLazy = teachersVmLazy;
            AddCommand = new DelegateCommand(Add);
            GoToCommand = new DelegateCommand<string>(GoTo);
            DeleteCommand = new DelegateCommand<Teacher>(Delete);
        }

        private void Add() => _mainLayoutNavStore.CurrentVmProp.Value =
            new EditTeacherVm(new Teacher(), _service,
                () => _mainLayoutNavStore.CurrentVmProp.Value = _teachersVmLazy.Value);

        private void GoTo(string id)
        {
            var item = _service.Get(id);
            _mainLayoutNavStore.CurrentVmProp.Value =
                new EditTeacherVm(item, _service,
                    () => _mainLayoutNavStore.CurrentVmProp.Value = _teachersVmLazy.Value);
        }

        private void Delete(Teacher item)
        {
            _mainLayoutNavStore.CurrentVmProp.Value = new ConfirmationVm(
                $"Удалить учителя '{item.LastName} {item.FirstName}'?",
                () =>
                {
                    try
                    {
                        _service.Delete(item.Id);

                        // Обновляем список учителей
                        RaisePropertyChanged(nameof(Teachers));

                        // Показываем сообщение об успехе
                        MessageBox.Show(
                            "Учитель успешно удален!",
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
                        _mainLayoutNavStore.CurrentVmProp.Value = _teachersVmLazy.Value;
                    }
                },
                () => _mainLayoutNavStore.CurrentVmProp.Value = _teachersVmLazy.Value
            );
        }
    }
}