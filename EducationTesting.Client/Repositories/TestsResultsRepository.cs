using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using EducationTesting.Client.Database;
using EducationTesting.Client.Models.Tests;

namespace EducationTesting.Client.Repositories
{
    public class TestsResultsRepository : ITestsResultsRepository
    {
        private readonly IDbConnectionProvider _connectionProvider;

        private IDbConnection Connection => _connectionProvider.Connection;

        public TestsResultsRepository(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
            InitializeDbTable();
        }

        public IEnumerable<TestResult> GetListByStudentAndTestId(string studentId, string testId)
        {
            using (var connection = Connection)
            {
                var results = connection.Query<TestResult>(@"
                    SELECT
                        tr.Id,
                        tr.TestId,
                        t.Name AS TestName,
                        t.Description AS TestDescription,
                        t.CreatedAt AS TestCreatedAt,
                        t.Duration AS TestDuration,
                        ut.Id AS TeacherId,
                        ut.LastName || ' ' || ut.FirstName || 
                        CASE 
                            WHEN ut.MiddleName IS NOT NULL AND ut.MiddleName <> '' THEN ' ' || ut.MiddleName 
                            ELSE '' 
                        END AS TeacherFullName,
                        tr.StudentId,
                        us.LastName || ' ' || us.FirstName || 
                        CASE
                            WHEN us.MiddleName IS NOT NULL AND us.MiddleName <> '' THEN ' ' || us.MiddleName 
                            ELSE '' 
                        END AS StudentFullName,
                        tr.StartDateTime,
                        tr.EndDateTime,
                        tr.MaxScore
                    FROM
                        TestResults tr
                    INNER JOIN Tests t ON t.Id = tr.TestId
                    INNER JOIN Users ut ON ut.Id = t.TeacherId
                    INNER JOIN Users us ON us.Id = tr.StudentId
                    WHERE
                        tr.TestId = @TestId AND tr.StudentId = @StudentId
                    ORDER BY tr.EndDateTime DESC
                ", new { TestId = testId, StudentId = studentId }).ToArray();

                foreach (var result in results)
                {
                    result.Answers = connection.Query<StudentAnswer>(@"
                    SELECT
                        sa.Id,
                        sa.TestResultId,
                        sa.AnswerOptionId,
                        sa.AnswerText,
                        sa.QuestionId,
                        q.Text AS QuestionText,
                        sa.IsCorrect
                    FROM
                        StudentAnswers sa
                    INNER JOIN Questions q ON q.Id = sa.QuestionId
                    WHERE
                        sa.TestResultId = @Id", new { result.Id }).ToArray();
                }

                return results;
            }
        }

        public TestResult Get(string id)
        {
            using (var connection = Connection)
            {
                var result = connection.QuerySingle<TestResult>(@"
                    SELECT
                        tr.Id,
                        tr.TestId,
                        t.Name AS TestName,
                        t.Description AS TestDescription,
                        t.CreatedAt AS TestCreatedAt,
                        t.Duration AS TestDuration,
                        ut.Id AS TeacherId,
                        ut.LastName || ' ' || ut.FirstName || 
                        CASE 
                            WHEN ut.MiddleName IS NOT NULL AND ut.MiddleName <> '' THEN ' ' || ut.MiddleName 
                            ELSE '' 
                        END AS TeacherFullName,
                        tr.StudentId,
                        us.LastName || ' ' || us.FirstName || 
                        CASE
                            WHEN us.MiddleName IS NOT NULL AND us.MiddleName <> '' THEN ' ' || us.MiddleName 
                            ELSE '' 
                        END AS StudentFullName,
                        tr.StartDateTime,
                        tr.EndDateTime,
                        tr.MaxScore
                    FROM
                        TestResults tr
                    INNER JOIN Tests t ON t.Id = tr.TestId
                    INNER JOIN Users ut ON ut.Id = t.TeacherId
                    INNER JOIN Users us ON us.Id = tr.StudentId
                    WHERE
                        tr.Id = @Id
                ", new { Id = id });
                result.Answers = connection.Query<StudentAnswer>(@"
                    SELECT
                        sa.Id,
                        sa.TestResultId,
                        sa.AnswerOptionId,
                        sa.AnswerText,
                        sa.QuestionId,
                        q.Text AS QuestionText,
                        sa.IsCorrect
                    FROM
                        StudentAnswers sa
                    INNER JOIN Questions q ON q.Id = sa.QuestionId
                    WHERE
                        sa.TestResultId = @Id", new { Id = id });
                return result;
            }
        }

        public void Create(TestResult item)
        {
            using (var connection = Connection)
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    connection.Execute(@"
                        INSERT INTO TestResults (Id, TestId, StudentId, StartDateTime, EndDateTime, MaxScore, AchievedScore)
                        VALUES (@Id, @TestId, @StudentId, @StartDateTime, @EndDateTime, @MaxScore, @AchievedScore);
                    ", item, transaction);

                    foreach (var answer in item.Answers)
                    {
                        connection.Execute(@"
                            INSERT INTO StudentAnswers (Id, TestResultId, QuestionId, AnswerOptionId, AnswerText, IsCorrect)
                            VALUES (@Id, @TestResultId, @QuestionId, @AnswerOptionId, @AnswerText, @IsCorrect);
                        ", answer, transaction);
                    }

                    transaction.Commit();
                }
            }
        }

        public void Update(TestResult item)
        {
            using (var connection = Connection)
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    // Обновляем запись в таблице TestResults
                    connection.Execute(@"
                        UPDATE TestResults
                        SET
                            TestId = @TestId,
                            StudentId = @StudentId,
                            StartDateTime = @StartDateTime,
                            EndDateTime = @EndDateTime,
                            MaxScore = @MaxScore,
                            AchievedScore = @AchievedScore
                        WHERE
                            Id = @Id;
                    ", item, transaction);

                    // Удаляем старые записи в таблице StudentAnswers
                    connection.Execute(@"
                        DELETE FROM StudentAnswers
                        WHERE TestResultId = @Id;
                    ", new { item.Id }, transaction);

                    // Вставляем новые записи в таблицу StudentAnswers
                    foreach (var answer in item.Answers)
                    {
                        connection.Execute(@"
                            INSERT INTO StudentAnswers (Id, TestResultId, QuestionId, AnswerOptionId, AnswerText, IsCorrect)
                            VALUES (@Id, @TestResultId, @QuestionId, @AnswerOptionId, @AnswerText, @IsCorrect);
                        ", answer, transaction);
                    }

                    transaction.Commit();
                }
            }
        }

        public void Delete(string id)
        {
            using (var connection = Connection)
                connection.Execute(@"DELETE FROM TestResults WHERE Id = @id;", new { id });
        }

        private void InitializeDbTable()
        {
            using (var connection = Connection)
                connection.Execute(@"
                    CREATE TABLE IF NOT EXISTS TestResults
                    (
                        Id            TEXT NOT NULL PRIMARY KEY,
                        TestId        TEXT NOT NULL,
                        StudentId     TEXT NOT NULL,
                        StartDateTime TEXT NOT NULL,
                        EndDateTime   TEXT,
                        MaxScore      INTEGER,
                        AchievedScore INTEGER,
                        FOREIGN KEY (TestId) REFERENCES Tests (Id) ON DELETE CASCADE ON UPDATE CASCADE,
                        FOREIGN KEY (StudentId) REFERENCES Users (Id) ON DELETE CASCADE ON UPDATE CASCADE
                    );
                    CREATE TABLE IF NOT EXISTS StudentAnswers
                    (
                        Id             TEXT NOT NULL PRIMARY KEY,
                        TestResultId   TEXT NOT NULL,
                        QuestionId     TEXT NOT NULL,
                        AnswerOptionId TEXT NOT NULL,
                        AnswerText     TEXT,
                        IsCorrect      INTEGER,
                        FOREIGN KEY (TestResultId) REFERENCES TestResults (Id) ON DELETE CASCADE ON UPDATE CASCADE,
                        FOREIGN KEY (QuestionId) REFERENCES Questions (Id) ON DELETE CASCADE ON UPDATE CASCADE,
                        FOREIGN KEY (AnswerOptionId) REFERENCES AnswerOptions (Id) ON DELETE CASCADE ON UPDATE CASCADE
                    );
                ");
        }
    }
}