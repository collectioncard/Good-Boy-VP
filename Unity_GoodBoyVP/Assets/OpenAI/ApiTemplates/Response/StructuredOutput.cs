namespace OpenAI.ApiTemplates.Response
{
    public class StructuredOutput
    {
        public string DogActionDescription { get; set; }
        public DogValue[] DogValues { get; set; }
        public string NewStateName { get; set; }
    }

    public class DogValue
    {
        public int HappinessPercentage { get; set; }
        public int HealthPercentage { get; set; }
        public int HungerPercentage { get; set; }
        public int SickChancePercentage { get; set; }
        public bool IsSleeping { get; set; }
        public bool IsSick { get; set; }
        public int TiredLevelPercentage { get; set; }
    }
}