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
                ActionResponseTemplate apiResponse = await openAPI.handleUserInput(userInput, "text");
                string aiResult = apiResponse.choices[0].message.content;
                Debug.Log("AI was sent: " + userInput + "\nAI returned: " + aiResult);
                
                //TODO: Just replace the text box content with the response for now. This should eventually go to the output box.
                inputField.text = aiResult;
            }
        }
    }
}