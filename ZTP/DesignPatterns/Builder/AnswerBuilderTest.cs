using ZTP.Models;

namespace ZTP.DesignPatterns.Builder
{
    public class AnswerBuilderTest : IAnswerBuilder
    {
        private readonly ZTPDbContext _context;
        private int _userId;

        public List<Word> AnswerWords { get; set; }
        public Word CorrectAnswer { get; set; }

        public AnswerBuilderTest(ZTPDbContext context, int userId)
        {
            _context = context;
            AnswerWords = new List<Word>();
            _userId = userId;
        }

        public Word BuildWord()
        {
            List<int> userWordsIds = _context.UserWords.Where(x => x.UserId == _userId && !x.IsLearned).Select(x => x.WordId).ToList();

            Word word = _context.Words.Where(x => userWordsIds.Contains(x.Id)).FirstOrDefault();

            return word;
        }

        public void GetResult()
        {
            CorrectAnswer = BuildWord();
            AnswerWords.Add(CorrectAnswer);
        }
    }
}