using Newtonsoft.Json;
using OpenAI.ApiTemplates.Response;
using TMPro;
using UnityEngine;

namespace OpenAI
{
    public class openAITextbox : MonoBehaviour
    {
        public TMP_InputField inputField;
        private OpenAIClient openAPI; 
        

        private void Start()
        {
            openAPI = GameObject.Find("OpenAIClient").GetComponent<OpenAIClient>();
            
            inputField.onEndEdit.AddListener(OnEndEdit);
        }

        async void OnEndEdit(string userInput)
        {
            if (!string.IsNullOrEmpty(userInput))
            {
                StructuredOutput apiResponse = await openAPI.handleUserInput(userInput, "text");
                
                //TODO: Change the dog's state
                inputField.text = apiResponse.DogActionDescription;
            }
        }
    }
}