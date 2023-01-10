using ZTP.Models.Enum;

namespace ZTP.Models.ModelView
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public Word Word { get; set; }
        public Difficulty Difficulty { get; set; }
    }
}
