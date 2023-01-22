using ZTP.Models.ModelView;

namespace ZTP.DesignPatterns.Iterator
{
    public class IteratorQuestion : IIterator
    {
        public List<QuestionViewModel> questions { get; set; }
        public int numberOfQuestions;

        public IteratorQuestion(List<QuestionViewModel> questions)
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