using ZTP.Models;

namespace ZTP.DesignPatterns.Builder
{
    public abstract class AnswerBuilder
    {
        public abstract void BuildWord();                 //tworzenie odpowiedzi

        public abstract void BuildCorrectAnswer();        //tworzenie poprawnej odpowiedzi
        public abstract List<Word> GetResult();
    }
}