using System;

namespace EducationTesting.Client.Models.PracticalTasks
{
    public class PracticalTask
    {
        public string Id { get; set; }
        public string SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FilePath { get; set; }
        public DateTime Deadline { get; set; }
    }
}