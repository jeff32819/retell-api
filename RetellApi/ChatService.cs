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
        /// <param name="agentId"></param>
        public ChatService(string apiKey, string agentId)
        {
            ApiKey = apiKey;
            AgentId = agentId;
        }
        /// <summary>
        /// Agent id
        /// </summary>
        public string AgentId { get; set; }

        /// <summary>
        /// Api key
        /// </summary>
        private string ApiKey { get; }
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
                var result = await GetChatDetailById(chatId);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Start chat with agent. If chatId is provided, will try to continue that chat. (good for testing)
        /// </summary>
        /// <returns></returns>
        private async Task<string> ChatCreate()
        {
            var payload = new CreateChatRequest
            {
                agent_id = AgentId
            };

            try
            {
                var response = await BaseRequest("create-chat")
                    .PostJsonAsync(payload)
                    .ReceiveJson<CreateChatResponse>();
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
        /// Create a chat if the chatId is blank
        /// </summary>
        /// <param name="chatId"></param>
        /// <returns></returns>
        private async Task<string> CreateChatIdIfBlank(string chatId) => string.IsNullOrEmpty(chatId) ? await ChatCreate() : chatId;
        /// <summary>
        /// Send message
        /// </summary>
        /// <param name="content">content</param>
        /// <param name="chatId">chat id</param>
        /// <returns></returns>
        public async Task<SendMessageResponse> SendMessage(string chatId, string content)
        {
            chatId = await CreateChatIdIfBlank(chatId);
            var result = await CreateChatCompletion(chatId, content);
            if (result != null)
            {
                return new SendMessageResponse
                {
                    chatId = chatId,
                    messages = result.messages
                };
            }
            chatId = await CreateChatIdIfBlank(chatId); // chat id was passed but not found, just create another.
            result = await CreateChatCompletion(content, chatId);
            return new SendMessageResponse
            {
                chatId = chatId,
                messages = result.messages
            };
        }

        /// <summary>
        /// Send chat message to agent
        /// </summary>
        /// <param name="chatId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private async Task<CreateChatCompletionResponse> CreateChatCompletion(string chatId, string content)
        {
            var payload = new CreateChatCompletionRequest
            {
                chat_id = chatId,
                content = content
            };
            try
            {
                var response = await BaseRequest("create-chat-completion")
                    .PostJsonAsync(payload)
                    .ReceiveJson<CreateChatCompletionResponse>();
                return response;
            }
            catch (FlurlHttpTimeoutException tex)
            {
                // Handle timeout specifically
                Console.WriteLine("Request timed out.");
                return null;
            }
            catch (FlurlHttpException ex)
            {
                // Log basic info
                Console.WriteLine($"Request to {ex.Call.Request.Url} failed with status {ex.Call.Response?.StatusCode}");

                // Optionally get raw response
                var raw = await ex.GetResponseStringAsync();
                Console.WriteLine($"Raw response: {raw}");

                // Or deserialize error body
                //var error = await ex.GetResponseJsonAsync<MyErrorResponse>();
                //Console.WriteLine($"Error detail: {error?.Message}");
                return null;
            }

        }
        /// <summary>
        /// Get chat details
        /// </summary>
        /// <returns></returns>
        public async Task<GetChatResponse> GetChatDetailById(string chatId)
        {
            var response = await BaseRequest($"get-chat/{chatId}")
                .GetJsonAsync<GetChatResponse>();
            return response;
        }
    }
}