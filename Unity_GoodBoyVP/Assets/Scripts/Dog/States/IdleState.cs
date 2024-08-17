using UnityEngine;
using UnityHFSM;

namespace Dog.States
{
    public class IdleState : StateBase
    {
        private DogState dogState;
        
        public IdleState(DogState state) : base(needsExitTime: false, isGhostState: false)
        {
            this.dogState = state;
        }

        public override void OnEnter()
        {
            Debug.Log("Dog is idle.");
        }

        public override void OnLogic()
        {
            // display idle pic ?
        }

        public override void OnExit()
        {
            Debug.Log("Exiting Idle State.");
        }
    }
}