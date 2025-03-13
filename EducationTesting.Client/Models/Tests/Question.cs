using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Prism.Mvvm;

namespace EducationTesting.Client.Models.Tests
{
    public class Question : BindableBase
    {
        private IEnumerable<AnswerOption> _options = new ObservableCollection<AnswerOption>();
        private QuestionType _type = QuestionType.SingleOption;
        public string Id { get; set; }
        public string TestId { get; set; }
        public string Text { get; set; }

        public QuestionType Type
        {
            get => _type;
            set
            {
                _type = value;
                NormalizeOptions();
                Options = Options;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Options));
            }
        }

        public int Score => Options.Count(o => o.IsCorrect);

        public IEnumerable<AnswerOption> Options
        {
            get => _options;
            set => _options = new ObservableCollection<AnswerOption>(value);
        }

        private void NormalizeOptions()
        {
            if (_type != QuestionType.SingleOption)
            {
                return;
            }

            foreach (var option in Options)
            {
                option.IsCorrect = false;
            }
        }
    }
}