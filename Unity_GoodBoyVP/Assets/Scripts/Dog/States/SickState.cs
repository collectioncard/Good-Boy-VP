using UnityEngine;
using UnityHFSM;

namespace Dog.States
{
    public class SickState : StateBase
    {
        private DogState dogState;

        public SickState(DogState state) : base(needsExitTime: false, isGhostState: false)
        {
            this.dogState = state;
        }

        public override void OnEnter()
        {
            Debug.Log("Dog is now sick.");
        }

        public override void OnLogic()
        {

            dogState.Health -= Time.deltaTime * 5;  // Decrease health faster when sick

            // Maybe prevent the dog from playing
            if (dogState.Health <= 0)
            {
                // Death
            }
        }

        public override void OnExit()
        {
            Debug.Log("Dog is no longer sick.");
            // Logic for when the dog recovers from sickness
        }
    }
}