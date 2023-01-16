using ZTP.Models;

namespace ZTP.DesignPatterns.Builder
{
    public interface AnswerBuilder
    {
        public Word BuildWord();

        public void GetResult();
    }
}