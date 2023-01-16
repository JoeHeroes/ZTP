using System.ComponentModel.DataAnnotations.Schema;

namespace ZTP.Models
{
    public class UserWord
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        public int WordId { get; set; }

        [ForeignKey("WordId")]
        public Word Word { get; set; }

        public bool IsLearned { get; set; }
    }
}