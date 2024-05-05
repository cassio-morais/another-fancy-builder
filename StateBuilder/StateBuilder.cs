namespace DotnetStateBuilder
{
    public class StateBuilder<State> where State : class, IState, new()
    {
        public static StateBuilder<State> Initialize(State initialState = null)
        {
            var builder = new StateBuilder<State>();
            builder.SetInitialState(initialState ?? new State());
            builder.SetStepsQueue();

            return builder;
        }

        public virtual StateBuilder<State> AddStep(IStep<State> step)
        {
            if (_stepsQueue == null)
                SetStepsQueue();

            _stepsQueue.Enqueue(step);

            return this;
        }

        public virtual async Task<StateBuilder<State>> RunAsync() 
        {
            while (_stepsQueue.TryDequeue(out IStep<State> processingStep))
            {
                await processingStep.ExecuteAsync(_processingState);
            }

            return this;
        }

        private State _processingState;
        public State ProcessingState 
        {
            get { return _processingState; }
            private set { _processingState = value; }
        }

        private Queue<IStep<State>> _stepsQueue;
        private void SetInitialState(State state) => _processingState = state;
        private void SetStepsQueue() => _stepsQueue = new Queue<IStep<State>>();
    }
}