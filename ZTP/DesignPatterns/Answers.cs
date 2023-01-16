using ZTP.DesignPatterns.Builder;
using ZTP.DesignPatterns.Decorator;
using ZTP.Models;
using ZTP.Models.Enum;

namespace ZTP.DesignPatterns
{
    public class Answers
    {
        private List<Word> _answers;
        private readonly ZTPDbContext _context;
        private int _userId;

        public Word CorrectAnswer { get; set; }
        private AnswerDirector _director;

        public Answers(ZTPDbContext context, int userId)
        {
            _director = new AnswerDirector();
            _context = context;
            _userId = userId;
        }

        private void DecorateAnswers(AnswersDecorator answersDecorator)
        {
            _answers = answersDecorator.DecorateAnswers(_answers);
        }

        public List<Word> GetAnswersList()
        {
            User user = _context.Users.Where(x => x.Id == _userId).FirstOrDefault();
            Difficulty difficulty = user.Difficulty;

            if (difficulty == Difficulty.Easy)
            {
                AnswerBuilderEasy builderEasy = new AnswerBuilderEasy(_context, _userId);
                _director.Construct(builderEasy);
                _answers = builderEasy.AnswerWords;
                CorrectAnswer = builderEasy.CorrectAnswer;

                AnswerDecoratorMixList decoratorMixList = new AnswerDecoratorMixList();
                DecorateAnswers(decoratorMixList);
            }
            else if (difficulty == Difficulty.Normal)
            {
                AnswerBuilderNormal builderNormal = new AnswerBuilderNormal(_context, _userId);
                _director.Construct(builderNormal);
                _answers = builderNormal.AnswerWords;
                CorrectAnswer = builderNormal.CorrectAnswer;

                AnswerDecorateMixLetters decorateMixLetters = new AnswerDecorateMixLetters();
                DecorateAnswers(decorateMixLetters);
            }
            else if (difficulty == Difficulty.Hard)
            {
                AnswerBuilderHard builderHard = new AnswerBuilderHard(_context, _userId);
                _director.Construct(builderHard);
                _answers = builderHard.AnswerWords;
                CorrectAnswer = builderHard.CorrectAnswer;
            }

            return _answers;
        }
    }
}