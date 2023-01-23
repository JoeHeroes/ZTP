using ZTP.Models;

namespace ZTP.DesignPatterns.Builder
{
    public class AnswerDirector
    {
        public List<Word> Construct(AnswerBuilder builder)       //metoda tworząca odpowiedzi do pytania
        {
            builder.BuildCorrectAnswer();                       //stworzenie najpierw poprawnej odpowiedzi, a następnie pozostałych
            builder.BuildWord();
            builder.BuildWord();
            builder.BuildWord();
            return builder.GetResult();
        }
    }
}