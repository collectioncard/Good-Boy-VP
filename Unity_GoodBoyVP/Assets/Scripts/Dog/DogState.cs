public class DogState
{
    public float HungerLevel { get; set; }
    public float Health { get; set; }
    public float Happiness { get; set; }
    public float TiredLevel { get; set; }
    public bool IsSleeping { get; set; }
    public float SickChance { get; set; }
    public bool IsSick { get; set; }
    public static int amount = 10;

    public DogState()
    {
        // starting w full specs, can add more or adjust
        HungerLevel = 0f;
        Health = 100f;
        Happiness = 100f;
        TiredLevel = 0f;
        SickChance = 0f;
        IsSick = false;
        IsSleeping = false;
    }

    // We can have a set amount of food it gives every time or it can be up to the player
    public void Feed()
    {
        HungerLevel -= amount;
        if (HungerLevel < 0) HungerLevel = 0;
    }

    public void Play()
    {
        Happiness += amount;
        // max happiness
        if (Happiness > 100) Happiness = 100;
    }

    public void giveMedicine()
    {
        Health += amount;
        // Can player even give medicine if the dog isnt sick?
        if (Health > 100) Health = 100;
        IsSick = false;
        SickChance= 0;
    }

}