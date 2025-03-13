using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EducationTesting.Client.Models.Disciplines;
using EducationTesting.Client.Models.Users;
using EducationTesting.Client.Services;
using EducationTesting.Client.Stores;
using Prism.Commands;
using Prism.Mvvm;

namespace EducationTesting.Client.ViewModels
{
    public class DisciplinesVm : BindableBase
    {
        private readonly IDisciplinesService _service;
        private readonly IMainLayoutNavStore _mainLayoutNavStore;
        private readonly Lazy<DisciplinesVm> _disciplinesVmLazy;
        private readonly ISubjectsStore _subjectsStore;
        private readonly Lazy<SubjectsVm> _subjectsVmLazy;
        private readonly IAuthStore _authStore;
        private readonly IGuidProvider _guidProvider;
        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (value == _searchText) return;
                _searchText = value;
                RaisePropertyChanged(nameof(Disciplines));
            }
        }

        public IEnumerable<Discipline> Disciplines =>
            _service.GetList().Where(t =>
                string.IsNullOrWhiteSpace(SearchText) || t.Name.ToLower().Contains(SearchText.ToLower()));

        public ICommand AddCommand { get; }
        public ICommand GoToCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand GoToSubjectsCommand { get; }

        public Roles Role => _authStore.CurrentUser.Role;

        public DisciplinesVm(IDisciplinesService service,
            IMainLayoutNavStore mainLayoutNavStore,
            Lazy<DisciplinesVm> disciplinesVmLazy, ISubjectsStore subjectsStore, Lazy<SubjectsVm> subjectsVmLazy,
            IAuthStore authStore, IGuidProvider guidProvider)
        {
            _service = service;
            _mainLayoutNavStore = mainLayoutNavStore;
            _disciplinesVmLazy = disciplinesVmLazy;
            _subjectsStore = subjectsStore;
            _subjectsVmLazy = subjectsVmLazy;
            _authStore = authStore;
            _guidProvider = guidProvider;
            AddCommand = new DelegateCommand(Add);
            GoToCommand = new DelegateCommand<string>(GoTo);
            DeleteCommand = new DelegateCommand<Discipline>(Delete);
            GoToSubjectsCommand = new DelegateCommand<Discipline>(GoToSubjects);
        }

        private void Add() => _mainLayoutNavStore.CurrentVmProp.Value =
            new EditDisciplineVm(new Discipline(), _service,
                () => _mainLayoutNavStore.CurrentVmProp.Value = _disciplinesVmLazy.Value, _guidProvider);

        private void GoTo(string id)
        {
            var item = _service.Get(id);
            _mainLayoutNavStore.CurrentVmProp.Value =
                new EditDisciplineVm(item, _service,
                    () => _mainLayoutNavStore.CurrentVmProp.Value = _disciplinesVmLazy.Value, _guidProvider);
        }

        private void Delete(Discipline item)
        {
            _mainLayoutNavStore.CurrentVmProp.Value = new ConfirmationVm(
                $"Удалить предмет '{item.Name}'?",
                () =>
                {
                    try
                    {
                        _service.Delete(item.Id);

                        // Обновляем список дисциплин
                        RaisePropertyChanged(nameof(Disciplines));

                        // Показываем сообщение об успехе
                        MessageBox.Show(
                            "Предмет успешно удален!",
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
                        _mainLayoutNavStore.CurrentVmProp.Value = _disciplinesVmLazy.Value;
                    }
                },
                () => _mainLayoutNavStore.CurrentVmProp.Value = _disciplinesVmLazy.Value
            );
        }
        private void GoToSubjects(Discipline item)
        {
            _subjectsStore.SelectedDiscipline = item;
            _mainLayoutNavStore.CurrentVmProp.Value = _subjectsVmLazy.Value;
        }
    }
}