using System;
using System.Collections.Generic;
using UnityEditor.VersionControl;


namespace OpenAI.ApiTemplates
{
    [Serializable]
    public class ActionRequestTemplate
    {
        public readonly string model = "gpt-4o-mini";
        public readonly int temperature = 1;
        public readonly int max_tokens = 256;
        public readonly int top_p = 1;
        public readonly int frequency_penalty = 0;
        public readonly int presence_penalty = 0;
            
        public List<Message> messages;  
            
        public List<Tool> tools;
        public ResponseFormat response_format;
        
    }
    
    public class Content
    {
        public string type;
        public string text;
    }

    public class Function
    {
        public string name;
        public string description;
        public bool strict;
    }

    public class Message
    {
        public string role;
        public List<Content> content;
    }

    public class ResponseFormat
    {
        public string type;
    }
    
    public class Tool
    {
        public string type;
        public Function function;
    }


    
    
}
