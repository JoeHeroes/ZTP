using System.Collections.ObjectModel;
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
        IList<QuestionViewModel> questions = new ObservableCollection<QuestionViewModel>();
        public Context ContextState;
        public QuestionsIterator Iterator { get; set; }

        public AnswersQuestions(ZTPDbContext context, int userId)            //inicjalizacja 
        {
            this.context = context;
            this.userId = userId;
        }

        public void GenerateQuestions(int number)                      //metoda generująca listę pytań
        {
            for (int i = 0; i < number; i++)
            {
                QuestionViewModel question = GetQuestionFromDB();
                question.QuestionNumber = i + 1;
                questions.Add(question);
            }

            Iterator = new QuestionsIterator(questions);

            if (ContextState.CheckState() is LearningState)
            {
                Remove(questions);
            }
            else
            {
                Update(questions);
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
                questionViewModel.CorrectWord = answers.correctAnswer;
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
                questionViewModel.CorrectWord = answers.correctAnswer;
            }

            return questionViewModel;
        }

        private void Remove(IList<QuestionViewModel> questions)
        {
            foreach (var question in questions)
            {
                UserWord userWord = context.UserWords.Where(x => x.UserId == userId && x.WordId == question.CorrectWord.Id).FirstOrDefault();
                context.Remove(userWord);
            }

            context.SaveChanges();
        }

        private void Update(IList<QuestionViewModel> questions)
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