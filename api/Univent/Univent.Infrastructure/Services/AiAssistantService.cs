using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Univent.App.Interfaces;
using Univent.App.Weather.Dtos;

namespace Univent.Infrastructure.Services
{
    public class AiAssistantService : IAiAssistantService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AiAssistantService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> AskForInterestsBasedSuggestionsAsync(string userDescription, ICollection<string> eventSummaries)
        {
            var stopwatch = Stopwatch.StartNew();

            var _endpoint = _configuration["OpenAI:Endpoint"];
            var _apiKey = _configuration["OpenAI:ApiKey"];
            var _model = _configuration["OpenAI:Model"];

            var prompt = @$"
                You are a helpful assistant that recommends events to users based on their interests.
                The user said: ""{userDescription}""

                Here are some upcoming events:
                {string.Join("\n", eventSummaries)}

                Based on the user's preferences, suggest the most relevant events.
                Respond in a friendly, human tone and briefly explain why each event might be a good fit.
                ";

            var requestBody = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful event recommendation assistant." },
                    new { role = "user", content = prompt }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.PostAsync(_endpoint, content);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseBody);

            stopwatch.Stop();

            Console.WriteLine($"Execution time: {stopwatch.ElapsedMilliseconds} ms");

            return jsonDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "No recommendation was generated.";
        }

        public async Task<string> AskForLocationBasedSuggestionsAsync(string locationInfo, ICollection<string> eventSummaries)
        {
            var stopwatch = Stopwatch.StartNew();

            var _endpoint = _configuration["OpenAI:Endpoint"];
            var _apiKey = _configuration["OpenAI:ApiKey"];
            var _model = _configuration["OpenAI:Model"];

            var prompt = @$"
                You are a helpful assistant that recommends events to users based on their location.
                The user is looking for events that match the following location preference:

                ""{locationInfo}""

                Below is a list of upcoming events with details:

                {string.Join("\n", eventSummaries)}

                Please suggest the most suitable events based on location relevance and briefly explain why. Limit your suggestions to the top 3–5 matches.
                ";

            var requestBody = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "system", content = "You are an assistant that recommends local events to users based on location preferences." },
                    new { role = "user", content = prompt }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.PostAsync(_endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"AI API error: {response.StatusCode} - {errorContent}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseBody);

            stopwatch.Stop();

            Console.WriteLine($"Execution time: {stopwatch.ElapsedMilliseconds} ms");

            return jsonDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "No recommendation was generated.";
        }

        public async Task<string> AskForTimeBasedSuggestionsAsync(string timePreference, ICollection<string> eventSummaries)
        {
            var stopwatch = Stopwatch.StartNew();

            var _endpoint = _configuration["OpenAI:Endpoint"];
            var _apiKey = _configuration["OpenAI:ApiKey"];
            var _model = _configuration["OpenAI:Model"];

            var currentDate = DateTime.UtcNow.AddHours(3).ToString("f");

            var prompt = @$"
                You are an assistant that helps users find events based on their availability.

                The current date and time is: {currentDate}

                User is interested in attending events during this time range or preference:
                ""{timePreference}""

                Here is a list of upcoming events:
                {string.Join("\n", eventSummaries)}

                Instructions:
                - Analyze the user's time preference in the context of the current date.
                - Suggest 3 to 5 events that best fit their schedule.
                - Mention why each event matches, including its start time.
                - Use a clear, helpful, and friendly tone.
                ";

            var requestBody = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "system", content = "You are a smart event assistant that matches user availability with upcoming events." },
                    new { role = "user", content = prompt }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.PostAsync(_endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"AI API error: {response.StatusCode} - {errorContent}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseBody);

            stopwatch.Stop();

            Console.WriteLine($"Execution time: {stopwatch.ElapsedMilliseconds} ms");

            return jsonDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "No recommendation was generated.";
        }

        public async Task<string> AskForWeatherBasedSuggestionsAsync(ICollection<string> eventSummaries, ICollection<DailyWeatherForecastResponseDto> forecast)
        {
            var stopwatch = Stopwatch.StartNew();

            var _endpoint = _configuration["OpenAI:Endpoint"];
            var _apiKey = _configuration["OpenAI:ApiKey"];
            var _model = _configuration["OpenAI:Model"];

            var today = DateTime.UtcNow.ToString("yyyy-MM-dd");

            var weatherDetails = string.Join("\n", forecast.Select(f =>
                $"- Date: {f.Date:yyyy-MM-dd}, Condition: {f.Condition} ({f.Description}), Temp: {f.TempMin}–{f.TempMax}°C, Rain: {(f.RainVolume.HasValue ? $"{f.RainVolume}mm" : "0mm")}, POP: {f.PrecipitationProbability:P0}, UVI: {f.Uvi}, Humidity: {f.Humidity}%, Wind: {f.WindSpeed} m/s"));

            var eventList = string.Join("\n", eventSummaries);

            var prompt = @$"
                You are a helpful assistant that recommends events from the given list, based on weather conditions for the next 8 days in Timișoara, Romania.

                Today is {today}. Here is the weather forecast:
                {weatherDetails}

                Here are upcoming events (with their dates and descriptions):
                {eventList}

                Instructions:
                - Suggest 3–5 REAL events from this list that best fit the weather forecast.
                - Match indoor events with rainy days, and outdoor events with sunny/mild days.
                - Always include the actual event name and date in your recommendation.
                - Briefly explain *why* each event is a good fit.
                - Do NOT invent events. Use only what's provided.

                Respond in a clear, helpful, and friendly tone. Suggest 3–5 events at most.
                ";

            var requestBody = new
            {
                model = _model,
                messages = new[]
                {
                    new { role = "system", content = "You are a smart assistant that suggests weather-friendly events." },
                    new { role = "user", content = prompt }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.PostAsync(_endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"AI API error: {response.StatusCode} - {errorContent}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseBody);

            stopwatch.Stop();

            Console.WriteLine($"Execution time: {stopwatch.ElapsedMilliseconds} ms");

            return jsonDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "No recommendation was generated.";
        }
    }
}
