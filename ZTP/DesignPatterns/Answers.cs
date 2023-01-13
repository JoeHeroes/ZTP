using ZTP.DesignPatterns.Decorator;
using ZTP.Models;

namespace ZTP.DesignPatterns
{
    public class Answers
    {
        private List<Word> _answers;

        public Word CorrectAnswer { get; set; }

        public Answers(List<Word> answers)
        {
            _answers = answers;
        }

        public List<string> DecorateAnswers(AnswersDecorator answersDecorator)
        {
            List<string> foreginAnswers = new List<string>();
            foreach (var word in _answers)
            {
                foreginAnswers.Add(word.ForeignLanguageWord);
            }

            return answersDecorator.DecorateAnswers(foreginAnswers);
        }
    }
}