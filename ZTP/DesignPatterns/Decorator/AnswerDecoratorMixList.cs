using ZTP.Models;

namespace ZTP.DesignPatterns.Decorator
{
    public class AnswerDecoratorMixList : AnswersDecorator
    {
        public List<string> DecorateAnswers(List<string> words)
        {
            Random random = new Random();
            int randomNumber = random.Next(words.Count);

            for (int i = 0; i < randomNumber; i++)
            {
                string temp = words[0];
                words[0] = words[1];
                words[1] = words[2];
                words[2] = words[3];
                words[3] = temp;
            }

            return words;
        }
    }
}