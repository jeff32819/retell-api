using Newtonsoft.Json;
using RetellApi;
using RetellApi.Models;

namespace ConsoleApp1;

public class Tests
{
    public Tests(string apiKey)
    {
        Svc = new ChatService(apiKey, "agent_cf8e9c51ac5ece9754fbe6ef4e");
    }

    public ChatService Svc { get; set; }

    public async Task<SendMessageResponse> Query(string chatId, string text)
    {
        var sendMessage = await Svc.Query(chatId, text);
        if (sendMessage == null)
        {
            throw new Exception("result was null");
        }

        await File.WriteAllTextAsync("t:\\retell_query.txt", JsonConvert.SerializeObject(sendMessage, (Formatting)System.Xml.Formatting.Indented));
        return sendMessage;
    }

    public async Task<ChatLookupResponse> Lookup(string chatId)
    {
        var result = await Svc.Lookup(chatId);
        Console.WriteLine($"start timestamp: {result.start_timestamp} (successfully parsed)");
        await File.WriteAllTextAsync("t:\\retell_chatDetails.txt", JsonConvert.SerializeObject(result, (Formatting)System.Xml.Formatting.Indented));
        foreach (var item in result.message_with_tool_calls)
        {
            Console.WriteLine($"{item.role} :: {item.content}");
        }

        return result;
    }
}