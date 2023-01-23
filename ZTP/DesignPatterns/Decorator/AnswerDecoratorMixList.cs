using ZTP.Controllers;
using ZTP.Models;

namespace ZTP.DesignPatterns.Decorator
{
    public class AnswerDecoratorMixList : AnswersDecorator                //dekorator mieszający kolejność odpowiedzi
    {
        public AnswerDecoratorMixList(IAnswers answers) : base(answers)
        {
        }
        public override List<Word> GetAnswersList()                  //zwraca listę odpowiedzi do pytania
        {
            List<Word> words = answers.GetAnswersList();
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