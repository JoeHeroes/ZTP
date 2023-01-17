namespace ZTP.State
{
    public class TestingState : State
    {
        public float Points { get; set; }

        public override int AnswerQuestion()
        {
            throw new NotImplementedException();
        }

        public override float GetPoints()
        {
            return Points;
        }

        public override void SetPoints(float points)
        {
            Points = points;
        }
    }
}