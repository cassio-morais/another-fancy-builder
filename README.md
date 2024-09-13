# Another fancy builder

It seems like a builder pattern, a state pattern, or a little chain of responsibility pattern... by the way, for me, it seems like a 'state builder'. It's a builder with a chain of classes and a method that operates over a state. Those classes are enqueued and then dequeued when it is executed.

When I made this solution, I thought about operations step by step in which operation is tested separately and it is possible to increment operation over and over again... and after the final execution, I can test if a state is consistent.

Improve testability and cohesion when I have operations with multiple single steps. Separate steps with its business concern, test them individually, and increment as needed.

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

Running...

```bash
test@test:~/projects/csharp/DotnetStateBuilder/Samples$ dotnet run

initialState = 0
First Step: AddOneToCurrentValueStep = 1
Second Step: AddRandomNumToCurrentValueStep = -8
State Inconsistent. Current value = -8

test@test~/projects/csharp/DotnetStateBuilder/Samples$ dotnet run

initialState = 0
First Step: AddOneToCurrentValueStep = 1
Second Step: AddRandomNumToCurrentValueStep = 0
State is ok. Current value = 0
```

*Ok, it is a very simple example. But it's just a quick demonstration.* :')

An extension was also developed to operate over lists for incrementing the solution (the reason for its creation). For example, a list of emails may require actions such as resetting passwords, resetting 2FA, and incorporating additional information from another service. Processing states were added to each item in the email list, indicating whether they were unprocessed or processed. The final test checks the processing state of the entire list or individual email processing states.

Imagine the following list:

```json
[
   "somewrongemailformat",
   "valid@emailcom",
   "doesnotexistinourdatabase@email.com"
]
```

There are four steps:

- Verify email format.
- Check if the email exists; if not, create it.
- Reset 2FA credentials if they exist; otherwise, prompt the user to use 2FA.
- Retrieve information about emails from another service.

The constraint here is processing step by step waiting for the previous step to finish and just processing items that are not set to unprocessed. Like a pipeline...

Using this solution, we can incrementally process the emails and remove those with problems from the subsequent steps. This results in something like the following:

```json
[
  { 
    "email" : "somewrongemailformat",
    "status" : "UnProcessed", 
    "error" : "email is not a valid email"
  },
  { 
    "email" : "valid@emailcom",
    "status" : "Processed", 
    "error" : null
  },
    { 
    "email" : "doesnotexistinourdatabase@email.com",
    "status" : "Processed", 
    "error" : null
  }
]
```

disclaimer.: a result of 1 million emails processing is not a thing the user wants to see 'now'... follow the constraints.

[UNDER CONSTRUCTION]
