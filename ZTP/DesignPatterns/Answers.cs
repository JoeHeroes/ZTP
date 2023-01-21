using ZTP.Controllers;
using ZTP.DesignPatterns.Builder;
using ZTP.DesignPatterns.Decorator;
using ZTP.Fascade;
using ZTP.Models;
using ZTP.Models.Enum;
using ZTP.State;

namespace ZTP.DesignPatterns
{
    public class Answers : IAnswers
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



        public override List<Word> GetAnswersList()
        {
            AnswerBuilder builder = null;
            if (_contextState.CheckState() is LearningState)
            {
                User user = _context.Users.Where(x => x.Id == _userId).FirstOrDefault();
                Difficulty difficulty = user.Difficulty;
                if (difficulty == Difficulty.Easy)
                {
                    builder = new AnswerBuilderEasy(_context, _userId);
                    _answers = _director.Construct(builder);
                }
                else if (difficulty == Difficulty.Normal)
                {
                    builder = new AnswerBuilderNormal(_context, _userId);
                    _answers = _director.Construct(builder);

                   
                }
                else if (difficulty == Difficulty.Hard)
                {
                    builder = new AnswerBuilderHard(_context, _userId);
                    _answers = _director.Construct(builder);
                }

                CorrectAnswer = _answers[0];
                UserWord userWord = new UserWord();
                userWord.UserId = _userId;
                userWord.WordId = CorrectAnswer.Id;
                userWord.IsLearned = false;

                _context.UserWords.Add(userWord);
                _context.SaveChanges();
            }
            else
            {
                builder = new AnswerBuilderTest(_context, _userId);
                _answers = _director.Construct(builder);
                CorrectAnswer = _answers[0];

                UserWord userWord = this.db.FindUserWord(_userId, CorrectAnswer.Id);
                userWord.IsLearned = true;

                _context.UserWords.Update(userWord);
                _context.SaveChanges();
            }

            return _answers;
        }
    }
}