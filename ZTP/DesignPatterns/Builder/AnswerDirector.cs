namespace ZTP.DesignPatterns.Builder
{
    public class AnswerDirector
    {
        public void Construct(AnswerBuilder builder)
        {
            builder.GetResult();
        }
    }
}