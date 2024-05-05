namespace DotnetStateBuilder.StateBuilder.Extensions
{
    public abstract class ItemState : IState
    {
        public Status Status { get; private set; }

        public string? Error { get; private set; }

        public virtual void SetUnProcessed(string errorMessage)
        {
            this.Error = errorMessage;
            this.Status = Status.UnProcessed;
        }

        public virtual void SetProcessed()
        {
            this.Error = default;
            this.Status = Status.Processed;
        }

        public virtual void SetProcessing()
        {
            this.Error = default;
            this.Status = Status.Processing;
        }

        public bool IsInconsistent() 
            => Status == Status.UnProcessed;
    }
}