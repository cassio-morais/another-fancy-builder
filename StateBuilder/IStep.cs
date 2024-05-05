namespace DotnetStateBuilder
{
    public interface IStep<State> where State : class, IState, new()
    {
        Task ExecuteAsync(State state);
    }
}