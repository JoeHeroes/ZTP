using ZTP.DesignPatterns.Decorator;
using ZTP.DesignPatterns.Iterator;
using ZTP.Models;
using ZTP.Models.Enum;
using ZTP.Models.ModelView;
using ZTP.State;

namespace ZTP.DesignPatterns
{
    public class AnswersQuestions
    {
        private readonly ZTPDbContext context;
        private int userId;

        public Context ContextState;
        public IteratorQuestion Iterator { get; set; }

        public AnswersQuestions(ZTPDbContext context, int userId)
        {
            this.context = context;
            this.userId = userId;
        }

        public void GenerateQuestions(int number)                      //metoda generująca listę pytań
        {
            List<QuestionViewModel> Questions = new List<QuestionViewModel>();

            for (int i = 0; i < number; i++)
            {
                QuestionViewModel question = GetQuestionFromDB();
                question.QuestionNumber = i + 1;
                Questions.Add(question);
            }

            Iterator = new IteratorQuestion(Questions);

            if (ContextState.CheckState() is LearningState)
            {
                Remove(Questions);
            }
            else
            {
                Update(Questions);
            }
        }

        public QuestionViewModel GetQuestion()                                  //metoda zwracająca pytanie z iteratora
        {
            if (!Iterator.IsDone())
            {
                QuestionViewModel currentQuestion = Iterator.Next();

                return currentQuestion;
            }

            return null;
        }

        private QuestionViewModel GetQuestionFromDB()                              //metoda tworząca pytania ze względu na trudność
        {
            User user = context.Users.Where(x => x.Id == userId).FirstOrDefault();
            Difficulty difficulty = user.Difficulty;
            Answers answers = null;

            QuestionViewModel questionViewModel = new QuestionViewModel();
            //względem wybranej trudności wybierz odpowiedni dekorator
            if (Difficulty.Easy == user.Difficulty)                        //dla łatwego trybu -> mieszanie kolejności odpowiedzi
            {
                questionViewModel.Answers = new AnswerDecoratorMixList(answers= new Answers(context, userId, ContextState)).GetAnswersList();
                questionViewModel.CorrectWord = answers.CorrectAnswer;
            }
            else if(Difficulty.Normal == user.Difficulty)                  //dla normalengo trybu -> mieszanie liter w odpowiedziach
            {
                questionViewModel.Answers = new AnswerDecorateMixLetters(answers= new Answers(context, userId, ContextState)).GetAnswersList();
                questionViewModel.CorrectWord = questionViewModel.Answers.First();
            }
            else if(Difficulty.Hard == user.Difficulty) 
            {
                answers = new Answers(context, userId, ContextState);
                questionViewModel.Answers = answers.GetAnswersList();
                questionViewModel.CorrectWord = answers.CorrectAnswer;
            }

            return questionViewModel;
        }

        private void Remove(List<QuestionViewModel> questions)
        {
            foreach (var question in questions)
            {
                UserWord userWord = context.UserWords.Where(x => x.UserId == userId && x.WordId == question.CorrectWord.Id).FirstOrDefault();
                context.Remove(userWord);
            }

            context.SaveChanges();
        }

        private void Update(List<QuestionViewModel> questions)
        {
            foreach (var question in questions)
            {
                UserWord userWord = context.UserWords.Where(x => x.UserId == userId && x.WordId == question.CorrectWord.Id).FirstOrDefault();
                userWord.IsLearned = false;
                context.Update(userWord);
            }

            context.SaveChanges();
        }
    }
}