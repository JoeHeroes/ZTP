using ZTP.Models.ModelView;

namespace ZTP.DesignPatterns.Iterator
{
    public interface IIterator
    {
        public QuestionViewModel First();

        public QuestionViewModel Next();

        public bool IsDone();

        public QuestionViewModel CurrentItem();
    }
}