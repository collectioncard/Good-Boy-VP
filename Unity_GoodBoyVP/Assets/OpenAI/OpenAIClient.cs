using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenAI.ApiTemplates;
using OpenAI.ApiTemplates.Requests;
using OpenAI.ApiTemplates.Response;
using UnityEngine;
using Message = OpenAI.ApiTemplates.Requests.Message;

public class OpenAIClient : MonoBehaviour
{
   private static readonly HttpClient client = new HttpClient();
   private DogManager dogManager;
 
   private string openAiKey = "Put the API key here. Don't push it to the repo or else!";
   private string sysprompt = "You are a virtual pet dog. A user can interact with you by typing commands. Your job is to take the dog's status, current state, valid state transitions, and user input, and respond accordingly. If the user asks for something or gives a command that makes no sense, respond with 'the dog looks at you with confusion' but do so sparingly and always try to accommodate a request. Otherwise, respond with an appropriate message along with a state change from the valid list if desired.";

   void Start()
   {
      dogManager = GameObject.Find("HFSMObject")?.GetComponent<DogManager>();
      if (dogManager == null)
      {
         Debug.LogError("DogManager component not found on HFSMObject.");
      }

      client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", openAiKey);
   }

   public async Task<StructuredOutput> handleUserInput(string input, string outputType)
   {
      OpenRequestBuilder requestBuilder = new OpenRequestBuilder()
         .addSystemMessage(sysprompt)
         .addSystemMessage(getDogStatusString(dogManager))
         .addUserMessage(input);

      string requestBody = requestBuilder.buildRequest();
      var encodedRequestBody = new StringContent(requestBody, Encoding.UTF8, "application/json");

      var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", encodedRequestBody);
      var responseString = await response.Content.ReadAsStringAsync();

      //convert the response back into an object
      ActionResponseTemplate apiResponse = new ActionResponseTemplate();

      apiResponse = JsonUtility.FromJson<ActionResponseTemplate>(responseString);

      string messageContent = apiResponse.choices[0].message.content;

      StructuredOutput output = JsonConvert.DeserializeObject<StructuredOutput>(messageContent);

      return output;
   }

   private static string getDogStatusString(DogManager dogManager)
   {
      DogState dogState = dogManager.getDogState();

      StringBuilder stateString = new StringBuilder();

      stateString.Append("Dog Hunger Level:").Append(dogState.HungerLevel)
         .Append("\nDog Health Level:").Append(dogState.Health)
         .Append("\nDog Happiness Level:").Append(dogState.Happiness)
         .Append("\nDog Is Currently Sick: ").Append(dogState.IsSick)
         .Append("\nValid state transitions:").Append(dogManager.getValidTransitions());

      return stateString.ToString();
   }

}
