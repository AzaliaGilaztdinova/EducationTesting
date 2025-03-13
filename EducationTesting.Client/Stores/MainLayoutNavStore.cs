using EducationTesting.Client.Helpers;

namespace EducationTesting.Client.Stores
{
    public class MainLayoutNavStore : IMainLayoutNavStore
    {
        public IProperty<object> CurrentVmProp { get; } = new Property<object>();
    }
}