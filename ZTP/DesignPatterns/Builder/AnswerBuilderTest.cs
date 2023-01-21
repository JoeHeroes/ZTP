using ZTP.Models;

namespace ZTP.DesignPatterns.Builder
{
    public class AnswerBuilderTest : AnswerBuilder
    {
        private readonly ZTPDbContext _context;
        private int _userId;
        private List<Word> _answerWords { get; set; }

        public AnswerBuilderTest(ZTPDbContext context, int userId)
        {
            _context = context;
            _userId = userId;
            _answerWords = new List<Word>();
        }

        public override void BuildAnswer()
        {
            List<int> userWordsIds = _context.UserWords.Where(x => x.UserId == _userId && !x.IsLearned).Select(x => x.WordId).ToList();

            Word word = _context.Words.Where(x => userWordsIds.Contains(x.Id)).FirstOrDefault();

            _answerWords.Add(word);
        }
        public override void BuildWord()
        {}
        public override List<Word> GetResult()
        {
            return _answerWords;
        }
    }
}