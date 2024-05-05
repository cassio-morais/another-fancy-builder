using DotnetStateBuilder;

namespace Samples;

public class Program
{
    public static async Task Main(string[] args)
    { 
        // below the state in which SampleStateBuilder operates
        var initialState = new SampleState() { CurrentValue = 0 };
        Console.WriteLine($"initialState = {initialState.CurrentValue}");

        var stateBuilder = await SampleStateBuilder
            .Initialize(initialState)
            .AddStep(new AddOneToCurrentValueStep())
            .AddStep(new AddRandomNumToCurrentValueStep())
            .RunAsync();

        // processingState is the SampleState class
        if(stateBuilder.ProcessingState.IsInconsistent()) 
            Console.WriteLine($"State Inconsistent. Current value = {stateBuilder.ProcessingState.CurrentValue}");
        else 
            Console.WriteLine($"State is ok. Current value = {stateBuilder.ProcessingState.CurrentValue}");
    }
}


class AddOneToCurrentValueStep : IStep<SampleState>
{
    public Task ExecuteAsync(SampleState state)
    {
        state.CurrentValue =  1;
        Console.WriteLine($"First Step: {nameof(AddOneToCurrentValueStep)} = {state.CurrentValue}");

        return Task.CompletedTask;
    }
}

class AddRandomNumToCurrentValueStep : IStep<SampleState>
{
    public Task ExecuteAsync(SampleState state)
    {
        Random rnd = new Random();
        state.CurrentValue = rnd.Next(-10, 10);
        Console.WriteLine($"Second Step: {nameof(AddRandomNumToCurrentValueStep)} = {state.CurrentValue}");

        return Task.CompletedTask;
    }
}

