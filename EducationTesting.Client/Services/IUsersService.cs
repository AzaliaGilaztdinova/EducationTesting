using EducationTesting.Client.Models.Users;

namespace EducationTesting.Client.Services
{
    public interface IUsersService
    {
        bool Auth(AuthUserCommand command);
        void UpdatePassword(UpdatePasswordCommand command);
        bool ValidateOldPassword(ValidateOldPasswordCommand command);
    }
}