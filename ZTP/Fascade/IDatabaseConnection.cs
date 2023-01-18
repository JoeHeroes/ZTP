using ZTP.Models;

namespace ZTP.Fascade
{
    public interface IDatabaseConnection
    {
        UserWord FindUserWord(int userId, int wordId);
        void SaveChanges();
        void AddWord(Word word);
        void RemoveWord(Word word);
        User GetUser(int id);
    }
}
