using System;
using System.Collections.Generic;
using System.Linq;

namespace EducationTesting.Client.Models.Tests
{
    /// <summary>
    /// Результат выполнения теста
    /// </summary>
    public class TestResult
    {
        /// <summary>
        /// Идентификатор результата
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Идентификатор теста
        /// </summary>
        public string TestId { get; set; }

        /// <summary>
        /// Название теста
        /// </summary>
        public string TestName { get; set; }

        public int TestDuration { get; set; }

        /// <summary>
        /// Описание теста
        /// </summary>
        public string TestDescription { get; set; }

        /// <summary>
        /// Дата и время создания теста
        /// </summary>
        public DateTime TestCreatedAt { get; set; }

        /// <summary>
        /// Идентификатор учителя
        /// </summary>
        public string TeacherId { get; set; }

        /// <summary>
        /// ФИО учителя
        /// </summary>
        public string TeacherFullName { get; set; }

        /// <summary>
        /// Идентификатор ученика
        /// </summary>
        public string StudentId { get; set; }

        /// <summary>
        /// ФИО ученика
        /// </summary>
        public string StudentFullName { get; set; }

        /// <summary>
        /// Дата и время начала
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// Дата и время окончания
        /// </summary>
        public DateTime EndDateTime { get; set; }

        public TimeSpan Duration => EndDateTime - StartDateTime;

        /// <summary>
        /// Максимальное количество баллов
        /// </summary>
        public int MaxScore { get; set; }

        /// <summary>
        /// Количество набранных баллов
        /// </summary>
        public int AchievedScore
        {
            get
            {
                var result = Answers.Count(a => a.IsCorrect) - Answers.Count(a => a.IsCorrect is false);
                return result < 0 ? 0 : result;
            }
        }

        public bool InTime => Duration.Minutes < TestDuration;

        public int Percentage => (int)Math.Round(AchievedScore * 1.0 / MaxScore * 100);
        public int Grade => Percentage > 90 ? 5 : Percentage > 65 ? 4 : Percentage > 49 ? 3 : Percentage > 1 ? 2 : 1;

        public IEnumerable<StudentAnswer> Answers { get; set; } = new List<StudentAnswer>();
    }
}