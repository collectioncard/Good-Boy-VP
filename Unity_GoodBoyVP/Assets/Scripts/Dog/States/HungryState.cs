using UnityHFSM;

public class HungryState : StateBase
{
    private DogState dogState;

    public HungryState(DogState state)
    {
        this.dogState = state;
    }

    public override void Enter()
    {
        Debug.Log("Dog is hungry.");
    }

    public override void Execute()
    {
        // ??
    }

    public override void Exit()
    {
        Debug.Log("Exiting Hungry State.");
    }
}
