using EducationTesting.Client.Stores;

namespace EducationTesting.Client.ViewModels
{
    public class MainVm
    {
        public IMainNavStore NavStore { get; }

        public MainVm(IMainNavStore mainNavStore, LoginVm loginVm)
        {
            NavStore = mainNavStore;
            mainNavStore.CurrentVmProp.Value = loginVm;
        }
    }
}