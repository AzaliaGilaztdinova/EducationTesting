using System;

namespace EducationTesting.Client.Models.PracticalTasks
{
    public class PracticalTaskAnswer
    {
        public string Id { get; set; }
        public string StudentId { get; set; }
        public string StudentFullName { get; set; }
        public string PracticalTaskId { get; set; }
        public string PracticalTaskName { get; set; }
        public DateTime SubmittedAt { get; set; }
        public string AnswerFilePath { get; set; }
        public float Grade { get; set; }
    }
}