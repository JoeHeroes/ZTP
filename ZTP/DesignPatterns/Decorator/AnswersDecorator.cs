using ZTP.Models;

namespace ZTP.DesignPatterns.Decorator
{
    public interface AnswersDecorator
    {
        public List<Word> DecorateAnswers(List<Word> words);
    }
}