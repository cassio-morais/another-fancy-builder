namespace DotnetStateBuilder.StateBuilder.Extensions
{
    public abstract class ListState<Item> : IState
        where Item : ItemState, new()
    {
        public List<Item> Items { get; protected set; }

        public ListState()
        {
            Items = new List<Item>();
        }

        public virtual List<Item> GetItemsInProcess()
            => Items.Where(x => x.Status == Status.Processing).ToList() ?? new List<Item>();

        public virtual List<Item> GetProcessedItems()
            => Items.Where(x => x.Status == Status.Processed).ToList() ?? new List<Item>();

        public virtual List<Item> GetUnProcessedItems()
            => Items.Where(x => x.Status == Status.UnProcessed).ToList() ?? new List<Item>();

        public virtual bool HasNoItemsToProcess()
            => Items.Any(x => x.Status == Status.Processing) == false;

        public virtual bool IsInconsistent()
            => Items.Any(x => x.IsInconsistent());
    }
}