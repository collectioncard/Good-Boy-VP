using System;
using System.Collections.Generic;

namespace OpenAI.ApiTemplates.Requests
{
    [Serializable]
    public class ActionRequestTemplate
    {
        public string model = "gpt-4o-mini";
        public List<Message> messages = new List<Message>();
        public int temperature = 1;
        public int max_tokens = 256;
        public int top_p = 1;
        public int frequency_penalty = 0;
        public int presence_penalty = 0;
        //public List<Tool> tools = new List<Tool>(); TODO: Implement if we want
        public ResponseFormat response_format = new ResponseFormat();
        public bool strict = true;
    }

    [Serializable]
    public class Content
    {
        public string type;
        public string text;
    }

    [Serializable]
    public class Function
    {
        public string name;
        public string description;
        public bool strict;
    }

    [Serializable]
    public class Message
    {
        public string role;
        public List<Content> content = new List<Content>();
    }

    [Serializable]
    public class ResponseFormat
    {
        public string type;
        public Dictionary<string, object> json_schema = new Dictionary<string, object>();
    }

    [Serializable]
    public class Tool
    {
        public string type;
        public Function function = new Function();
    }
}