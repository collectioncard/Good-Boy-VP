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

      private string sysprompt = "You are a virtual pet dog. The next message will contain the status of the dog, the current overall dog state, and a list of all states that it is possible to transition to within an HFSM (Hierarchical Finite State Machine). The message following that will contain the user input, which is how the user wants to interact with the dog. Your job is to: 1. Provide a short description of how the dog reacts to the user input. This MUST directly reflect the specific user action. For example, if the user message is 'give the dog food', your description could be 'The dog eagerly eats the food, wagging its tail in excitement'. 2. Update the dog's status values based on the interaction. For instance, feeding the dog would decrease its hunger level. 3. Select the appropriate next state for the dog based on the interaction and the list of valid state transitions.";
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
