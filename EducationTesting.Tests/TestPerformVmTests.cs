using System;
using System.Linq;
using System.Windows;
using EducationTesting.Client;
using EducationTesting.Client.Helpers;
using EducationTesting.Client.Models.Tests;
using EducationTesting.Client.Models.Users;
using EducationTesting.Client.Services;
using EducationTesting.Client.Stores;
using EducationTesting.Client.ViewModels;
using Moq;
using Moq.Language.Flow;
using Xunit;

namespace EducationTesting.Tests
{
    public class TestPerformVmTests
    {
        private readonly TestPerformVm _sut;
        private readonly Mock<ITestsStore> _testsStoreMock;
        private readonly Mock<ITestsResultsService> _testResultsServiceMock;
        private readonly Mock<IAuthStore> _authStoreMock;
        private readonly Mock<IMainLayoutNavStore> _mainLayoutNavStoreMock;
        private readonly Mock<IGuidProvider> _guidProviderMock;
        private readonly Mock<IMomentProvider> _momentProviderMock;
        private readonly Mock<IProperty<object>> _currentVmPropertyMock;
        private readonly ISetup<IMessageBoxProvider, MessageBoxResult> _showMessageBoxSetup;

        public TestPerformVmTests()
        {
            _testsStoreMock = new Mock<ITestsStore>();
            _testResultsServiceMock = new Mock<ITestsResultsService>();
            _authStoreMock = new Mock<IAuthStore>();
            _mainLayoutNavStoreMock = new Mock<IMainLayoutNavStore>();
            _guidProviderMock = new Mock<IGuidProvider>();
            _momentProviderMock = new Mock<IMomentProvider>();
            _currentVmPropertyMock = new Mock<IProperty<object>>();
            var messageBoxProviderMock = new Mock<IMessageBoxProvider>();
            _showMessageBoxSetup = messageBoxProviderMock
                .Setup(s => s.Show(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>(),
                    It.IsAny<MessageBoxImage>()));

            ConfigureSut();

            _sut = new TestPerformVm(_testsStoreMock.Object, _testResultsServiceMock.Object, _authStoreMock.Object,
                _mainLayoutNavStoreMock.Object, new Lazy<ITestsVm>(Mock.Of<ITestsVm>), _guidProviderMock.Object,
                _momentProviderMock.Object, messageBoxProviderMock.Object);
        }

        [Fact]
        public void SaveResult_SuccessMaxGrade()
        {
            // Arrange
            _sut.Item.Questions.ElementAt(0).Options.ElementAt(0).IsSelected = true;
            _sut.Item.Questions.ElementAt(1).Options.ElementAt(0).IsSelected = true;
            _sut.Item.Questions.ElementAt(2).Options.ElementAt(0).IsSelected = true;
            _sut.Item.Questions.ElementAt(2).Options.ElementAt(1).IsSelected = true;
            _showMessageBoxSetup.Returns(MessageBoxResult.Yes);

            // Act
            _sut.SaveCommand.Execute(null);

            // Assert
            _testResultsServiceMock.Verify(
                s => s.Create(It.Is<TestResult>(r =>
                    r.Id == "e3b36533-3273-4cb3-ba3b-a8f152fdfd3a" && r.MaxScore == 4 && r.AchievedScore == 4 &&
                    r.Percentage == 100 && r.Grade == 5)),
                Times.Once);
        }

        [Fact]
        public void SaveResult_SkippedCorrectAnswer()
        {
            // Arrange
            _sut.Item.Questions.ElementAt(0).Options.ElementAt(0).IsSelected = true;
            _sut.Item.Questions.ElementAt(1).Options.ElementAt(0).IsSelected = true;
            _sut.Item.Questions.ElementAt(2).Options.ElementAt(0).IsSelected = true;
            _showMessageBoxSetup.Returns(MessageBoxResult.Yes);

            // Act
            _sut.SaveCommand.Execute(null);

            // Assert
            _testResultsServiceMock.Verify(
                s => s.Create(It.Is<TestResult>(r =>
                    r.Id == "e3b36533-3273-4cb3-ba3b-a8f152fdfd3a" && r.MaxScore == 4 && r.AchievedScore == 3 &&
                    r.Percentage == 75 && r.Grade == 4)),
                Times.Once);
        }

