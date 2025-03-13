namespace EducationTesting.Client.Models.Users
{
    public class ValidateOldPasswordCommand
    {
        public string UserId { get; set; }
        public string OldPassword { get; set; }
    }
}