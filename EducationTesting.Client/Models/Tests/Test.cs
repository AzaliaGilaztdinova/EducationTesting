using System;
using System.Collections.Generic;

namespace EducationTesting.Client.Models.Tests
{
    /// <summary>
    /// Тест
    /// </summary>
    public class Test
    {
        /// <summary>
        /// Идентификатор теста
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Название теста
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Описание теста
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Идентификатор темы предмета
        /// </summary>
        public string SubjectId { get; set; }
        
        /// <summary>
        /// Название темы предмета
        /// </summary>
        public string Subject { get; set; }
        
        /// <summary>
        /// Идентификатор учителя
        /// </summary>
        public string TeacherId { get; set; }
        
        /// <summary>
        /// ФИО учителя
        /// </summary>
        public string TeacherFullName { get; set; }
        
        /// <summary>
        /// Дата создания теста
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Продолжительность теста
        /// </summary>
        public int Duration { get; set; }

        public IEnumerable<Question> Questions { get; set; } = new List<Question>();
    }
}