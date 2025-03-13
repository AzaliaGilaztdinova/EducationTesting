using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using EducationTesting.Client.Database;
using EducationTesting.Client.Models.Tests;

namespace EducationTesting.Client.Repositories
{
    /// <summary>
    /// Репозиторий для работы с тестами.
    /// </summary>
    public class TestsRepository : ITestsRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        /// <summary>
        /// Подключение к базе данных.
        /// </summary>
        private IDbConnection Connection => _connectionProvider.Connection;

        /// <summary>
        /// Конструктор репозитория.
        /// </summary>
        /// <param name="connectionProvider">Провайдер подключения к базе данных.</param>
        public TestsRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
            InitializeTable(); // Инициализация таблиц при создании репозитория
        }

        /// <summary>
        /// Получение теста по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор теста.</param>
        /// <returns>Данные теста, включая вопросы и варианты ответов.</returns>
        public Test Get(string id)
        {
            using (var connection = Connection)
            {
                // Получаем тест по Id
                var test = connection.QueryFirstOrDefault<Test>(@"
                    SELECT
                        t.Id,
                        t.Name,
                        t.Description,
                        t.SubjectId,
                        s.Name AS SubjectName,
                        t.TeacherId,
                        u.LastName || ' ' || u.FirstName || 
                        CASE 
                            WHEN u.MiddleName IS NOT NULL AND u.MiddleName <> '' THEN ' ' || u.MiddleName 
                            ELSE '' 
                        END AS TeacherFullName,
                        t.CreatedAt,
                        t.Duration
                    FROM Tests t
                        INNER JOIN Subjects s ON t.SubjectId = s.Id
                        INNER JOIN Users u ON t.TeacherId = u.Id
                    WHERE t.Id = @id
                ", new { id });

                // Если тест не найден, возвращаем null
                if (test == null)
                    return null;

                // Получаем вопросы для теста
                test.Questions = connection.Query<Question>(@"
                    SELECT
                        Id,
                        TestId,
                        Text,
                        Type
                    FROM Questions
                    WHERE TestId = @id
                ", new { id });

                // Получаем варианты ответов для каждого вопроса
                foreach (var question in test.Questions)
                {
                    question.Options = connection.Query<AnswerOption>(@"
                        SELECT
                            Id,
                            QuestionId,
                            Text,
                            IsCorrect
                        FROM AnswerOptions
                        WHERE QuestionId = @questionId
                    ", new { questionId = question.Id });
                }

                return test;
            }
        }

        /// <summary>
        /// Создание нового теста.
        /// </summary>
        /// <param name="test">Данные теста для создания.</param>
        public void Create(Test test)
        {
            if (test == null)
                throw new ArgumentNullException(nameof(test));

            using (var connection = Connection)
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Вставляем тест
                        connection.Execute(@"
                            INSERT INTO Tests (Id, Name, Description, SubjectId, TeacherId, CreatedAt, Duration)
                            VALUES (@Id, @Name, @Description, @SubjectId, @TeacherId, @CreatedAt, @Duration);
                        ", test, transaction);

                        // Вставляем вопросы
                        foreach (var question in test.Questions)
                        {
                            connection.Execute(@"
                                    INSERT INTO Questions (Id, TestId, Text, Type, Score)
                                    VALUES (@Id, @TestId, @Text, @Type, @Score);
                                ", question, transaction);

                            // Вставляем варианты ответов
                            foreach (var option in question.Options)
                            {
                                connection.Execute(@"
                                        INSERT INTO AnswerOptions (Id, QuestionId, Text, IsCorrect)
                                        VALUES (@Id, @QuestionId, @Text, @IsCorrect);
                                    ", option, transaction);
                            }
                        }

                        // Фиксируем транзакцию
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        // Откатываем транзакцию в случае ошибки
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Обновление данных теста.
        /// </summary>
        /// <param name="test">Данные теста для обновления.</param>
        public void Update(Test test)
        {
            if (test == null)
                throw new ArgumentNullException(nameof(test));

            using (var connection = Connection)
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Обновляем тест
                        connection.Execute(@"
                            UPDATE Tests
                            SET
                                Name = @Name,
                                Description = @Description,
                                SubjectId = @SubjectId,
                                TeacherId = @TeacherId,
                                CreatedAt = @CreatedAt,
                                Duration = @Duration
                            WHERE Id = @Id;
                        ", test, transaction);

                        // Удаляем старые вопросы
                        connection.Execute(@"
                            DELETE FROM Questions
                            WHERE TestId = @TestId;
                        ", new { TestId = test.Id }, transaction);

                        // Вставляем обновленные вопросы
                        foreach (var question in test.Questions)
                        {
                            connection.Execute(@"
                                    INSERT INTO Questions (Id, TestId, Text, Type, Score)
                                    VALUES (@Id, @TestId, @Text, @Type, @Score);
                                ", question, transaction);

                            // Вставляем обновленные варианты ответов
                            foreach (var option in question.Options)
                            {
                                connection.Execute(@"
                                        INSERT INTO AnswerOptions (Id, QuestionId, Text, IsCorrect)
                                        VALUES (@Id, @QuestionId, @Text, @IsCorrect);
                                    ", option, transaction);
                            }
                        }

                        // Фиксируем транзакцию
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        // Откатываем транзакцию в случае ошибки
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Удаление теста по его идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор теста.</param>
        public void Delete(string id)
        {
            using (var connection = Connection)
            {
                // Удаляем тест
                connection.Execute(@"DELETE FROM Tests WHERE Id = @id;", new { id });
            }
        }

        /// <summary>
        /// Получение списка тестов по идентификатору предмета.
        /// </summary>
        /// <param name="subjectId">Идентификатор предмета.</param>
        /// <returns>Список тестов, связанных с указанным предметом.</returns>
        public IEnumerable<Test> GetListBySubjectId(string subjectId)
        {
            using (var connection = Connection)
            {
                // Получаем тесты по SubjectId
                var tests = connection.Query<Test>(@"
                    SELECT
                        t.Id,
                        t.Name,
                        t.Description,
                        t.SubjectId,
                        s.Name AS SubjectName,
                        t.TeacherId,
                        u.LastName || ' ' || u.FirstName || 
                        CASE 
                            WHEN u.MiddleName IS NOT NULL AND u.MiddleName <> '' THEN ' ' || u.MiddleName 
                            ELSE '' 
                        END AS TeacherFullName,
                        t.CreatedAt,
                        t.Duration
                    FROM Tests t
                        INNER JOIN Subjects s ON t.SubjectId = s.Id
                        INNER JOIN Users u ON t.TeacherId = u.Id
                    WHERE t.SubjectId = @subjectId
                ", new { subjectId }).ToArray();

                // Получаем вопросы и варианты ответов для каждого теста
                foreach (var test in tests)
                {
                    test.Questions = connection.Query<Question>(@"
                        SELECT
                            Id,
                            TestId,
                            Text,
                            Type
                        FROM Questions
                        WHERE TestId = @Id
                    ", new { test.Id });

                    foreach (var question in test.Questions)
                    {
                        question.Options = connection.Query<AnswerOption>(@"
                            SELECT
                                Id,
                                QuestionId,
                                Text,
                                IsCorrect
                            FROM AnswerOptions
                            WHERE QuestionId = @questionId
                        ", new { questionId = question.Id });
                    }
                }

                return tests;
            }
        }

        /// <summary>
        /// Инициализация таблиц в базе данных.
        /// </summary>
        private void InitializeTable()
        {
            using (var connection = Connection)
                connection.Execute(@"
                    CREATE TABLE IF NOT EXISTS Tests
                    (
                        Id          TEXT NOT NULL PRIMARY KEY,
                        Name        TEXT NOT NULL,
                        Description TEXT,
                        SubjectId   TEXT NOT NULL,
                        TeacherId   TEXT NOT NULL,
                        CreatedAt   TEXT NOT NULL,
                        Duration    INTEGER NOT NULL,
                        FOREIGN KEY (SubjectId) REFERENCES Subjects (Id) ON DELETE CASCADE ON UPDATE CASCADE,
                        FOREIGN KEY (TeacherId) REFERENCES Users (Id) ON DELETE CASCADE ON UPDATE CASCADE
                    );
                    CREATE TABLE IF NOT EXISTS Questions
                    (
                        Id     TEXT     NOT NULL PRIMARY KEY,
                        TestId TEXT     NOT NULL,
                        Text   TEXT     NOT NULL,
                        Type   INTEGER  NOT NULL,
                        Score  INTEGER,
                        FOREIGN KEY (TestId) REFERENCES Tests (Id) ON DELETE CASCADE ON UPDATE CASCADE
                    );
                    CREATE TABLE IF NOT EXISTS AnswerOptions
                    (
                        Id         TEXT NOT NULL PRIMARY KEY,
                        QuestionId TEXT NOT NULL,
                        Text       TEXT,
                        IsCorrect  INTEGER,
                        FOREIGN KEY (QuestionId) REFERENCES Questions (Id) ON DELETE CASCADE ON UPDATE CASCADE
                    );
                ");
        }
    }
}