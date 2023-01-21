using ZTP.Models;

namespace ZTP.DesignPatterns.Builder
{
    public abstract class AnswerBuilder
    {
        public abstract void BuildWord();

        public abstract void BuildAnswer();
        public abstract List<Word> GetResult();
    }
}