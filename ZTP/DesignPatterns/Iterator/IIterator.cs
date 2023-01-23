using ZTP.Models.ModelView;

namespace ZTP.DesignPatterns.Iterator
{
    public interface IIterator               //interfejs dla iteratorów
    {
        public QuestionViewModel First();

        public QuestionViewModel Next();

        public bool IsDone();

        public QuestionViewModel CurrentItem();
    }
}