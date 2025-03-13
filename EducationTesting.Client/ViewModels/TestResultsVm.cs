using System;
using System.Collections.Generic;
using System.Windows.Input;
using EducationTesting.Client.Models.Tests;
using EducationTesting.Client.Services;
using EducationTesting.Client.Stores;
using Prism.Commands;

namespace EducationTesting.Client.ViewModels
{
    public class TestResultsVm
    {
        private readonly ITestsStore _store;
        private readonly IAuthStore _authStore;
        private readonly ITestsResultsService _resultsService;
        private readonly IMainLayoutNavStore _mainLayoutNavStore;
        private readonly Lazy<DisciplinesVm> _disciplinesVmLazy;
        private readonly Lazy<SubjectsVm> _subjectsVmLazy;
        private readonly Lazy<TestsVm> _testsVmLazy;
        private readonly Lazy<TestResultsVm> _testResultsVmLazy;

        public IEnumerable<TestResult> Results =>
            _resultsService.GetListByStudentAndTestId(_authStore.CurrentUser.Id, _store.SelectedTest.Id);

        public string DisciplineName => _store.SelectedDiscipline.Name;

        public string SubjectName => _store.SelectedSubject.Name;

        public string TestName => _store.SelectedTest.Name;

        public ICommand GoBackCommand { get; }
        public ICommand GoBackDisciplinesCommand { get; }
        public ICommand GoBackSubjectsCommand { get; }
        public ICommand GoBackTestsCommand { get; }
        public ICommand GoToCommand { get; }

        public TestResultsVm(ITestsStore store, IAuthStore authStore, ITestsResultsService resultsService,
            IMainLayoutNavStore mainLayoutNavStore, Lazy<DisciplinesVm> disciplinesVmLazy,
            Lazy<SubjectsVm> subjectsVmLazy, Lazy<TestsVm> testsVmLazy, Lazy<TestResultsVm> testResultsVmLazy)
        {
            _store = store;
            _authStore = authStore;
            _resultsService = resultsService;
            _mainLayoutNavStore = mainLayoutNavStore;
            _disciplinesVmLazy = disciplinesVmLazy;
            _subjectsVmLazy = subjectsVmLazy;
            _testsVmLazy = testsVmLazy;
            _testResultsVmLazy = testResultsVmLazy;
            GoBackCommand = new DelegateCommand(GoBack);
            GoBackDisciplinesCommand = new DelegateCommand(GoBackDisciplines);
            GoBackSubjectsCommand = new DelegateCommand(GoBackSubjects);
            GoBackTestsCommand = new DelegateCommand(GoBackTests);
            GoToCommand = new DelegateCommand<TestResult>(GoTo);
        }

        private void GoBack() => _mainLayoutNavStore.CurrentVmProp.Value = _testsVmLazy.Value;

        private void GoBackDisciplines() => _mainLayoutNavStore.CurrentVmProp.Value = _disciplinesVmLazy.Value;

        private void GoBackSubjects() => _mainLayoutNavStore.CurrentVmProp.Value = _subjectsVmLazy.Value;

        private void GoBackTests() => _mainLayoutNavStore.CurrentVmProp.Value = _testsVmLazy.Value;

        private void GoTo(TestResult item) =>
            _mainLayoutNavStore.CurrentVmProp.Value = new TestResultVm(_store.SelectedTest, item,
                () => _mainLayoutNavStore.CurrentVmProp.Value = _testResultsVmLazy.Value);
    }
}