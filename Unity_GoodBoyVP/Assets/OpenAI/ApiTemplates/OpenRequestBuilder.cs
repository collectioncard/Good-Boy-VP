using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenAI.ApiTemplates
{
    public class OpenRequestBuilder
    {
        private readonly List<Message> messages = new List<Message>();

        public OpenRequestBuilder addSystemMessage(string messageContent)
        {
            messages.Add(new Message
            {
                role = "system",
                content = new[]
                {
                    new { type = "text", messageContent }
                }
            });

            return this;
        }

        public OpenRequestBuilder addUserMessage(string messageContent)
        {
            messages.Add(new Message
            {
                role = "user",
                content = new[]
                {
                    new { type = "text", messageContent }
                }
            });

            return this;
        }

        public string buildRequest()
        {
            var requestContent = new
            {
                model = "gpt-4o-mini",
                messages = messages,
                temperature = 1,
                max_tokens = 256,
                top_p = 1,
                frequency_penalty = 0,
                presence_penalty = 0,
                response_format = new
                {
                    type = "json_schema",
                    json_schema = new
                    {
                        name = "dogResponseV1",
                        schema = new
                        {
                            type = "object",
                            properties = new
                            {
                                DogActionDescription = new { type = "string" },
                                DogValues = new
                                {
                                    type = "array",
                                    items = new
                                    {
                                        type = "object",
                                        properties = new
                                        {
                                            HungerPercentage = new { type = "integer" },
                                            HealthPercentage = new { type = "integer" },
                                            HappinessPercentage = new { type = "integer" },
                                            isSick = new { type = "boolean" },
                                            isHungry = new { type = "boolean" }
                                        },
                                        required = new[]
                                        {
                                            "HungerPercentage",
                                            "HealthPercentage",
                                            "HappinessPercentage",
                                            "isSick",
                                            "isHungry"
                                        },
                                        additionalProperties = false
                                    }
                                },
                                NewStateName = new { type = "string" }
                            },
                            required = new[]
                            {
                                "DogActionDescription",
                                "DogValues",
                                "NewStateName"
                            },
                            additionalProperties = false
                        },
                        strict = true
                    }
                }
            };
            return JsonConvert.SerializeObject(requestContent);
        }
    }

    public class Message
    {
        public string role { get; set; }
        public object[] content { get; set; }
    }
}