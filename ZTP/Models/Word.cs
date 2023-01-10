namespace ZTP.Models
{
    public class Word
    {
        public int Id { get; set; }
        public string PolishWord { get; set; } = string.Empty;
        public string ForeignLanguageWord { get; set; } = string.Empty;
    }
}
