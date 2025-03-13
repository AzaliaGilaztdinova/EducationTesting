using EducationTesting.Client.Helpers;

namespace EducationTesting.Client.Stores
{
    public class MainLayoutNavStore : IMainLayoutNavStore
    {
        public Property<object> CurrentVmProp { get; } = new Property<object>();
    }
}