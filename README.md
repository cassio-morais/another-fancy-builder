## State builder

I don't know if it seems like a builder pattern, a little bit of a state pattern, or a little chain of responsibility pattern... by the way, for me, it seems like a 'state builder'. It's a builder with a chain of classes with a method that operates over a state. Those classes are enqueued and then dequeued when it is executed.

When I made this solution, I thought about synchronous operations step by step in which operation is tested separately and it is possible to increment operation over and over again... and in the final execution, I can test if a state is consistent.

Improve testability and cohesion when I have long-run synchronous operations. Separate steps with its business concern, test them individually, and increment as needed.

- initial state  = 0
- add step1 => queue step1
- add step2 => queue step2
- run:
  - operator dequeue step1
  - step 1 modifies the state to 1
  - operator dequeue step2
  - step 2 modifies the state to random number...
- finally test the processing state to know if is consistent or inconsistent

and so on...

```c#
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
```

The 2 steps

```c#
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
```

but who knows if a state is consistent or not is the state itself 

```c#
public class SampleState : IState
{
    public int CurrentValue { get; set; }

    public bool IsInconsistent()
    {
        return CurrentValue < 0;  
    }
}
```

Also was made an extension to operate over lists to increment the solution (the reason why this solution was made).

Eg. A list of emails to reset the password, reset 2FA, and add some information from another service.... etc. Add processing state to each item of a list of emails like unprocessed or processed and the final test against the whole processing state or individual email processing state
