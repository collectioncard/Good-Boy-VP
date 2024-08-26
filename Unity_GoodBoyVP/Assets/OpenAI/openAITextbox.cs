using OpenAI.ApiTemplates.Response;
using TMPro;
using UnityEngine;

namespace OpenAI
{
    public class openAITextbox : MonoBehaviour
    {
        public TMP_InputField inputField;
        private OpenAIClient openAPI; 
        public DogManager dogManager;
        

        private void Start()
        {
            openAPI = GameObject.Find("OpenAIClient").GetComponent<OpenAIClient>();
            
            inputField.onEndEdit.AddListener(OnEndEdit);
            dogManager = GameObject.Find("HFSMObject")?.GetComponent<DogManager>();
        }

        async void OnEndEdit(string userInput)
        {
            if (!string.IsNullOrEmpty(userInput))
            {
                StructuredOutput apiResponse = await openAPI.handleUserInput(userInput);
                
                //Print the dog's reaction
                inputField.text = apiResponse.DogActionDescription;
                
                //TODO: figure out why the ai sends multiple states. Just use the first one
                DogValue dogValue = apiResponse.DogValues;
                DogState state = dogManager.getDogState();
                
                
                state.Happiness = dogValue.HappinessPercentage;
                state.Health = dogValue.HealthPercentage;
                state.HungerLevel = dogValue.HungerPercentage;
                state.SickChance = dogValue.SickChancePercentage;
                state.TiredLevel = dogValue.TiredLevelPercentage;  
                state.IsSleeping = dogValue.IsSleeping;
                state.IsSick = dogValue.IsSick;
                
                //transition to a new state
                dogManager.ChangeDogState(apiResponse.StateToTransition);

            }
        }
    }
}