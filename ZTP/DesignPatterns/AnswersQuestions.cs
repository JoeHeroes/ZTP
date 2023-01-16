using ZTP.DesignPatterns.Iterator;
using ZTP.Models;
using ZTP.Models.ModelView;
using ZTP.State;

namespace ZTP.DesignPatterns
{
    public class AnswersQuestions
    {
        private readonly ZTPDbContext _context;
        private IteratorQuestion _iterator;
        private int _userId;
        private Context _contextState;

        public List<QuestionViewModel> Questions { get; set; }

        public AnswersQuestions(ZTPDbContext context, int userId, Context contextState)
        {
            _context = context;
            Questions = new List<QuestionViewModel>();
            _iterator = new IteratorQuestion(Questions);
            _userId = userId;
            _contextState = contextState;
        }

        public void GenerateQuestions()
        {
            for (int i = 0; i < 10; i++)
            {
                QuestionViewModel question = GetQuestion();
                Questions.Add(question);
            }
        }

        public QuestionViewModel GetQuestion()
        {
            Answers answers = new Answers(_context, _userId, _contextState);

            QuestionViewModel questionViewModel = new QuestionViewModel();
            questionViewModel.Answers = answers.GetAnswersList();
            questionViewModel.CorrectWord = answers.CorrectAnswer;

            return questionViewModel;
        }
    }
}