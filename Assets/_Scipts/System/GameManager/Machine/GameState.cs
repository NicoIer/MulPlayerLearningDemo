namespace Kitchen
{
    public abstract class GameState
    {
        protected GameStateMachine stateMachine;
        protected GameManager owner => stateMachine.Owner;

        public void SetMachine(GameStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public virtual void Update()
        {
        }

        public virtual void Enter()
        {
        }

        public virtual void Exit()
        {
        }
    }
}