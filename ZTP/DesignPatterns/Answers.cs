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
        private List<Word> answers;
        private readonly ZTPDbContext context;
        private int userId;
        private Context contextState;
        private AnswerDirector answerDirector;
        private DatabaseConnection db;

        public Word correctAnswer { get; set; }

        public Answers(ZTPDbContext context, int userId, Context contextState)
        {
            answerDirector = new AnswerDirector();
            this.context = context;
            this.userId = userId;
            this.contextState = contextState;
            this.db = new DatabaseConnection(this.context);
        }



        public override List<Word> GetAnswersList()       //zwraca listę odpowiedzi do pytania
        {
            AnswerBuilder builder = null;
            if (contextState.CheckState() is LearningState)           //jeżeli tryb nauki 
            {
                User user = context.Users.Where(x => x.Id == userId).FirstOrDefault();
                Difficulty difficulty = user.Difficulty;
                if (difficulty == Difficulty.Easy)                    //wybranie buildera na podstawie trudności
                {
                    builder = new AnswerBuilderEasy(context, userId);
                }
                else if (difficulty == Difficulty.Normal)
                {
                    builder = new AnswerBuilderNormal(context, userId);
                }
                else if (difficulty == Difficulty.Hard)
                {
                    builder = new AnswerBuilderHard(context, userId);
                }
                answers = answerDirector.Construct(builder);
                correctAnswer = answers[0];
                UserWord userWord = new UserWord();
                userWord.UserId = userId;
                userWord.WordId = correctAnswer.Id;
                userWord.IsLearned = false;

                context.UserWords.Add(userWord);
                context.SaveChanges();
            }
            else                                                //jeżeli tryb testu
            {
                builder = new AnswerBuilderTest(context, userId);
                answers = answerDirector.Construct(builder);
                correctAnswer = answers[0];

                UserWord userWord = this.db.FindUserWord(userId, correctAnswer.Id);
                userWord.IsLearned = true;

                context.UserWords.Update(userWord);
                context.SaveChanges();
            }

            return answers;
        }
    }
}