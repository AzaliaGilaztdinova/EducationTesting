using System.Windows;
using System.Windows.Controls;
using EducationTesting.Client.Models.Tests;

namespace EducationTesting.Client.Helpers
{
    public class QuestionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SingleOptionTemplate { get; set; }
        public DataTemplate ManyOptionsTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is Question question)
            {
                return question.Type == QuestionType.SingleOption ? SingleOptionTemplate : ManyOptionsTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}