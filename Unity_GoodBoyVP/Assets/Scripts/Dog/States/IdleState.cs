using UnityEngine;
using UnityHFSM;

namespace Dog.States {
    
    public class IdleState : StateBase {
        private DogState dogState;
        private DogManager dogManager;  // Reference to DogManager for sprite and message changes

        private Sprite[] idleSprites;  // Array to hold all idle sprites

        public IdleState(DogState state, DogManager manager) : base(needsExitTime: false, isGhostState: false) {
            this.dogState = state;
            this.dogManager = manager;

            // Load all sprites from the "NeutralImages" folder under Resources
            idleSprites = Resources.LoadAll<Sprite>("Dog Images/Neutral");
        }

        public override void OnEnter() {
            Debug.Log("Dog is idle.");

            // Change the sprite to a random one when entering Idle state
            if (idleSprites.Length > 0) {
                int randomIndex = Random.Range(0, idleSprites.Length);
                Sprite randomSprite = idleSprites[randomIndex];
                dogManager.ChangeDogSprite(randomSprite);
            }
            else {
                Debug.LogWarning("No idle sprites found.");
            }
        }

        public override void OnLogic() {
        }

        public override void OnExit() {
            Debug.Log("Exiting Idle State.");
        }
    }
}
