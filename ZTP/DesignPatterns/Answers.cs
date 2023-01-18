using ZTP.Controllers;
using ZTP.DesignPatterns.Builder;
using ZTP.DesignPatterns.Decorator;
using ZTP.Models;
using ZTP.Models.Enum;
using ZTP.State;

namespace ZTP.DesignPatterns
{
    public class Answers
    {
        private List<Word> _answers;
        private readonly ZTPDbContext _context;
        private int _userId;
        private Context _contextState;
        private AnswerDirector _director;
        private DatabaseConnection db;


        public Word CorrectAnswer { get; set; }

        public Answers(ZTPDbContext context, int userId, Context contextState)
        {
            _director = new AnswerDirector();
            _context = context;
            _userId = userId;
            _contextState = contextState;
            this.db = new DatabaseConnection(this._context);
        }

        private void DecorateAnswers(IAnswersDecorator answersDecorator)
        {
            _answers = answersDecorator.DecorateAnswers(_answers);
        }

        public List<Word> GetAnswersList()
        {
            User user = _context.Users.Where(x => x.Id == _userId).FirstOrDefault();
            Difficulty difficulty = user.Difficulty;

            if (_contextState.CheckState() is LearningState)
            {
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

                UserWord userWord = new UserWord();
                userWord.UserId = _userId;
                userWord.WordId = CorrectAnswer.Id;
                userWord.IsLearned = false;

                _context.UserWords.Add(userWord);
                _context.SaveChanges();
            }
            else
            {
                AnswerBuilderTest builderHard = new AnswerBuilderTest(_context, _userId);
                _director.Construct(builderHard);
                _answers = builderHard.AnswerWords;
                CorrectAnswer = builderHard.CorrectAnswer;

                UserWord userWord = this.db.FindUserWord(_userId, CorrectAnswer.Id);
                userWord.IsLearned = true;

                _context.UserWords.Update(userWord);
                _context.SaveChanges();
            }

            return _answers;
        }
    }
}