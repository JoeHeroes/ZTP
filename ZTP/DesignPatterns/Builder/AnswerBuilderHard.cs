using ZTP.Models;

namespace ZTP.DesignPatterns.Builder
{
    public class AnswerBuilderHard : AnswerBuilder
    {
        private readonly ZTPDbContext _context;
        public List<Word> AnswerWords { get; set; }
        public Word CorrectAnswer { get; set; }

        public AnswerBuilderHard(ZTPDbContext context)
        {
            _context = context;
            AnswerWords = new List<Word>();
        }

        public Word BuildWord()
        {
            Random random = new Random();
            int number = random.Next(8) + 2;

            return _context.Words.Where(x => x.Id == number).FirstOrDefault();
        }

        public void GetResult()
        {
            CorrectAnswer = BuildWord();
            AnswerWords.Add(CorrectAnswer);
        }
    }
}