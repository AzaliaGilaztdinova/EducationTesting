using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EducationTesting.Client.Models.Subjects;
using EducationTesting.Client.Models.Users;
using EducationTesting.Client.Services;
using EducationTesting.Client.Stores;
using Prism.Commands;
using Prism.Mvvm;

namespace EducationTesting.Client.ViewModels
{
    public class SubjectsVm : BindableBase
    {
        private readonly ISubjectsStore _subjectsStore;
        private readonly ISubjectsService _service;
        private readonly ITestsStore _testsStore;
        private readonly Lazy<ITestsVm> _testsVmLazy;
        private readonly IMainLayoutNavStore _mainLayoutNavStore;
        private readonly IAuthStore _authStore;
        private readonly IGuidProvider _guidProvider;
        private readonly Lazy<SubjectsVm> _subjectsVmLazy;
        private readonly Lazy<DisciplinesVm> _disciplinesVmLazy;
        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (value == _searchText) return;
                _searchText = value;
                RaisePropertyChanged(nameof(Subjects));
            }
        }

        public string DisciplineName => _subjectsStore.SelectedDiscipline.Name;

        public IEnumerable<Subject> Subjects =>
            _service.GetListByDisciplineId(_subjectsStore.SelectedDiscipline.Id).Where(t =>
                string.IsNullOrWhiteSpace(SearchText) || t.Name.ToLower().Contains(SearchText.ToLower()));

        public Roles Role => _authStore.CurrentUser.Role;

        public ICommand AddCommand { get; }
        public ICommand GoToCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand GoToTestsCommand { get; }

        public SubjectsVm(ISubjectsStore subjectsStore, ISubjectsService service, ITestsStore testsStore,
            Lazy<ITestsVm> testsVmLazy,
            Lazy<SubjectsVm> subjectsVmLazy, Lazy<DisciplinesVm> disciplinesVmLazy,
            IMainLayoutNavStore mainLayoutNavStore, IAuthStore authStore, IGuidProvider guidProvider)
        {
            _subjectsStore = subjectsStore;
            _service = service;
            _testsStore = testsStore;
            _testsVmLazy = testsVmLazy;
            _subjectsVmLazy = subjectsVmLazy;
            _disciplinesVmLazy = disciplinesVmLazy;
            _mainLayoutNavStore = mainLayoutNavStore;
            _authStore = authStore;
            _guidProvider = guidProvider;
            AddCommand = new DelegateCommand(Add);
            GoToCommand = new DelegateCommand<string>(GoTo);
            DeleteCommand = new DelegateCommand<Subject>(Delete);
            GoBackCommand = new DelegateCommand(GoBack);
            GoToTestsCommand = new DelegateCommand<Subject>(GoToTests);
        }

        private void Add() => _mainLayoutNavStore.CurrentVmProp.Value =
            new EditSubjectVm(new Subject { DisciplineId = _subjectsStore.SelectedDiscipline.Id }, _service,
                () => _mainLayoutNavStore.CurrentVmProp.Value = _subjectsVmLazy.Value, _guidProvider);

        private void GoTo(string id)
        {
            var item = _service.Get(id);
            _mainLayoutNavStore.CurrentVmProp.Value =
                new EditSubjectVm(item, _service,
                    () => _mainLayoutNavStore.CurrentVmProp.Value = _subjectsVmLazy.Value, _guidProvider);
        }

        private void Delete(Subject item)
        {
            _mainLayoutNavStore.CurrentVmProp.Value = new ConfirmationVm(
                $"Удалить тему '{item.Name}'?",
                () =>
                {
                    try
                    {
                        _service.Delete(item.Id);

                        // Обновляем список тем
                        RaisePropertyChanged(nameof(Subjects));

                        // Уведомление об успешном удалении
                        MessageBox.Show(
                            "Тема успешно удалена!",
                            "Успех",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information
                        );
                    }
                    catch (Exception ex)
                    {
                        // Обработка ошибок
                        MessageBox.Show(
                            $"Ошибка при удалении: {ex.Message}",
                            "Ошибка",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error
                        );
                    }
                    finally
                    {
                        // Возврат к исходному ViewModel
                        _mainLayoutNavStore.CurrentVmProp.Value = _subjectsVmLazy.Value;
                    }
                },
                () => _mainLayoutNavStore.CurrentVmProp.Value = _subjectsVmLazy.Value
            );
        }
        private void GoToTests(Subject item)
        {
            _testsStore.SelectedSubject = item;
            _testsStore.SelectedDiscipline = _subjectsStore.SelectedDiscipline;
            _mainLayoutNavStore.CurrentVmProp.Value = _testsVmLazy.Value;
        }

        private void GoBack() => _mainLayoutNavStore.CurrentVmProp.Value = _disciplinesVmLazy.Value;
    }
}