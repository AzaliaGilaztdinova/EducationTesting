using EducationTesting.Client.Models.Users;

namespace EducationTesting.Client.Stores
{
    public class AuthStore : IAuthStore
    {
        public User CurrentUser { get; set; }
    }
}