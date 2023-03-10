using NuGet.Packaging.Signing;
using System.Collections.Generic;
using ZTP.Models;

namespace ZTP.DesignPatterns.Decorator
{
    public class AnswerDecorateMixLetters : AnswersDecorator                  //dekorator miesza litery w odpowiedziach
    {
        public AnswerDecorateMixLetters(IAnswers answers) : base(answers)
        {}
        public override List<Word> GetAnswersList()                  //zwraca listę odpowiedzi do pytania
        {
            List<Word> words = answers.GetAnswersList();
            List<Word> answerList = new List<Word>();
            Word correctWord = words.First();
            answerList.Add(correctWord);

            for (int i = 0; i < words.Count - 1; i++)
            {
                Word temp = new Word();
                temp.PolishWord = MixLetters(correctWord.PolishWord);
                if (answerList.Where(x => x.PolishWord == temp.PolishWord).Any())
                {
                    temp.PolishWord = MixLettersSecond(correctWord.PolishWord);
                }

                temp.ForeignLanguageWord = MixLetters(correctWord.ForeignLanguageWord);
                if (answerList.Where(x => x.ForeignLanguageWord == temp.ForeignLanguageWord).Any())
                {
                    temp.ForeignLanguageWord = MixLettersSecond(correctWord.ForeignLanguageWord);
                }
                answerList.Add(temp);
            }

            return answerList;
        }

        private string MixLetters(string word)             //pierwsza metoda mieszająca litery
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
                for (int j = 1; j < wordLength - 2; j++)
                {
                    char temp = liters[j];
                    liters[j] = liters[j + 1];
                    liters[j + 1] = temp;
                }
            }

            word = new String(liters);

            return word;
        }

        private string MixLettersSecond(string word)     //druga metoda mieszająca litery
        {
            int wordLength = word.Length;
            char[] liters = new char[wordLength];
            for (int i = 0; i < wordLength; i++)
            {
                liters[i] = word[i];
            }

            Random random = new Random();
            int number = random.Next(word.Length - 2) + 1;

            if (wordLength > 3)
            {
                for (int i = 0; i < number; i++)
                {
                    for (int j = 1; j < wordLength - 3; j++)
                    {
                        char temp = liters[j];
                        liters[j] = liters[j + 2];
                        liters[j + 2] = temp;
                    }
                }
            }
            else
            {
                for (int i = 0; i < number; i++)
                {
                    for (int j = 0; j < wordLength - 2; j++)
                    {
                        char temp = liters[j];
                        liters[j] = liters[j + 2];
                        liters[j + 2] = temp;
                    }
                }
            }

            word = new String(liters);

            return word;
        }
    }
}