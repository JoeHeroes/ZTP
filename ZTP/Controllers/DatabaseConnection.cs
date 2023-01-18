using ZTP.Fascade;
using ZTP.Models;

namespace ZTP.Controllers
{
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly ZTPDbContext dbContext;

        public DatabaseConnection(ZTPDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public UserWord FindUserWord(int userId, int wordId)
        {
            UserWord userWord = this.dbContext.UserWords.Where(x => x.UserId == userId && x.WordId == wordId).FirstOrDefault();

            return userWord;
        }

        public User GetUser(int id)
        {
            User user = this.dbContext.Users.Where(x => x.Id == id).FirstOrDefault();

            return user;
        }

        public List<int> GetUserWordId(List<int> userWordIds)
        {
            throw new NotImplementedException();
        }

        public List<Word> GetWords()
        {
            var list = this.dbContext.Words.ToList();

            return list;
        }


        public Word GetWord(int id)
        {
            var word = this.dbContext.Words.FirstOrDefault(x => x.Id == id);

            return word;
        }

        public void RemoveWord(Word word)
        {
            this.dbContext.Words.Remove(word);
            this.dbContext.SaveChanges();
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

        public void AddWord(Word word)
        {
            this.dbContext.Words.Add(word);
            this.dbContext.SaveChanges();
        }
    }
}