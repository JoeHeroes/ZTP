using ZTP.Models;

namespace ZTP.DesignPatterns.Builder
{
    public class AnswerBuilderNormal : AnswerBuilder
    {
        private readonly ZTPDbContext context;
        private int userId;
        private List<Word> answerWords;

        public AnswerBuilderNormal(ZTPDbContext context, int userId)
        {
            this.context = context;
            this.userId = userId;
            answerWords = new List<Word>();
        }
        public override void BuildAnswer()
        {
            List<int> userWordsIds = context.UserWords.Where(x => x.UserId == userId).Select(x => x.WordId).ToList();
            if (answerWords.Count != 0)
            {
                userWordsIds.AddRange(answerWords.Select(x => x.Id));
            }

            Random random = new Random();
            int number = random.Next(3);

            Word word = null;
            if (number % 3 == 0)
            {
                word = context.Words.Where(x => !userWordsIds.Contains(x.Id)).FirstOrDefault();
            }
            else if (number % 3 == 1)
            {
                word = context.Words.Where(x => !userWordsIds.Contains(x.Id)).Skip(2).FirstOrDefault();
            }
            else if (number % 3 == 2)
            {
                word = context.Words.Where(x => !userWordsIds.Contains(x.Id)).Skip(5).FirstOrDefault();
            }

            for (int i = 0; i < 4; i++)
            {
                answerWords.Add(word);
            }
        }
        public override void BuildWord()
        {}

        public override List<Word> GetResult()
        {
            return answerWords;
        }
    }
}