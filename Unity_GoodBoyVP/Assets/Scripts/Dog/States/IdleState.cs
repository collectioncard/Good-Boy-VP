using UnityHFSM;

public class IdleState : StateBase
{
    private DogState dogState;

    public IdleState(DogState state)
    {
        this.dogState = state;
    }

    public override void Enter()
    {
        Debug.Log("Dog is idle.");
    }

    public override void Execute()
    {
        // display idle pic ?
    }

    public override void Exit()
    {
        Debug.Log("Exiting Idle State.");
    }
}
