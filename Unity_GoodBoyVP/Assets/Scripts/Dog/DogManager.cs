using Dog.States;
using UnityEngine;
using UnityHFSM;
using System;
using TMPro;

/* 
DOG MANAGER CLASS PURPOSE: manage the overall logic for the dog
-Create instance of StateMachine
-Initialize states 
-Update FSM logic (Update())
*/

public class DogManager : MonoBehaviour
{
    public TextMeshProUGUI statsText;
    private StateMachine fsm;
    private DogState dogState;

    private DateTime startTime;

    // Use a single Random instance for the entire class to avoid reinitializing every time
    private System.Random random = new System.Random();
    public UIManager uiManager; // Reference to the UIManager to handle UI updates

    void Start()
    {
        // Try to find the UIManager if it hasn't been assigned in the Inspector
        if (uiManager == null)
        {
            uiManager = FindObjectOfType<UIManager>();
            if (uiManager == null)
            {
                Debug.LogError("UIManager could not be found in the scene.");
            }
        }

        dogState = new DogState();
        fsm = new StateMachine();

        var idleState = new IdleState(dogState, this);
        var hungryState = new HungryState(dogState, this);
        var sickState = new SickState(dogState, this);
        var sleepyState = new SleepyState(dogState, this);

        fsm.AddState("Idle", idleState);
        fsm.AddState("Hungry", hungryState);
        fsm.AddState("Sick", sickState);
        fsm.AddState("Sleepy", sleepyState);

        fsm.SetStartState("Idle");

        //Please add any new transitions to the list down below too. Thanks!
        fsm.AddTransition("Idle", "Hungry", t => dogState.HungerLevel >= 75);
        fsm.AddTransition("Idle", "Sleepy", t => dogState.IsSleeping == true);
        fsm.AddTransition("Idle", "Sick", t => dogState.IsSick == true);

        fsm.AddTransition("Hungry", "Idle", t => dogState.HungerLevel <= 10);
        fsm.AddTransition("Hungry", "Sleepy", t => dogState.TiredLevel >= 80);
        fsm.AddTransition("Hungry", "Sick", t => dogState.IsSick == true);

        fsm.AddTransition("Sick", "Idle", t => dogState.IsSick == false);
        fsm.AddTransition("Sick", "Sleepy", t => dogState.IsSleeping == true);

        fsm.AddTransition("Sleepy", "Idle", t => dogState.IsSleeping == false);

        fsm.Init();

        // Initialize the time tracking to the current time
        startTime = DateTime.Now;
    }

    void Update()
    {
        fsm.OnLogic();
        // Calculate the time difference since the game started
        double elapsedTime = (DateTime.Now - startTime).TotalSeconds;

        
        // Check if the elapsed time is a multiple of 3 for Hunger
        if (Math.Floor(elapsedTime / 3) > Math.Floor((elapsedTime - Time.deltaTime) / 3)) {
            HungerUpdate();
        }

        // Check if the time is a multiple of 20, the dog has a chance of getting sick
        if (Math.Floor(elapsedTime / 10) > Math.Floor((elapsedTime - Time.deltaTime) / 10))
        {
            SickUpdate();
        }

        // Check if the elapsed time is a multiple of 10 for TiredLevel
        if (Math.Floor(elapsedTime / 5) > Math.Floor((elapsedTime - Time.deltaTime) / 5))
        {
            SleepinessUpdate();
        }

        if (Math.Floor(elapsedTime / 7) > Math.Floor((elapsedTime - Time.deltaTime) / 7))
        {
            dogState.Happiness -= 1;
        }
        
        /*

        // Check if the elapsed time is a multiple of 15 for Happiness

        //Debug.Log("Dog's happiness decreased: " + dogState.Happiness); }
        */

        // Update the text of the dog's current stats
        if (statsText != null)
        {
            statsText.text =
                "Health: " + dogState.Health + "\n" +
                "Hunger: " + dogState.HungerLevel + "\n" +
                "Happiness: " + dogState.Happiness + "\n" +
                "Tired: " + dogState.TiredLevel + "\n" +
                "Sick Chance: " + dogState.SickChance + "%\n" +
                "isSick: " + dogState.IsSick + "\n" +
                "isSleeping: " + dogState.IsSleeping;
        }

        uiManager.UpdateStatBars(dogState);

    }

