using ZTP.Models;

namespace ZTP.DesignPatterns.Builder
{
    public class AnswerDirector
    {
        public List<Word> Construct(AnswerBuilder builder)
        {
            builder.BuildAnswer();
            builder.BuildWord();
            builder.BuildWord();
            builder.BuildWord();
            return builder.GetResult();
        }
    }
}