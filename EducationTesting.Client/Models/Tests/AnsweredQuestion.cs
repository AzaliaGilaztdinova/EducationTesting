using System.Collections.Generic;

namespace EducationTesting.Client.Models.Tests
{
    public class AnsweredQuestion
    {
        public Question Question { get; set; }
        public List<AnswerOption> AnswerOptions { get; set; } = new List<AnswerOption>();
    }
}