    // Method to change the sprite via UIManager
    public void ChangeDogSprite(Sprite newSprite)
    {
        if (uiManager != null)
        {
            uiManager.ChangeToNewImage(newSprite);
        }
        else
        {
            Debug.LogWarning("UIManager is not assigned in DogManager.");
        }
    }

    private void CheckIfDogGetsSick()
    {
        System.Random random = new System.Random();
        int roll = random.Next(1, 101); // Roll between 1 and 100
        if (roll <= dogState.SickChance)
        {
            dogState.IsSick = true;
            dogState.SickChance = 100;
        }
        //Debug.Log("Dog's chance of being Sick: " + roll + " <= " + dogState.SickChance);
    }

    private void CheckIfDogWakesUp()
    {
        // Guaranteed wake-up when TiredLevel reaches 0
        if (dogState.TiredLevel == 0)
        {
            dogState.IsSleeping = false;
        }
        // Random wake-up chance when TiredLevel is 10 or less
        else if (dogState.TiredLevel <= 10 && dogState.IsSleeping)
        {
            int wakeRoll = random.Next(1, 11); // 1 in 10 chance
            if (wakeRoll == 1)
            {
                dogState.IsSleeping = false;
            }
        }
    }

    private void CheckIfDogSleeps()
    {
        // Guaranteed sleep when TiredLevel reaches 100
        if (dogState.TiredLevel == 100)
        {
            dogState.IsSleeping = true;
        }
        // Random sleep chance when TiredLevel is 80 or more
        else if (dogState.TiredLevel >= 80 && !dogState.IsSleeping)
        {
            int sleepRoll = random.Next(1, 6); // 1 in 5 chance
            if (sleepRoll <= 20)
            {
                dogState.IsSleeping = true;
            }
        }
    }

    private void SickUpdate()
    {
        if (!dogState.IsSick)
        {
            dogState.SickChance += 1;

            if (dogState.HungerLevel >= 50)
            {
                dogState.SickChance += 3;
            }

            if (dogState.Happiness <= 50)
            {
                dogState.SickChance += 3;
            }

            if (dogState.SickChance > 100)
            {
                dogState.SickChance = 100;
            }

            CheckIfDogGetsSick();

            // Only increase health if the dog is not sick and not too hungry
            if (dogState.Health < 100 && dogState.HungerLevel <= 60)
            {
                dogState.Health += 1;
            }
        }
        else
        {
            // If the dog is sick, reduce its health
            dogState.Health -= 3;
            if (dogState.Health < 0)
            {
                dogState.Health = 0;
            }
        }
    }


    private void HungerUpdate()
    {
        dogState.HungerLevel += 1;
        //Debug.Log("Dog's hunger increased: " + dogState.HungerLevel);
        if (dogState.HungerLevel < 0)
        {
            dogState.HungerLevel = 0;
        }

        if (dogState.HungerLevel > 100)
        {
            dogState.HungerLevel = 100;
        }
    }

    private void SleepinessUpdate()
    {
        if (dogState.IsSleeping == true)
        {
            dogState.TiredLevel -= 5;
            if (dogState.TiredLevel < 0)
            {
                dogState.TiredLevel = 0;
            }

            CheckIfDogWakesUp();
        }
        else
        {
            dogState.TiredLevel += 1;
            if (dogState.TiredLevel > 100)
            {
                dogState.TiredLevel = 100;
            }

            CheckIfDogSleeps();
        }
        //Debug.Log("Dog's TiredLevel decreased: " + dogState.TiredLevel);
    }


    /**
     * Returns the instance of the DogState object that the state machine uses to make decisions.
     * Any modifications to this will change logic so be careful.
     */
    public DogState getDogState()
    {
        return dogState;
    }

    public string getDogStateName()
    {
        return fsm.ActiveState.name;
    }

    /**
     * Returns a string array of all valid transitions from the current state.
     *
     * This is made manually because there doesn't seem to be a way to get the transitions from the state machine. (Kinda annoying ngl)
     */
    public string getValidTransitions()
    {
        //Get the current state from the machine
        string currentState = fsm.ActiveState.name;

        return currentState switch
        {
            "Idle" => "Hungry, Sleepy, Sick",
            "Hungry" => "Idle, Sleepy, Sick",
            "Sick" => "Idle, Sleepy",
            "Sleepy" => "Idle",
            _ => "Idle"
        };
    }

    public void ChangeDogState(string newStateName)
    {
        try
        {
            fsm.RequestStateChange(newStateName);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Tried to transition into an unknown state: " + e);
        }
        
    }

    
}
