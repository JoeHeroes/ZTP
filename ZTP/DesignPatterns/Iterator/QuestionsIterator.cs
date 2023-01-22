using System.Collections.ObjectModel;
using ZTP.Models.ModelView;

namespace ZTP.DesignPatterns.Iterator
{
    public class QuestionsIterator : IIterator
    {
        public IList<QuestionViewModel> questions { get; set; } = new ObservableCollection<QuestionViewModel>();
        public int numberOfQuestions { get; set; };

        public QuestionsIterator(IList<QuestionViewModel> questions)
        {
            this.questions = questions;
            numberOfQuestions = 1;
        }

        public QuestionViewModel CurrentItem()
        {
            return questions[numberOfQuestions - 1];
        }

        public QuestionViewModel First()
        {
            numberOfQuestions = 1;
            QuestionViewModel question = CurrentItem();

            return question;
        }

        public bool IsDone()
        {
            return !(numberOfQuestions < questions.Count);
        }

        public QuestionViewModel Next()
        {
            numberOfQuestions++;
            QuestionViewModel question = CurrentItem();

            return question;
        }
    }
}