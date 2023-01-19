using ZTP.Models;

namespace ZTP.Fascade
{
    public class DatabaseConnection : IDatabaseConnection
    {
        private ZTPDbContext dbContext;

        public DatabaseConnection(ZTPDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        

        public void RemoveUserWord(UserWord userWord)
        {
            this.dbContext.UserWords.Remove(userWord);
            this.dbContext.SaveChanges();
        }

        public void UpdateUserWord(UserWord userWord)
        {
            this.dbContext.UserWords.Update(userWord);
            this.dbContext.SaveChanges();
        }
        public UserWord FindUserWord(int userId, int wordId)
        {
            UserWord userWord = this.dbContext.UserWords.FirstOrDefault(x => x.UserId == userId && x.WordId == wordId);

            return userWord;
        }



        public Word FindWordAswer(List<int> userWordsIds)
        {
            Word word = this.dbContext.Words.Where(x => !userWordsIds.Contains(x.Id)).FirstOrDefault();

            return word;
        }


      

        public List<int> FindUserWordInts (int userId)
        {


            List<int> ints = this.dbContext.UserWords.Where(x => x.UserId == userId && !x.IsLearned).Select(x => x.Id).ToList();
            
            return ints;
        }

        public void AddUserWord(UserWord userWord)
        {
            this.dbContext.UserWords.Add(userWord);
            this.dbContext.SaveChanges();
        }

        public void AddUser(User user)
        {
            this.dbContext.Users.Add(user);
            this.dbContext.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            this.dbContext.Users.Update(user);
            this.dbContext.SaveChanges();
        }

        public User GetUserById(int id)
        {
            User user = this.dbContext.Users.FirstOrDefault(x => x.Id == id);

            return user;
        }

        public User GetUserByEmail(string email)
        {
            User user = this.dbContext.Users.FirstOrDefault(u => u.Email == email);

            return user;
        }


        public Word GetWord(int id)
        {
            var word = this.dbContext.Words.FirstOrDefault(x => x.Id == id);

            return word;
        }

        public List<Word> GetWords()
        {
            var list = this.dbContext.Words.ToList();

            return list;
        }

        public bool AnyWord(int id)
        {
            var any = this.dbContext.Words.Any(e => e.Id == id);
            return any;
        }

        public void AddWord(Word word)
        {
            this.dbContext.Words.Add(word);
            this.dbContext.SaveChanges();
        }

        public void RemoveWord(Word word)
        {
            this.dbContext.Words.Remove(word);
            this.dbContext.SaveChanges();
        }

        public void UpdateWord(Word word)
        {
            this.dbContext.Words.Update(word);
            this.dbContext.SaveChanges();
        }

        public void SaveChanges()
        {
            this.dbContext.SaveChanges();
        }

    }
}