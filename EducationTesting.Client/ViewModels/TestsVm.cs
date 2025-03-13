using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EducationTesting.Client.Models.Tests;
using EducationTesting.Client.Models.Users;
using EducationTesting.Client.Services;
using EducationTesting.Client.Stores;
using Prism.Commands;
using Prism.Mvvm;

namespace EducationTesting.Client.ViewModels
{
    public class TestsVm : BindableBase
    {
        private readonly ITestsStore _store;
        private readonly ITestsService _service;
        private readonly IMainLayoutNavStore _mainLayoutNavStore;
        private readonly Lazy<SubjectsVm> _subjectsVmLazy;
        private readonly Lazy<TestsVm> _testsVmLazy;
        private readonly IAuthStore _authStore;
        private readonly Lazy<TestPerformVm> _testPerformLazy;
        private readonly Lazy<DisciplinesVm> _disciplinesVmLazy;
        private readonly Lazy<TestResultsVm> _testResultsVmLazy;
        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (value == _searchText) return;
                _searchText = value;
                RaisePropertyChanged(nameof(Tests));
            }
        }

        public string DisciplineName => _store.SelectedDiscipline.Name;
        public string SubjectName => _store.SelectedSubject.Name;

        public IEnumerable<Test> Tests =>
            _service.GetListBySubjectId(_store.SelectedSubject.Id).Where(t =>
                string.IsNullOrWhiteSpace(SearchText) || t.Name.ToLower().Contains(SearchText.ToLower()));

        public Roles Role => _authStore.CurrentUser.Role;

        public ICommand AddCommand { get; }
        public ICommand GoToCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand GoBackDisciplinesCommand { get; }
        public ICommand GoToResultsCommand { get; }
        public ICommand GoToPerformCommand { get; }

        public TestsVm(ITestsStore store, ITestsService service, IMainLayoutNavStore mainLayoutNavStore,
            Lazy<SubjectsVm> subjectsVmLazy, Lazy<TestsVm> testsVmLazy, IAuthStore authStore,
            Lazy<TestPerformVm> testPerformLazy, Lazy<DisciplinesVm> disciplinesVmLazy,
            Lazy<TestResultsVm> testResultsVmLazy)
        {
            _store = store;
            _service = service;
            _mainLayoutNavStore = mainLayoutNavStore;
            _subjectsVmLazy = subjectsVmLazy;
            _testsVmLazy = testsVmLazy;
            _authStore = authStore;
            _testPerformLazy = testPerformLazy;
            _disciplinesVmLazy = disciplinesVmLazy;
            _testResultsVmLazy = testResultsVmLazy;
            GoBackCommand = new DelegateCommand(GoBack);
            GoBackDisciplinesCommand = new DelegateCommand(GoBackDisciplines);
            GoToResultsCommand = new DelegateCommand<Test>(GoToResults);
            AddCommand = new DelegateCommand(Add);
            GoToCommand = new DelegateCommand<string>(GoTo);
            DeleteCommand = new DelegateCommand<Test>(Delete);
            GoToPerformCommand = new DelegateCommand<Test>(GoToPerform);
        }

        private void Add() => _mainLayoutNavStore.CurrentVmProp.Value = new EditTestVm(new Test(), _service, _store,
            () => _mainLayoutNavStore.CurrentVmProp.Value = _testsVmLazy.Value, _authStore);

        private void GoTo(string id) => _mainLayoutNavStore.CurrentVmProp.Value = new EditTestVm(_service.Get(id),
            _service, _store, () => _mainLayoutNavStore.CurrentVmProp.Value = _testsVmLazy.Value, _authStore);

        private void GoToPerform(Test item)
        {
            _store.SelectedTest = item;
            _mainLayoutNavStore.CurrentVmProp.Value = _testPerformLazy.Value;
        }

        private void Delete(Test item)
        {
            _mainLayoutNavStore.CurrentVmProp.Value = new ConfirmationVm(
                $"Вы действительно хотите удалить тест \"{item.Name}\"?",
                () =>
                {
                    try
                    {
                        // Выполняем удаление
                        _service.Delete(item.Id);

                        // Обновляем список тестов
                        RaisePropertyChanged(nameof(Tests));

                        // Показываем уведомление
                        MessageBox.Show("Тест успешно удалён!",
                                      "Успех",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        // Обработка ошибок
                        MessageBox.Show($"Ошибка при удалении: {ex.Message}",
                                      "Ошибка",
                                      MessageBoxButton.OK,
                                      MessageBoxImage.Error);
                    }
                    finally
                    {
                        // Возвращаемся обратно
                        _mainLayoutNavStore.CurrentVmProp.Value = _testsVmLazy.Value;
                    }
                },
                // Действие при отмене
                () => _mainLayoutNavStore.CurrentVmProp.Value = _testsVmLazy.Value
            );
        }
        private void GoBack() => _mainLayoutNavStore.CurrentVmProp.Value = _subjectsVmLazy.Value;

        private void GoBackDisciplines() => _mainLayoutNavStore.CurrentVmProp.Value = _disciplinesVmLazy.Value;

        public void GoToResults(Test item)
        {
            _store.SelectedTest = item;
            _mainLayoutNavStore.CurrentVmProp.Value = _testResultsVmLazy.Value;
        }
    }
}