using ZTP.Models;

namespace ZTP.DesignPatterns.Builder
{
    public class AnswerDirector
    {
        public List<Word> Construct(IAnswerBuilder builder)
        {
            return builder.GetResult();
        }
    }
}