using EducationTesting.Client.Models.Users;

namespace EducationTesting.Client.Models.Students
{
    /// <summary>
    /// Ученик
    /// </summary>
    public class Student : User
    {
        /// <summary>
        /// Идентификатор класса
        /// </summary>
        public string ClassId { get; set; }
        
        /// <summary>
        /// Название класса
        /// </summary>
        public string Class { get; set; }
    }
}