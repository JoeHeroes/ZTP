using ZTP.Models;
using ZTP.Models.ModelView;

namespace ZTP.DesignPatterns
{
    public class AnswersQuestions
    {
        private readonly ZTPDbContext _context;
        private List<QuestionViewModel> Questions { get; set; }

        public AnswersQuestions(ZTPDbContext context)
        {
            _context = context;
        }

        public QuestionViewModel GetQuestion()
        {
            Answers answers = new Answers(_context, 1);

            QuestionViewModel questionViewModel = new QuestionViewModel();
            questionViewModel.Answers = answers.GetAnswersList();
            questionViewModel.CorrectWord = answers.CorrectAnswer;

            return questionViewModel;
        }
    }
}