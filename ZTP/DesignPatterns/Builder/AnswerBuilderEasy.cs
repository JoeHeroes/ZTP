using ZTP.Models;

namespace ZTP.DesignPatterns.Builder
{
    public class AnswerBuilderEasy : AnswerBuilder
    {
        private readonly ZTPDbContext _context;
        private int _userId;

        public List<Word> AnswerWords { get; set; }
        public Word CorrectAnswer { get; set; }

        public AnswerBuilderEasy(ZTPDbContext context, int userId)
        {
            _context = context;
            AnswerWords = new List<Word>();
            _userId = userId;
        }

        public Word BuildWord()
        {
            List<int> userWord = _context.UserWords.Where(x => x.UserId == _userId).Select(x => x.WordId).ToList();
            if (AnswerWords.Count != 0)
            {
                userWord.AddRange(AnswerWords.Select(x => x.Id));
            }

            Random random = new Random();
            int number = random.Next(3);

            Word word = null;
            if (number % 3 == 0)
            {
                word = _context.Words.Where(x => !userWord.Contains(x.Id)).FirstOrDefault();
            }
            else if (number % 3 == 1)
            {
                word = _context.Words.Where(x => !userWord.Contains(x.Id)).Skip(2).FirstOrDefault();
            }
            else if (number % 3 == 2)
            {
                word = _context.Words.Where(x => !userWord.Contains(x.Id)).Skip(5).FirstOrDefault();
            }

            return word;
        }

        public void GetResult()
        {
            CorrectAnswer = BuildWord();
            AnswerWords.Add(CorrectAnswer);
            AnswerWords.Add(BuildWord());
            AnswerWords.Add(BuildWord());
            AnswerWords.Add(BuildWord());
        }
    }
}