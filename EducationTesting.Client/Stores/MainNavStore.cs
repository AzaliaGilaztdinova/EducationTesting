using EducationTesting.Client.Helpers;

namespace EducationTesting.Client.Stores
{
    public class MainNavStore : IMainNavStore
    {
        public Property<object> CurrentVmProp { get; } = new Property<object>();
    }
}