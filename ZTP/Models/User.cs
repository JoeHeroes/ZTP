using System.Data;
using ZTP.Models.Enum;

namespace ZTP.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty; 
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public Difficulty Difficulty { get; set; }
        public float Points { get; set; }


        void ChangeDifficulty(Difficulty difficulty)
        {
            this.Difficulty = difficulty;
        }

        void AddPoints(float points)
        {
            this.Points = points;
        }

        Difficulty CheckDifficulty()
        {
            return this.Difficulty;
        }
    }
}
