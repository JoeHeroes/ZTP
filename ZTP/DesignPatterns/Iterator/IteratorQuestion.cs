using ZTP.Models.ModelView;

namespace ZTP.DesignPatterns.Iterator
{
    public class IteratorQuestion : IIterator
    {
        public List<QuestionViewModel> Questions { get; set; }
        private int _numberOfQuestions;

        public IteratorQuestion(List<QuestionViewModel> questions)
        {
            Questions = questions;
            _numberOfQuestions = 0;
        }

        public QuestionViewModel CurrentItem()
        {
            return Questions[_numberOfQuestions];
        }

        public QuestionViewModel First()
        {
            _numberOfQuestions = 0;
            QuestionViewModel question = CurrentItem();

            return question;
        }

        public bool IsDone()
        {
            return !(_numberOfQuestions < Questions.Count);
        }

        public QuestionViewModel Next()
        {
            _numberOfQuestions++;
            QuestionViewModel question = CurrentItem();

            return question;
        }
    }
}