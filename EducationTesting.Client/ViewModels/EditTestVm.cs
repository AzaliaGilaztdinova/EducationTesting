using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using EducationTesting.Client.Models.Tests;
using EducationTesting.Client.Services;
using EducationTesting.Client.Stores;
using Prism.Commands;

namespace EducationTesting.Client.ViewModels
{
    public class EditTestVm : EditItemVm<Test>
    {
        private readonly ITestsService _testsService;
        private readonly ITestsStore _store;
        private readonly IAuthStore _authStore;

        public ICommand AddQuestionCommand { get; }
        public ICommand AddAnswerOptionCommand { get; }
        public ICommand DeleteQuestionCommand { get; }
        public ICommand DeleteAnswerOptionCommand { get; }

        public EditTestVm(Test item, ITestsService testsService, ITestsStore store, Action goBack,
            IAuthStore authStore) : base(item, goBack)
        {
            _testsService = testsService;
            _store = store;
            _authStore = authStore;
            AddQuestionCommand = new DelegateCommand(AddQuestion);
            AddAnswerOptionCommand = new DelegateCommand<Question>(AddAnswerOption);
            DeleteQuestionCommand = new DelegateCommand<Question>(DeleteQuestion);
            DeleteAnswerOptionCommand = new DelegateCommand<AnswerOption>(DeleteAnswerOption);
        }

        protected override void Save()
        {
            // Проверка валидации
            if (Validate() is false)
            {
                return;
            }
            try
            {
                bool isNew = Item.Id is null;

                if (isNew)
                {
                    // Создание нового теста
                    Item.Id = Guid.NewGuid().ToString();
                    Item.CreatedAt = DateTime.Now;

                    foreach (var question in Item.Questions)
                    {
                        question.TestId = Item.Id;
                    }

                    Item.SubjectId = _store.SelectedSubject.Id;
                    Item.TeacherId = _authStore.CurrentUser.Id;

                    // Вызов сервиса для создания теста
                    _testsService.Create(Item);

                    // Сообщение об успешном создании
                    MessageBox.Show("Тест успешно создан!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Обновление существующего теста
                    _testsService.Update(Item);

                    // Сообщение об успешном редактировании
                    MessageBox.Show("Тест успешно отредактирован!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }

                // Возврат к предыдущему экрану
                GoBack();
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void AddQuestion()
        {
            var question = new Question
            {
                Id = Guid.NewGuid().ToString()
            };
            Item.Questions = Item.Questions.Append(question);
            RaisePropertyChanged(nameof(Item));
        }

        private void DeleteQuestion(Question item)
        {
            Item.Questions = Item.Questions.Where(q => q != item);
            RaisePropertyChanged(nameof(Item));
        }

        private void AddAnswerOption(Question item)
        {
            var options = (ObservableCollection<AnswerOption>)item.Options;

            options.Add(new AnswerOption
            {
                Id = Guid.NewGuid().ToString(),
                QuestionId = item.Id,
            });
        }

        private void DeleteAnswerOption(AnswerOption item)
        {
            var question = Item.Questions.Single(q => q.Options.Contains(item));
            var options = (ObservableCollection<AnswerOption>)question.Options;
            options.Remove(item);
        }

        private bool Validate()
        {
            var isNullOrWhiteSpaceTest = string.IsNullOrWhiteSpace(Item.Name) ||
                                         string.IsNullOrWhiteSpace(Item.Description);

            if (isNullOrWhiteSpaceTest)
            {
                ErrorMessageProp.Value = "Заполните все поля";
                return false;
            }

            var validDuration = Item.Duration > 0;

            if (validDuration is false)
            {
                ErrorMessageProp.Value = "Длительность не корректна";
                return false;
            }

            if (Item.Questions.Any() is false)
            {
                ErrorMessageProp.Value = "Добавьте вопросы";
                return false;
            }

            foreach (var question in Item.Questions)
            {
                var isNullOrWhiteSpaceQuestion = string.IsNullOrWhiteSpace(question.Text);

                if (isNullOrWhiteSpaceQuestion)
                {
                    ErrorMessageProp.Value = "Заполните все поля";
                    return false;
                }

                if (question.Options.Any(o => string.IsNullOrWhiteSpace(o.Text)))
                {
                    ErrorMessageProp.Value = "Заполните все поля";
                    return false;
                }

                if (question.Options.Count() < 2)
                {
                    ErrorMessageProp.Value = "Добавьте несколько вариантов ответа для каждого вопроса";
                    return false;
                }

                var existsCorrectOption = question.Options.Any(o => o.IsCorrect);

                if (existsCorrectOption is false)
                {
                    ErrorMessageProp.Value = "Укажите правильные ответы для всех вопросов";
                    return false;
                }
            }

            return true;
        }
    }
}