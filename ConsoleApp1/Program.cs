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

var service = new ChatService(apiKey, "agent_cf8e9c51ac5ece9754fbe6ef4e");
const string previousChatId = "chat_4cb44d97b121efa77f01b097cca";

//var sendMessage = await service.SendMessage(previousChatId, "what is your business name");
//if (sendMessage == null)
//{
//    throw new Exception("result was null");
//}
//File.WriteAllText("t:\\retell.txt", JsonConvert.SerializeObject(sendMessage, (Formatting)System.Xml.Formatting.Indented));


var chatDetails = await service.GetChatDetailById(previousChatId);
File.WriteAllText("t:\\retell_chatDetails.txt", JsonConvert.SerializeObject(chatDetails, (Formatting)System.Xml.Formatting.Indented));
foreach (var item in chatDetails.message_with_tool_calls)
{
    Console.WriteLine($"{item.role} :: {item.content}");
}

Console.WriteLine();
Console.WriteLine("Press any key to exit.");
Console.ReadKey();