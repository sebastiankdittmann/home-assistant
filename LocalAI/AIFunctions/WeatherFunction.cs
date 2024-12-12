namespace LocalAI.AIFunctions;

using System.Text.Json;
using System.Text.Json.Serialization;

public class WeatherFunction (HttpClient client, WeatherOptions options)
{
    public async Task<WeatherResponse> GetWeather(string city)
    {
        var response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={options.ApiKey}&units=metric");
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Failed to get weather data");
        }

        var weather = JsonSerializer.Deserialize<WeatherResponse>(content);

        if (weather == null)
        {
            throw new Exception("Failed to deserialize weather data");
        }

        return weather;
    }
}

public class WeatherOptions
{
    public string ApiKey { get; set; }
}

public class WeatherResponse
{
    [JsonPropertyName("name")]
    public string City { get; set; }

    [JsonPropertyName("wind")]
    public Wind Wind { get; set; }
    
    [JsonPropertyName("weather")]
    public Weather[] Weather { get; set; }
    
    [JsonPropertyName("main")]
    public WeatherMetrics Main { get; set; }
}

public class Weather
{
    [JsonPropertyName("description")] public string Description { get; set; }
}

public class Wind
{
    [JsonPropertyName("speed")] public double Speed { get; set; }
    [JsonPropertyName("speedUnit")] public const string SpeedUnit = "m/s";
    [JsonPropertyName("deg")] public int Degree { get; set; }
}

public class WeatherMetrics
{
    [JsonPropertyName("temp_min")] public double MinTemperatur { get; set; }
    [JsonPropertyName("temp_max")] public double MaxTemperatur { get; set; }
    [JsonPropertyName("humidity")] public int Humidity { get; set; }
}
    