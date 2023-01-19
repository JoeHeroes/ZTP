using ZTP.Models;

namespace ZTP.DesignPatterns.Builder
{
    public class AnswerBuilderEasy : IAnswerBuilder
    {
        private readonly ZTPDbContext _context;
        private int _userId;
        private List<Word> _answerWords;

        public AnswerBuilderEasy(ZTPDbContext context, int userId)
        {
            _context = context;
            _userId = userId;
            _answerWords = new List<Word>();
        }

        public Word BuildWord()
        {
            List<int> userWordsIds = _context.UserWords.Where(x => x.UserId == _userId).Select(x => x.WordId).ToList();
            if (_answerWords.Count != 0)
            {
                userWordsIds.AddRange(_answerWords.Select(x => x.Id));
            }

            Random random = new Random();
            int number = random.Next(3);

            Word word = null;
            if (number % 3 == 0)
            {
                word = _context.Words.Where(x => !userWordsIds.Contains(x.Id)).FirstOrDefault();
            }
            else if (number % 3 == 1)
            {
                word = _context.Words.Where(x => !userWordsIds.Contains(x.Id)).Skip(2).FirstOrDefault();
            }
            else if (number % 3 == 2)
            {
                word = _context.Words.Where(x => !userWordsIds.Contains(x.Id)).Skip(5).FirstOrDefault();
            }

            return word;
        }

        public List<Word> GetResult()
        {
            Word correctAnswer = BuildWord();
            _answerWords.Add(correctAnswer);
            _answerWords.Add(BuildWord());
            _answerWords.Add(BuildWord());
            _answerWords.Add(BuildWord());

            return _answerWords;
        }
    }
}