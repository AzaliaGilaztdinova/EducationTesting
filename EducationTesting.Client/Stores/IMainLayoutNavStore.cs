using EducationTesting.Client.Helpers;

namespace EducationTesting.Client.Stores
{
    public interface IMainLayoutNavStore
    {
        IProperty<object> CurrentVmProp { get; }
    }
}