using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EducationTesting.Client.Models.Tests;

namespace EducationTesting.Client.Helpers
{
    public class AnsweredOptionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SingleOptionTemplate { get; set; }
        public DataTemplate ManyOptionsTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is AnsweredQuestion answeredQuestion)
            {
                return answeredQuestion.Question.Type == QuestionType.SingleOption
                    ? SingleOptionTemplate
                    : ManyOptionsTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}