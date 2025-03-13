namespace EducationTesting.Client.Models.Users
{
    public class UpdatePasswordCommand
    {
        public string UserId { get; set; }
        public string NewPassword { get; set; }
    }
}