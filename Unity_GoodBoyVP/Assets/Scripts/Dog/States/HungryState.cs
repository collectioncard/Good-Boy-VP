using UnityEngine;
using UnityHFSM;

namespace Dog.States
{
    public class HungryState : StateBase
    {
        public HungryState(DogState state) : base(needsExitTime: false, isGhostState: false)
        {
            
        }

        public override void OnEnter()
        {
            Debug.Log("Dog is hungry.");
        }

        public override void OnLogic()
        {
            Debug.Log("The state is supposed to be doing something right now.");
        }

        public override void OnExit()
        {
            Debug.Log("Exiting Hungry State.");
        }
    }
}