using System.Collections.Generic;

using static RetellApi.Models.CreateChatCompletionResponse;

namespace RetellApi.Models
{
    public class SendMessageResponse
    {
        public List<Message> messages { get; set; }
        public string chatId { get; set; }
    }
}