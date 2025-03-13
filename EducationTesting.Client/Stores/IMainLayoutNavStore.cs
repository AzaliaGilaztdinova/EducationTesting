using EducationTesting.Client.Helpers;

namespace EducationTesting.Client.Stores
{
    public interface IMainLayoutNavStore
    {
        Property<object> CurrentVmProp { get; }
    }
}