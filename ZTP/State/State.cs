namespace ZTP.State
{
    public abstract class State
    {
        protected Context context;

        public void SetContext(Context context)
        {
            this.context = context;
        }
        public abstract void SetPoints(float points);
        public abstract float GetPoints();
        public abstract int AnswerQuestion();
    }
}
