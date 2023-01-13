using ZTP.Models;

namespace ZTP.DesignPatterns.Decorator
{
    public interface AnswersDecorator
    {
        public List<string> DecorateAnswers(List<string> words);
    }
}