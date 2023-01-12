namespace ZTP.DesignPatterns.Decorator
{
    public class AnswerDecorateMixLetters : AnswersDecorator
    {
        public List<string> DecorateAnswers(List<string> words)
        {
            string correctWord = words.First();
            List<string> answerList = new List<string>();
            answerList.Add(correctWord);

            for (int i = 0; i < 3; i++)
            {
                string mixWord = MixLetters(correctWord);
                answerList.Add(mixWord);
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
            int number = random.Next(word.Length) + 1;

            for (int i = 0; i < number; i++)
            {
                for (int j = 1; j < wordLength - 1; j++)
                {
                    char temp = liters[j];
                    liters[j] = liters[j + 1];
                    liters[j + 1] = temp;
                }
            }

            word = Convert.ToString(liters);

            return word;
        }
    }
}