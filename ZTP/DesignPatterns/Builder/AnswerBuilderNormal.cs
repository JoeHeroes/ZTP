using ZTP.Models;

namespace ZTP.DesignPatterns.Builder
{
    public class AnswerBuilderNormal : IAnswerBuilder
    {
        private readonly ZTPDbContext _context;
        private int _userId;

        public List<Word> AnswerWords { get; set; }
        public Word CorrectAnswer { get; set; }

        public AnswerBuilderNormal(ZTPDbContext context, int userId)
        {
            _context = context;
            AnswerWords = new List<Word>();
            _userId = userId;
        }

        public Word BuildWord()
        {
            List<int> userWordsIds = _context.UserWords.Where(x => x.UserId == _userId).Select(x => x.WordId).ToList();
            if (AnswerWords.Count != 0)
            {
                userWordsIds.AddRange(AnswerWords.Select(x => x.Id));
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

        public void GetResult()
        {
            CorrectAnswer = BuildWord();
            AnswerWords.Add(CorrectAnswer);
            AnswerWords.Add(CorrectAnswer);
            AnswerWords.Add(CorrectAnswer);
            AnswerWords.Add(CorrectAnswer);
        }
    }
}