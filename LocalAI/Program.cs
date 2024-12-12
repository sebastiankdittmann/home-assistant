using LocalAI.AIFunctions;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var chatClient =
    new ChatClientBuilder(new OllamaChatClient(new Uri("http://localhost:11434/"), config["ModelName"]))
        .UseFunctionInvocation()
        .Build();

var weatherOptions = new WeatherOptions
{
    ApiKey = config["WeatherApiKey"] ?? throw new NullReferenceException("WeatherApiKey is not set")
};
var weatherFunction = new WeatherFunction(new HttpClient(), weatherOptions);

var chatOptions = new ChatOptions
{
   Tools = [AIFunctionFactory.Create(async (string location) =>
   {
       var weather = await weatherFunction.GetWeather(location);

       return weather;
   },
   "get_current_weather",
   "Get the current weather in a given location in metric units")
   ]
};

// Start the conversation with context for the AI model
List<ChatMessage> chatHistory = [new(ChatRole.System, """
                                                      You are a home assistant AI. Treat any measurements as metric if not requested differently.
                                                      """)];

while (true)
{
    // Get user prompt and add to chat history
    Console.WriteLine("Your prompt:");
    var userPrompt = Console.ReadLine();
    chatHistory.Add(new ChatMessage(ChatRole.User, userPrompt));

    // Stream the AI response and add to chat history
    Console.WriteLine("AI Response:");
    var response = await chatClient.CompleteAsync(chatHistory, chatOptions);
    Console.WriteLine(response.Message);

    chatHistory.Add(new ChatMessage(ChatRole.Assistant, response.Message.Text));
    Console.WriteLine();
}