using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenAI.ApiTemplates;
using OpenAI.ApiTemplates.Response;
using UnityEngine;

namespace OpenAI
{
   public class OpenAIClient : MonoBehaviour
   {
      private static readonly HttpClient client = new HttpClient();
      private DogManager dogManager;

      private string openAiUrl = "https://api.openai.com/v1/chat/completions";
      private string openAiKey = "Put the API key here. Don't push it to the repo or else!";

      private string sysprompt =
         "You are a virtual pet dog. A user can interact with you by typing commands. Your job is to take the dog's status, current state, valid state transitions, and user input, and respond accordingly. If a command is unclear or doesn't make sense, respond with 'the dog looks at you with confusion,' but do so sparingly and try to provide different messages if the dog is in a non-idle state. Always aim to accommodate the user's request using the valid state transitions and update the dog's status accordingly. IMPORTANT: The state to transition to MUST be one of the states provided in the second message. Adjust the dog's status values (e.g., hunger, tiredness, happiness) based on the action performed. If no transition is needed, return 'None'.";
      private void Start()
      {
         dogManager = GameObject.Find("HFSMObject")?.GetComponent<DogManager>();
         if (dogManager == null)
         {
            Debug.LogError("DogManager component not found on HFSMObject.");
         }

         client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", openAiKey);
      }

      public async Task<StructuredOutput> handleUserInput(string userInput)
      {
         OpenRequestBuilder requestBuilder = new OpenRequestBuilder()
            .addSystemMessage(sysprompt)
            .addSystemMessage(getDogStatusString(dogManager))
            .addUserMessage(userInput);

         string requestBody = requestBuilder.buildRequest();
         var encodedRequestBody = new StringContent(requestBody, Encoding.UTF8, "application/json");

         HttpResponseMessage response = await client.PostAsync(openAiUrl, encodedRequestBody);
         string responseString = await response.Content.ReadAsStringAsync();

         //convert the response back into an object
         ActionResponseTemplate apiResponse = new ActionResponseTemplate();

         apiResponse = JsonUtility.FromJson<ActionResponseTemplate>(responseString);
         string messageContent = apiResponse.choices[0].message.content;

         return JsonConvert.DeserializeObject<StructuredOutput>(messageContent);
      }

      private static string getDogStatusString(DogManager dogManager)
      {
         DogState dogState = dogManager.getDogState();

         StringBuilder stateString = new StringBuilder();

         stateString.Append("Dog Hunger Level:").Append(dogState.HungerLevel)
            .Append("\nDog Health Level:").Append(dogState.Health)
            .Append("\nDog Happiness Level:").Append(dogState.Happiness)
            .Append("\nDog Tired Level:").Append(dogState.TiredLevel)
            .Append("\nDog Sick Chance:").Append(dogState.SickChance)
            .Append("\nDog Is Currently Sick: ").Append(dogState.IsSick)
            .Append("\nDog Is Currently Sleeping: ").Append(dogState.IsSleeping)
            .Append("\nCurrent state: ").Append(dogManager.getDogStateName())
            .Append("\nValid state transitions:").Append(dogManager.getValidTransitions());

         return stateString.ToString();
      }

   }
}
