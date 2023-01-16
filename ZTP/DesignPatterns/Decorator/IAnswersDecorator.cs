using ZTP.Models;

namespace ZTP.DesignPatterns.Decorator
{
    public interface IAnswersDecorator
    {
        public List<Word> DecorateAnswers(List<Word> words);
    }
}