using EducationTesting.Client.Models.Users;

namespace EducationTesting.Client.Stores
{
    public interface IAuthStore
    {
        User CurrentUser { get; set; }
    }
}