using System;
using System.Windows.Input;
using EducationTesting.Client.Models.Users;
using EducationTesting.Client.Stores;
using Prism.Commands;

namespace EducationTesting.Client.ViewModels
{
    public class MainLayoutVm
    {
        private readonly ProfileVm _profileVm;
        private readonly Lazy<LoginVm> _loginVmLazy;
        private readonly Lazy<ClassesVm> _classesVmLazy;
        private readonly Lazy<StudentsVm> _studentsVmLazy;
        private readonly Lazy<DisciplinesVm> _disciplinesVmLazy;
        private readonly Lazy<TeachersVm> _teachersVmLazy;
        private readonly IAuthStore _authStore;
        private readonly IMainNavStore _mainNavStore;

        public IMainLayoutNavStore LayoutNavStore { get; }
        public ICommand GoToCommand { get; }
        public ICommand LogoutCommand { get; }
        public Roles Role => _authStore.CurrentUser.Role;

        public MainLayoutVm(IMainLayoutNavStore mainLayoutNavStore, ProfileVm profileVm, Lazy<LoginVm> loginVmLazy,
            Lazy<ClassesVm> classesVmLazy, Lazy<StudentsVm> studentsVmLazy,
            Lazy<DisciplinesVm> disciplinesVmLazy, Lazy<TeachersVm> teachersVmLazy,
            IAuthStore authStore, IMainNavStore mainNavStore)
        {
            LayoutNavStore = mainLayoutNavStore;
            _profileVm = profileVm;
            _loginVmLazy = loginVmLazy;
            _classesVmLazy = classesVmLazy;
            _studentsVmLazy = studentsVmLazy;
            _disciplinesVmLazy = disciplinesVmLazy;
            _teachersVmLazy = teachersVmLazy;
            _authStore = authStore;
            _mainNavStore = mainNavStore;
            mainLayoutNavStore.CurrentVmProp.Value = profileVm;
            GoToCommand = new DelegateCommand<object>(GoTo);
            LogoutCommand = new DelegateCommand(Logout);
        }

        private void GoTo(object page)
        {
            switch (page)
            {
                case Pages.Profile:
                    LayoutNavStore.CurrentVmProp.Value = _profileVm;
                    break;
                case Pages.Students:
                    LayoutNavStore.CurrentVmProp.Value = _studentsVmLazy.Value;
                    break;
                case Pages.Disciplines:
                    LayoutNavStore.CurrentVmProp.Value = _disciplinesVmLazy.Value;
                    break;
                case Pages.Classes:
                    LayoutNavStore.CurrentVmProp.Value = _classesVmLazy.Value;
                    break;
                case Pages.Teachers:
                    LayoutNavStore.CurrentVmProp.Value = _teachersVmLazy.Value;
                    break;
                default:
                    return;
            }
        }

        private void Logout()
        {
            var temp = LayoutNavStore.CurrentVmProp.Value;
            LayoutNavStore.CurrentVmProp.Value = new ConfirmationVm("Вы действительно хотите выйти?", () =>
            {
                _authStore.CurrentUser = null;
                _mainNavStore.CurrentVmProp.Value = _loginVmLazy.Value;
            }, () =>
            {
                if (_authStore.CurrentUser is null)
                {
                    return;
                }

                LayoutNavStore.CurrentVmProp.Value = temp;
            });
        }
    }
}