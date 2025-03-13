using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using EducationTesting.Client.Models.Tests;

namespace EducationTesting.Client.Helpers
{
    public class AnswerOptionTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SingleOptionTemplate { get; set; }
        public DataTemplate ManyOptionsTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is AnswerOption && container is ContentPresenter element)
            {
                var parent = VisualTreeHelper.GetParent(element) as StackPanel;
                
                // Получаем контекст данных (вопрос) из родительского элемента
                if (parent?.DataContext is Question question)
                {
                    // Выбираем шаблон в зависимости от типа вопроса
                    return question.Type == QuestionType.SingleOption ? SingleOptionTemplate : ManyOptionsTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}