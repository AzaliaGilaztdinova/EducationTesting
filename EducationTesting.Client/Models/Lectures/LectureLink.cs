namespace EducationTesting.Client.Models.Lectures
{
    /// <summary>
    /// Ссылка на материал лекции
    /// </summary>
    public class LectureLink
    {
        /// <summary>
        /// Идентификатор ссылки
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Идентификатор лекции
        /// </summary>
        public string LectureId { get; set; }
        
        /// <summary>
        /// Ссылка
        /// </summary>
        public string Link { get; set; }
        
        /// <summary>
        /// Название ссылки
        /// </summary>
        public string Name { get; set; }
    }
}