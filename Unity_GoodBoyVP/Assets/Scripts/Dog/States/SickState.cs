using UnityEngine;
using UnityHFSM;

namespace Dog.States
{
    public class SickState : StateBase {
        private DogState dogState;
        private DogManager dogManager;  // Reference to DogManager for sprite and message changes

        private Sprite[] dogSprites;

        public SickState(DogState state, DogManager manager) : base(needsExitTime: false, isGhostState: false) {
            this.dogState = state;
            this.dogManager = manager;

            // Load all sprites from the "NeutralImages" folder under Resources
            dogSprites = Resources.LoadAll<Sprite>("Dog Images/Tired");
        }

        public override void OnEnter() {
            Debug.Log("Dog is Sick.");

            // Change the sprite to a random one when entering Sick state
            if (dogSprites.Length > 0) {
                int randomIndex = Random.Range(0, dogSprites.Length);
                Sprite randomSprite = dogSprites[randomIndex];
                dogManager.ChangeDogSprite(randomSprite);
            }
            else {
                Debug.LogWarning("No tired sprites found.");
            }
        }

        public override void OnLogic()
        {
            // display idle pic ?
            
        }

        public override void OnExit()
        {
            Debug.Log("Exiting Sick State.");
        }
    }
}