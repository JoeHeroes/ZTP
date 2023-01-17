using ZTP.Models.ModelView;

namespace ZTP.DesignPatterns.Iterator
{
    public class IteratorQuestion : IIterator
    {
        public List<QuestionViewModel> Questions { get; set; }
        public int numberOfQuestions;

        public IteratorQuestion(List<QuestionViewModel> questions)
        {
            Questions = questions;
            numberOfQuestions = 1;
        }

        public QuestionViewModel CurrentItem()
        {
            return Questions[numberOfQuestions - 1];
        }

        public QuestionViewModel First()
        {
            numberOfQuestions = 1;
            QuestionViewModel question = CurrentItem();

            return question;
        }

        public bool IsDone()
        {
            return !(numberOfQuestions < Questions.Count);
        }

        public QuestionViewModel Next()
        {
            numberOfQuestions++;
            QuestionViewModel question = CurrentItem();

            return question;
        }
    }
}