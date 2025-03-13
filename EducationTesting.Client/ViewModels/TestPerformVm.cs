﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EducationTesting.Client.Helpers;
using EducationTesting.Client.Models.Tests;
using EducationTesting.Client.Services;
using EducationTesting.Client.Stores;
using Prism.Commands;

namespace EducationTesting.Client.ViewModels
{
    public class TestPerformVm
    {
        private readonly ITestsResultsService _resultsService;
        private readonly IMainLayoutNavStore _navStore;
        private readonly Lazy<TestsVm> _testsVmLazy;
        private readonly TestResult _result;

        public Test Item { get; }
        public Property<string> ErrorMessageProp { get; } = new Property<string>();
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public TestPerformVm(ITestsStore store, ITestsResultsService resultsService,
            IAuthStore authStore, IMainLayoutNavStore navStore, Lazy<TestsVm> testsVmLazy)
        {
            _resultsService = resultsService;
            _navStore = navStore;
            _testsVmLazy = testsVmLazy;
            Item = store.SelectedTest;
            _result = new TestResult
            {
                Id = Guid.NewGuid().ToString(),
                TestId = Item.Id,
                StudentId = authStore.CurrentUser.Id,
                MaxScore = Item.Questions.Sum(q => q.Score),
                StartDateTime = DateTime.Now,
                TeacherId = Item.TeacherId
            };
            SaveCommand = new DelegateCommand(Save);
            CancelCommand = new DelegateCommand(GoBack);
        }

        private void Save()
        {
            // Проверка валидации
            if (Validate() is false)
            {
                return;
            }
            // Запрос подтверждения завершения теста
            var confirmation = MessageBox.Show(
                "Вы уверены, что хотите завершить тест? Это действие нельзя отменить.",
                "Подтверждение завершения",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            // Если пользователь отменил завершение теста
            if (confirmation == MessageBoxResult.No)
            {
                return;
            }
            // Обработка выбранных опций
            foreach (var question in Item.Questions)
            {
                foreach (var option in question.Options)
                {
                    if (option.IsSelected)
                    {
                        AddOption(option);
                    }
                }
            }
            // Установка времени завершения теста
            _result.EndDateTime = DateTime.Now;

            // Создание результата теста
            _resultsService.Create(_result);

            // Переход к следующему экрану
            _navStore.CurrentVmProp.Value = _testsVmLazy.Value;

            // Сообщение об успешном завершении
            MessageBox.Show(
                "Тест успешно завершен!",
                "Успех",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void GoBack() => _navStore.CurrentVmProp.Value = _testsVmLazy.Value;

        private void AddOption(AnswerOption item)
        {
            _result.Answers = _result.Answers.Append(new StudentAnswer
            {
                Id = Guid.NewGuid().ToString(),
                AnswerOptionId = item.Id,
                QuestionId = item.QuestionId,
                TestResultId = _result.Id,
                AnswerText = item.Text,
                IsCorrect = item.IsCorrect
            });
        }

        private bool Validate()
        {
            foreach (var question in Item.Questions)
            {
                var answered = question.Options.Any(o => o.IsSelected);

                if (answered is false)
                {
                    ErrorMessageProp.Value = "Ответьте на все вопросы";
                    return false;
                }
            }

            return true;
        }
    }
}