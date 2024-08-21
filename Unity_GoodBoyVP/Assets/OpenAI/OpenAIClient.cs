using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OpenAI.ApiTemplates.Requests;
using OpenAI.ApiTemplates.Response;
using UnityEngine;
using Message = OpenAI.ApiTemplates.Requests.Message;

public class OpenAIClient : MonoBehaviour
{
   private static readonly HttpClient client = new HttpClient();
   private string openAiKey = "Put the API key here. Don't push it to the repo or else!";

   private DogManager dogManager;


   void Start()
   {
      dogManager = GameObject.Find("HFSMObject")?.GetComponent<DogManager>();
      if (dogManager == null)
      {
         Debug.LogError("DogManager component not found on HFSMObject.");
      }
   }

   public async Task<ActionResponseTemplate> handleUserInput(string input, string outputType)
   {
      Message systemMessage = createMessage("system",
         "You are a virtual pet dog. A user can interact with you by typing commands. Your job is to take the dog's status, current state, valid state transitions, and user input, and respond accordingly. If the user asks for something or gives a command that makes no sense, respond with 'the dog looks at you with confusion' but do so sparingly and always try to accommodate a request. Otherwise, respond with an appropriate message along with a state change from the valid list if desired.");

      Message statusMessage = getDogStatusMessage(dogManager);

      Message userMessage = createMessage("user", input);

      ActionRequestTemplate requestBody = new ActionRequestTemplate
      {
         messages = new List<Message> { systemMessage, statusMessage, userMessage }
      };

      requestBody.response_format.type = outputType;
      
      

      string jsonBody = JsonUtility.ToJson(requestBody);
      
      client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", openAiKey);

      var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
      Debug.Log(content);
      var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);
      var responseString = await response.Content.ReadAsStringAsync();
      Debug.Log(responseString);

      //convert the response back into an object
      ActionResponseTemplate apiResponse = new ActionResponseTemplate();

      apiResponse = JsonUtility.FromJson<ActionResponseTemplate>(responseString);
      return apiResponse;
   }

   private static Message createMessage(string role, string text)
   {
      Message message = new Message
      {
         role = role,
         content = new List<Content>
         {
            new Content()
            {
               type = "text",
               text = text
            }
         }
      };
      return message;
   }

   private static Message getDogStatusMessage(DogManager dogManager)
   {
      DogState dogState = dogManager.getDogState();

      StringBuilder stateString = new StringBuilder();

      stateString.Append("Dog Hunger Level:").Append(dogState.HungerLevel)
         .Append("\nDog Health Level:").Append(dogState.Health)
         .Append("\nDog Happiness Level:").Append(dogState.Happiness)
         .Append("\nDog Is Currently Sick: ").Append(dogState.IsSick)
         .Append("\nValid state transitions:").Append(dogManager.getValidTransitions());

      return createMessage("system", stateString.ToString());
   }
   
   
}
