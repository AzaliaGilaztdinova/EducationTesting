using EducationTesting.Client.Helpers;

namespace EducationTesting.Client.Stores
{
    public interface IMainNavStore
    {
        Property<object> CurrentVmProp { get; }
    }
}