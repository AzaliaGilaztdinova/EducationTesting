namespace EducationTesting.Client.Models.Tests
{
    /// <summary>
    /// Вариант ответа
    /// </summary>
    public class AnswerOption
    {
        /// <summary>
        /// Идентификатор варианта ответа
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Идентификатор вопроса
        /// </summary>
        public string QuestionId { get; set; }

        /// <summary>
        /// Текст варианта ответа
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Правильный ли вариант
        /// </summary>
        public bool IsCorrect { get; set; }

        public bool IsSelected { get; set; }
    }
}