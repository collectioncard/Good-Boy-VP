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
                content = messageContent
            });

            return this;
        }

        public OpenRequestBuilder addUserMessage(string messageContent)
        {
            messages.Add(new Message
            {
                role = "user",
                content = messageContent
            });

            return this;
        }

        public string buildRequest()
        {
            var requestContent = new
            {
                model = "gpt-4o-mini",
                messages = messages,
                temperature = 1.0,
                max_tokens = 256,
                top_p = 1.0,
                frequency_penalty = 0.0,
                presence_penalty = 0.0,
                response_format = new
                {
                    type = "json_schema",
                    json_schema = new
                    {
                        name = "dogResponseV2",
                        schema = new
                        {
                            type = "object",
                            properties = new
                            {
                                DogActionDescription = new { type = "string" },
                                DogValues = new
                                {
                                    type = "object",
                                    properties = new
                                    {
                                        HappinessPercentage = new { type = "integer" },
                                        HealthPercentage = new { type = "integer" },
                                        HungerPercentage = new { type = "integer" },
                                        SickChancePercentage = new { type = "integer" },
                                        IsSleeping = new { type = "boolean" },
                                        IsSick = new { type = "boolean" },
                                        TiredLevelPercentage = new { type = "integer" }
                                    },
                                    required = new[]
                                    {
                                        "HappinessPercentage",
                                        "HealthPercentage",
                                        "HungerPercentage",
                                        "SickChancePercentage",
                                        "IsSleeping",
                                        "IsSick",
                                        "TiredLevelPercentage"
                                    },
                                    additionalProperties = false
                                },
                                StateToTransition = new { type = "string" }
                            },
                            required = new[]
                            {
                                "DogActionDescription",
                                "DogValues",
                                "StateToTransition"
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
        public string content { get; set; }
    }
}
