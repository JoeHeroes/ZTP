using ZTP.Models;

namespace ZTP.DesignPatterns.Decorator
{
    public class AnswerDecorateMixLetters : AnswersDecorator
    {
        public List<Word> DecorateAnswers(List<Word> words)
        {
            Word correctWord = words.First();
            List<Word> answerList = new List<Word>();
            answerList.Add(correctWord);

            for (int i = 0; i < words.Count - 1; i++)
            {
                Word temp = new Word();
                temp.PolishWord = MixLetters(correctWord.PolishWord);
                temp.ForeignLanguageWord = MixLetters(correctWord.ForeignLanguageWord);
                answerList.Add(temp);
            }

            return answerList;
        }

        public string MixLetters(string word)
        {
            int wordLength = word.Length;
            char[] liters = new char[wordLength];
            for (int i = 0; i < wordLength; i++)
            {
                liters[i] = word[i];
            }

            Random random = new Random();
            int number = random.Next(word.Length - 2) + 1;

            for (int i = 0; i < number; i++)
            {
                for (int j = 0; j < wordLength - 1; j++)
                {
                    char temp = liters[j];
                    liters[j] = liters[j + 1];
                    liters[j + 1] = temp;
                }
            }

            word = new String(liters);

            return word;
        }
    }
}