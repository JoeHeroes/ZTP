using ZTP.Models;

namespace ZTP.DesignPatterns.Decorator
{
    public class AnswerDecoratorMixList : IAnswersDecorator
    {
        public List<Word> DecorateAnswers(List<Word> words)
        {
            Random random = new Random();
            int randomNumber = random.Next(words.Count);

            for (int i = 0; i < randomNumber; i++)
            {
                Word temp = words[0];
                words[0] = words[1];
                words[1] = words[2];
                words[2] = words[3];
                words[3] = temp;
            }

            return words;
        }
    }
}