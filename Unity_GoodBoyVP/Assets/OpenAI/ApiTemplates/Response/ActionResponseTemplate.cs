using System.Collections.Generic;
using UnityEngine;

namespace OpenAI.ApiTemplates.Response
{
    [System.Serializable]
    public class Choice
    {
        public int index;
        public Message message;
        public object logprobs;
        public string finish_reason;
    }

    [System.Serializable]
    public class Message
    {
        public string role;
        public string content;
        public object refusal;
    }

    [System.Serializable]
    public class ActionResponseTemplate
    {
        public string id;
        public string @object;
        public int created;
        public string model;
        public List<Choice> choices;
        public Usage usage;
        public string system_fingerprint;
    }

    [System.Serializable]
    public class Usage
    {
        public int prompt_tokens;
        public int completion_tokens;
        public int total_tokens;
    }
}