using Microsoft.Build.Evaluation;

namespace ZTP.State
{
    public class Context
    {
       
        private State State { get; set; }


        public void ChangedState(State state)
        {
            this.State = state;
            this.State.SetContext(this);
        }

        public State CheckState()
        {
            return State;
        }
    }
}
