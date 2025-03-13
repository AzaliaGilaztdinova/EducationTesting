using EducationTesting.Client.Models.Tests;

namespace EducationTesting.Client.Helpers
{
    public static class QuestionsTypesStrings
    {
        
        private const string SingleOption = "Один вариант ответа";
        private const string ManyOptions = "Несколько вариантов ответа";
        private const string Undefined = "Не определено";

        /// <summary>
        /// Получить строковое представление типа вопроса
        /// </summary>
        /// <param name="type">Тип вопроса</param>
        /// <returns>Строковое представление типа</returns>
        public static string GetString(this QuestionType type)
        {
            switch (type)
            {
                case QuestionType.SingleOption:
                    return SingleOption;
                case QuestionType.ManyOptions:
                    return ManyOptions;
                default:
                    return Undefined;
            }
        }
    }
}