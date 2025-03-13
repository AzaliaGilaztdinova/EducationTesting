namespace EducationTesting.Client.Models.Lectures
{
    /// <summary>
    /// Лекция
    /// </summary>
    public class Lecture
    {
        /// <summary>
        /// Идентификатор лекции
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Название лекции
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Описание лекции
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Идентификатор темы предмета
        /// </summary>
        public string SubjectId { get; set; }
        
        /// <summary>
        /// Название темы предмета
        /// </summary>
        public string SubjectName { get; set; }
        
        /// <summary>
        /// Идентификатор предмета
        /// </summary>
        public string DisciplineId { get; set; }
        
        /// <summary>
        /// Название предмета
        /// </summary>
        public string DisciplineName { get; set; }
    }
}