using ZTP.Models.Enum;

namespace ZTP.Models.ModelView
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public Word CorrectWord { get; set; }
        public List<Word> Answers { get; set; }
        public int QuestionNumber { get; set; }
    }
}