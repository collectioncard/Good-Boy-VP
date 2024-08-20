using Dog.States;
using UnityEngine;
using UnityHFSM;
/* 
DOG MANAGER CLASS PURPOSE: manage the overall logic for the dog
-Create instance of StateMachine
-Initialize states 
-Update FSM logic (Update())
*/

public class DogManager : MonoBehaviour
{
    private StateMachine fsm;
    private DogState dogState;

    void Start()
    {
        dogState = new DogState();
        fsm = new StateMachine();

        var idleState = new IdleState(dogState);
        var hungryState = new HungryState(dogState);
        var sickState = new SickState(dogState);

        fsm.AddState("Idle", idleState);
        fsm.AddState("Hungry", hungryState);
        fsm.AddState("Sick", sickState);

        fsm.SetStartState("Idle");
        // If hunger level over 50, then dog will enter hungry state
        fsm.AddTransition("Idle", "Hungry", t => dogState.HungerLevel > 50);
        fsm.AddTransition("Hungry", "Idle", t => dogState.HungerLevel <= 50);
        
        fsm.AddTransition("Idle", "Sick", t => dogState.IsSick || dogState.Health < 20);
        fsm.AddTransition("Sick", "Idle", t => !dogState.IsSick && dogState.Health >= 20);

        fsm.Init();
    }

    void Update()
    {
        fsm.OnLogic();
        // Keep track of dogs hunger level over time:
        dogState.HungerLevel += Time.deltaTime;
    }
}
