using ZTP.Models;

namespace ZTP.DesignPatterns.Decorator
{
     public abstract class AnswersDecorator : IAnswers
    {
       protected IAnswers answers;

        public AnswersDecorator(IAnswers answers)
        {
            this.answers = answers;
        }

        public override List<Word> GetAnswersList()     //zwraca listę odpowiedzi do pytania
        {
            return answers.GetAnswersList();
        }
    }
}