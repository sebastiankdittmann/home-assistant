# Local AI chat bot
This is a simple AI chat bot to experiment with the .NET 9 functionality for routing AI requests to specific local implementations.

Current implemented functionality:
* Handle requests about the weather in a specific city

## How to run
The project requires an installation of a chat AI model to run locally on `localhost:11434`. The details can be configured in user secrets:
```json
{ 
  "ModelName": "llama3.2",
  "OpenAIKey": "OpenAI",
  "WeatherApiKey": ""
}
```

You have to run the chat model engine locally. I recommend using [Ollama](https://ollama.com), and LLama3 as the model.

You also need an account with [openweathermap.org](https://openweathermap.org) to get an API key for the weather functionality. The account is free and allows for a decent amount of API calls before you have to pay.