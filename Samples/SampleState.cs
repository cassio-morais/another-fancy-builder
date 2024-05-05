using DotnetStateBuilder;

namespace Samples;

public class SampleState : IState
{
    public int CurrentValue { get; set; }

    public bool IsInconsistent()
    {
        return CurrentValue < 0;  
    }
}
