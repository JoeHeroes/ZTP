using System.Collections.ObjectModel;
using ZTP.Models.ModelView;

namespace ZTP.DesignPatterns.Iterator
{
    public class QuestionsIterator : IIterator
    {
        public IList<QuestionViewModel> questions { get; set; } = new ObservableCollection<QuestionViewModel>();
        public int numberOfQuestions { get; set; }

        public QuestionsIterator(IList<QuestionViewModel> questions)      //inicjalizacja
        {
            this.questions = questions;
            numberOfQuestions = 1;
        }

        public QuestionViewModel CurrentItem()            //zwraca aktualne pytanie
        {
            return questions[numberOfQuestions - 1];
        }

        public QuestionViewModel First()                  //zwraca pierwsze pytanie z kolekcji
        {
            numberOfQuestions = 1;
            QuestionViewModel question = CurrentItem();

            return question;
        }

        public bool IsDone()                              //sprawdza czy skończyły się pytania
        {
            return !(numberOfQuestions < questions.Count);
        }

        public QuestionViewModel Next()                   //zwraca kolejne pytanie
        {
            numberOfQuestions++;
            QuestionViewModel question = CurrentItem();

            return question;
        }
    }
}