        [Fact]
        public void SaveResult_SelectedIncorrectAnswer()
        {
            // Arrange
            _sut.Item.Questions.ElementAt(0).Options.ElementAt(0).IsSelected = true;
            _sut.Item.Questions.ElementAt(1).Options.ElementAt(0).IsSelected = true;
            _sut.Item.Questions.ElementAt(2).Options.ElementAt(0).IsSelected = true;
            _sut.Item.Questions.ElementAt(2).Options.ElementAt(1).IsSelected = true;
            _sut.Item.Questions.ElementAt(2).Options.ElementAt(2).IsSelected = true;
            _showMessageBoxSetup.Returns(MessageBoxResult.Yes);

            // Act
            _sut.SaveCommand.Execute(null);

            // Assert
            _testResultsServiceMock.Verify(
                s => s.Create(It.Is<TestResult>(r =>
                    r.Id == "e3b36533-3273-4cb3-ba3b-a8f152fdfd3a" && r.MaxScore == 4 && r.AchievedScore == 3 &&
                    r.Percentage == 75 && r.Grade == 4)),
                Times.Once);
        }

        [Fact]
        public void SaveResult_SkippedQuestion()
        {
            // Arrange
            _sut.Item.Questions.ElementAt(0).Options.ElementAt(0).IsSelected = true;
            _sut.Item.Questions.ElementAt(1).Options.ElementAt(0).IsSelected = true;

            // Act
            _sut.SaveCommand.Execute(null);

            // Assert
            _testResultsServiceMock.Verify(
                s => s.Create(It.IsAny<TestResult>()), Times.Never);
            Assert.Equal(_sut.ErrorMessageProp.Value, "Ответьте на все вопросы");
        }

        [Fact]
        public void SaveResult_Cancel()
        {
            // Arrange
            _showMessageBoxSetup.Returns(MessageBoxResult.No);

            // Act
            _sut.SaveCommand.Execute(null);

            // Assert
            _testResultsServiceMock.Verify(
                s => s.Create(It.IsAny<TestResult>()), Times.Never);
        }

        private void ConfigureSut()
        {
            _testsStoreMock.SetupGet(s => s.SelectedTest).Returns(new Test
            {
                Id = "3cd15f70-f75b-4c3c-8427-6a792ddc79c1",
                Questions = new[]
                {
                    new Question
                    {
                        Id = "3ddc5ba9-7df8-4589-b575-18cf6dc4b360",
                        Options = new[]
                        {
                            new AnswerOption
                            {
                                IsCorrect = true,
                            },
                            new AnswerOption
                            {
                                IsCorrect = false,
                            }
                        }
                    },
                    new Question
                    {
                        Id = "d690986f-8caa-4b99-91fc-1b3173ce4afa",
                        Options = new[]
                        {
                            new AnswerOption
                            {
                                IsCorrect = true,
                            },
                            new AnswerOption
                            {
                                IsCorrect = false,
                            }
                        }
                    },
                    new Question
                    {
                        Id = "6b2b7f41-bbc9-4788-91d9-8769aedc444c",
                        Options = new[]
                        {
                            new AnswerOption
                            {
                                IsCorrect = true,
                            },
                            new AnswerOption
                            {
                                IsCorrect = true,
                            },
                            new AnswerOption
                            {
                                IsCorrect = false,
                            }
                        }
                    }
                },
                TeacherId = "b184b2fd-80be-42e3-8143-169bdabc1136"
            });
            _guidProviderMock.Setup(s => s.NewGuid()).Returns("e3b36533-3273-4cb3-ba3b-a8f152fdfd3a");
            _momentProviderMock.SetupGet(s => s.Now).Returns(DateTime.Parse("2021-01-01 00:00:00.0000000"));
            _authStoreMock.SetupGet(s => s.CurrentUser).Returns(new User
            {
                Id = "e3b36533-3273-4cb3-ba3b-a8f152fdfd3a"
            });
            _mainLayoutNavStoreMock.SetupGet(s => s.CurrentVmProp).Returns(_currentVmPropertyMock.Object);
        }
    }
}