using ConsoleApp1;

using Microsoft.Extensions.Configuration;
// Add this using directive


var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var apiKey = config["ApiKey"];
if (string.IsNullOrEmpty(apiKey))
{
    throw new Exception("apiKey not set");
}

var tests = new Tests(apiKey);

const string previousChatId = "chat_de8fcd4fb21a3f620cec52adad4"; 
//var sendMessage = await tests.Query(previousChatId, "what is your business name");
var result = await tests.Lookup(previousChatId);

Console.WriteLine();
Console.WriteLine("Press any key to exit.");
Console.ReadKey();