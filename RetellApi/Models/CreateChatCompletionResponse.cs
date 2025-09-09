using System.Collections.Generic;

namespace RetellApi.Models
{
    public class CreateChatCompletionResponse
    {
        public List<Message> messages { get; set; }

        public class Message
        {
            public string message_id { get; set; }
            public string role { get; set; }
            public string content { get; set; }
            public long created_timestamp { get; set; }
        }
    }
}