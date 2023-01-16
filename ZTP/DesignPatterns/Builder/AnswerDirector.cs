namespace ZTP.DesignPatterns.Builder
{
    public class AnswerDirector
    {
        public void Construct(IAnswerBuilder builder)
        {
            builder.GetResult();
        }
    }
}