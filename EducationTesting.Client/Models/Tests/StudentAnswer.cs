namespace EducationTesting.Client.Models.Tests
{
    /// <summary>
    /// Ответ ученика
    /// </summary>
    public class StudentAnswer
    {
        /// <summary>
        /// Идентификатор ответа
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Идентификатор результата теста
        /// </summary>
        public string TestResultId { get; set; }
        
        /// <summary>
        /// Идентификатор варианта ответа
        /// </summary>
        public string AnswerOptionId { get; set; }
        
        /// <summary>
        /// Текст ответа
        /// </summary>
        public string AnswerText { get; set; }
        
        /// <summary>
        /// Идентификатор вопроса
        /// </summary>
        public string QuestionId { get; set; }
        
        /// <summary>
        /// Текст вопроса
        /// </summary>
        public string QuestionText { get; set; }
        
        /// <summary>
        /// Правильный ли ответ
        /// </summary>
        public bool IsCorrect { get; set; }
    }
}