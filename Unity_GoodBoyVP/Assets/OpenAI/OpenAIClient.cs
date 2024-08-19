using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OpenAI.ApiTemplates;
using UnityEngine;

public class OpenAIClient : MonoBehaviour
{
   private static readonly HttpClient client = new HttpClient();
   private string openAiKey = "Put the API key here. Don't push it to the repo or else!";

   public async Task<string> handleUserInput(string input)
   {
      Message systemMessage = createMessage("system",
         "You are a virtual pet. You will receive the dog state and you are to determine how to continue based on the state and the user prompt. Respond only with the new dog state.");

      Message userMessage = createMessage("user", input);

      ActionRequestTemplate requestBody = new ActionRequestTemplate
      {
         messages = new List<Message> { systemMessage, userMessage }
      };
      
      

      string jsonBody = JsonUtility.ToJson(requestBody);
      
      client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", openAiKey);

      var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
      var response = await client.PostAsync("https://api.openai.com/v1/completions", content);
      var responseString = await response.Content.ReadAsStringAsync();

      // TODO: Convert this into a json response instead of a string

      return responseString;

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
   
   
}
