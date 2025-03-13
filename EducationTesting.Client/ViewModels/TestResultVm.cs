using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Dapper;
using EducationTesting.Client.Models.Tests;
using Prism.Commands;

namespace EducationTesting.Client.ViewModels
{
    public class TestResultVm
    {
        public Test Test { get; }
        public TestResult TestResult { get; }
        public ICommand GoBackCommand { get; }

        public List<AnsweredQuestion> AnsweredQuestions { get; } = new List<AnsweredQuestion>();

        public TestResultVm(Test test, TestResult testResult, Action goBack)
        {
            Test = test;
            TestResult = testResult;
            GoBackCommand = new DelegateCommand(goBack);
            ConfigureResult();
        }

        private void ConfigureResult()
        {
            foreach (var question in Test.Questions)
            {
                AnsweredQuestions.Add(new AnsweredQuestion
                {
                    Question = question,
                    AnswerOptions = question.Options.AsList()
                });

                foreach (var option in question.Options)
                {
                    option.IsSelected = TestResult.Answers.Any(a => a.AnswerOptionId == option.Id);
                }
            }
        }
    }
}