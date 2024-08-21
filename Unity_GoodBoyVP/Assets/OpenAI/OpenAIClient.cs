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

   public async Task<ActionResponseTemplate> handleUserInput(string input, string outputType)
   {
      Message systemMessage = createMessage("system",
         "You are a virtual pet. You will receive the dog state and you are to determine how to continue based on the state and the user prompt. Respond only with the new dog state.");

      Message userMessage = createMessage("user", input);

      ActionRequestTemplate requestBody = new ActionRequestTemplate
      {
         messages = new List<Message> { systemMessage, userMessage }
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
   
   
}
