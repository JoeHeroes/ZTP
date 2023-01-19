using ZTP.Models;

namespace ZTP.Fascade
{
    public interface IDatabaseConnection
    {
        /// UserWord
        void RemoveUserWord(UserWord userWord);
        void UpdateUserWord(UserWord userWord);
        UserWord FindUserWord(int userId, int wordId);
        List<int> FindUserWordInts(int userId);
        void AddUserWord(UserWord userWord);


        /// User
        void AddUser(User user);
        void UpdateUser(User user);
        User GetUserById(int id);
        User GetUserByEmail(string email);


        /// Word
        Word GetWord(int id);
        public List<Word> GetWords();
        bool AnyWord(int id);
        void AddWord(Word word);
        void RemoveWord(Word word);
        void UpdateWord(Word word);
        void SaveChanges();
      
    }
}
