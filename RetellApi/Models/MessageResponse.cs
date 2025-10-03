using System.Collections.Generic;

namespace RetellApi.Models
{
    /// <summary>
    /// Message post response
    /// </summary>
    public class MessageResponse
    {
        /// <summary>
        /// is successful?
        /// </summary>
        public bool success { get; set; }
        /// <summary>
        /// Messages returned.
        /// </summary>
        public List<CreateChatCompletionResponse.Message> messages { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier for the chat.
        /// </summary>
        public string chatId { get; set; }
    }
}