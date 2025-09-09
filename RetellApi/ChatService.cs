using System;
using System.Threading.Tasks;
using Flurl.Http;
using RetellApi.Models;

namespace RetellApi
{
    /// <summary>
    /// Chat service
    /// </summary>
    public class ChatService
    {
        /// <summary>
        /// Base url for all Retell api calls
        /// </summary>
        private const string BaseUrl = "https://api.retellai.com";

        /// <summary>
        /// constructor, pass in the api key
        /// </summary>
        /// <param name="apiKey"></param>
        public ChatService(string apiKey)
        {
            ApiKey = apiKey;
        }
        /// <summary>
        /// Api key
        /// </summary>
        private string ApiKey { get; }
        /// <summary>
        /// Chat id
        /// </summary>
        private string ChatId { get; set; } = "";
        /// <summary>
        /// Base request for all calls
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        private IFlurlRequest BaseRequest(string relativePath)
        {
            return $"{BaseUrl}/{relativePath}"
                .WithHeader("Authorization", $"Bearer {ApiKey}")
                .WithHeader("Content-Type", "application/json");
        }
        /// <summary>
        /// Does the chat exist? If not, clear the ChatId property.
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        private async Task<bool> ChatExists(string chatId)
        {
            if (string.IsNullOrEmpty(chatId))
            {
                return false;
            }

            try
            {
                var result = await GetChat();
                return true;
            }
            catch
            {
                ChatId = "";
                return false;
            }
        }
        /// <summary>
        /// Start chat with agent. If chatId is provided, will try to continue that chat. (good for testing)
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="chatId"></param>
        /// <returns></returns>
        public async Task<string> StartChatAsync(string agentId, string chatId = "")
        {
            ChatId = chatId;
            if (!string.IsNullOrEmpty(chatId) && await ChatExists(chatId))
            {
                return chatId;
            }

            var payload = new CreateChatRequest
            {
                agent_id = agentId
            };

            try
            {
                var response = await BaseRequest("create-chat")
                    .PostJsonAsync(payload)
                    .ReceiveJson<CreateChatResponse>();
                ChatId = response.chat_id; // Store the chat_id for future use
                return response.chat_id;
            }

            catch (FlurlHttpException ex)
            {
                var error = await ex.GetResponseStringAsync();
                Console.WriteLine($"Error: {error}");
                throw;
            }
        }
        /// <summary>
        /// Send chat message to agent
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<CreateChatCompletionResponse> CreateChatCompletion(string content)
        {
            var payload = new CreateChatCompletionRequest
            {
                chat_id = ChatId,
                content = content
            };

            var response = await BaseRequest("create-chat-completion")
                .PostJsonAsync(payload)
                .ReceiveJson<CreateChatCompletionResponse>();
            return response;
        }
        /// <summary>
        /// Get chat details
        /// </summary>
        /// <returns></returns>
        public async Task<GetChatResponse> GetChat()
        {
            Console.WriteLine("id " + ChatId);
            var response = await BaseRequest($"get-chat/{ChatId}")
                .GetJsonAsync<GetChatResponse>();
            return response;
        }
    }
}