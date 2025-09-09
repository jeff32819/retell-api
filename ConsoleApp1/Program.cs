using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RetellApi;
// Add this using directive
using Formatting = Newtonsoft.Json.Formatting;


var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var apiKey = config["ApiKey"];
if (string.IsNullOrEmpty(apiKey))
{
    throw new Exception("apiKey not set");
}

const string previousChatId = "chat_a50be6c0e1e80f09fd36383144c"; // use for continuing a chat

var service = new ChatService(apiKey);
var chatId = await service.StartChatAsync("agent_cf8e9c51ac5ece9754fbe6ef4e", previousChatId);

var chatCompletion = await service.CreateChatCompletion("book a move");
if (chatCompletion == null)
{
    throw new Exception("result was null");
}

File.WriteAllText("t:\\retell.txt", JsonConvert.SerializeObject(chatCompletion, (Formatting)System.Xml.Formatting.Indented));


var chatDetails = await service.GetChat();
foreach (var item in chatDetails.message_with_tool_calls)
{
    Console.WriteLine($"{item.role} :: {item.content}");
}

Console.WriteLine();
Console.WriteLine("Press any key to exit.");
Console.ReadKey();