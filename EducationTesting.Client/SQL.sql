﻿CREATE TABLE IF NOT EXISTS Users
(
    Id         TEXT    NOT NULL PRIMARY KEY,
    Login      TEXT    NOT NULL UNIQUE,
    Password   TEXT    NOT NULL,
    FirstName  TEXT    NOT NULL,
    LastName   TEXT    NOT NULL,
    MiddleName TEXT,
    Role       INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS Classes
(
    Id   TEXT NOT NULL PRIMARY KEY,
    Name TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Students
(
    UserId  TEXT NOT NULL PRIMARY KEY,
    ClassId TEXT NOT NULL,
    FOREIGN KEY (UserId) REFERENCES Users (Id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (ClassId) REFERENCES Classes (Id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS Disciplines
(
    Id          TEXT NOT NULL PRIMARY KEY,
    Name        TEXT NOT NULL,
    Description TEXT
);

CREATE TABLE IF NOT EXISTS Subjects
(
    Id           TEXT NOT NULL PRIMARY KEY,
    DisciplineId TEXT NOT NULL,
    Name         TEXT NOT NULL,
    Description  TEXT,
    FOREIGN KEY (DisciplineId) REFERENCES Disciplines (Id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS Tests
(
    Id          TEXT    NOT NULL PRIMARY KEY,
    Name        TEXT    NOT NULL,
    Description TEXT,
    SubjectId   TEXT    NOT NULL,
    TeacherId   TEXT    NOT NULL,
    CreatedAt   TEXT    NOT NULL,
    Duration    INTEGER NOT NULL,
    FOREIGN KEY (SubjectId) REFERENCES Subjects (Id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (TeacherId) REFERENCES Users (Id) ON DELETE CASCADE ON UPDATE CASCADE
);

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

CREATE TABLE IF NOT EXISTS Questions
(
    Id     TEXT    NOT NULL PRIMARY KEY,
    TestId TEXT    NOT NULL,
    Text   TEXT    NOT NULL,
    Type   INTEGER NOT NULL,
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

CREATE TABLE IF NOT EXISTS PracticalTasks
(
    Id          TEXT NOT NULL PRIMARY KEY,
    SubjectId   TEXT NOT NULL,
    Name        TEXT NOT NULL,
    Description TEXT,
    FilePath    TEXT NOT NULL,
    Deadline    TEXT,
    FOREIGN KEY (SubjectId) REFERENCES Subjects (Id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS PracticalTaskAnswers
(
    Id             TEXT NOT NULL PRIMARY KEY,
    StudentId      TEXT NOT NULL,
    TaskId         TEXT NOT NULL,
    AnswerFilePath TEXT NOT NULL,
    SubmittedAt    TEXT NOT NULL,
    Grade          REAL NOT NULL,
    FOREIGN KEY (StudentId) REFERENCES Students (UserId) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (TaskId) REFERENCES PracticalTasks (Id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS Lectures
(
    Id          TEXT NOT NULL PRIMARY KEY,
    SubjectId   TEXT NOT NULL,
    Name        TEXT NOT NULL,
    Description TEXT NOT NULL,
    FOREIGN KEY (SubjectId) REFERENCES Subjects (Id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE IF NOT EXISTS MaterialLinks
(
    Id   TEXT NOT NULL PRIMARY KEY,
    Link TEXT NOT NULL,
    Name TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS LecturesMaterialLinks
(
    LectureId TEXT NOT NULL,
    LinkId    TEXT NOT NULL,
    FOREIGN KEY (LectureId) REFERENCES Lectures (Id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (LinkId) REFERENCES MaterialLinks (Id) ON DELETE CASCADE ON UPDATE CASCADE
);