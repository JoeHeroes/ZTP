using ZTP.Models;

namespace ZTP.DesignPatterns.Builder
{
    public class AnswerBuilderTest : AnswerBuilder      //builder tworzy jedną odpowiedź, z nauczonych przez użytkownika słów
    {
        private readonly ZTPDbContext context;
        private int userId;
        private List<Word> answerWords { get; set; }

        public AnswerBuilderTest(ZTPDbContext context, int userId)
        {
            this.context = context;
            this.userId = userId;
            answerWords = new List<Word>();
        }

        public override void BuildCorrectAnswer()
        {
            List<int> userWordsIds = context.UserWords.Where(x => x.UserId == userId && !x.IsLearned).Select(x => x.WordId).ToList();

            Word word = context.Words.Where(x => userWordsIds.Contains(x.Id)).FirstOrDefault();

            answerWords.Add(word);
        }
        public override void BuildWord()
        {}
        public override List<Word> GetResult()
        {
            return answerWords;
        }
    }
}