using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Univent.App.Interfaces;

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
            var apiKey = _configuration["OpenAI:ApiKey"];

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
                model = "gpt-4.1",
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful event recommendation assistant." },
                    new { role = "user", content = prompt }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseBody);

            return jsonDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "No recommendation was generated.";
        }

        public async Task<string> AskForLocationBasedSuggestionsAsync(string locationInfo, ICollection<string> eventSummaries)
        {
            var apiKey = _configuration["OpenAI:ApiKey"];

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
                model = "gpt-4.1",
                messages = new[]
                {
                    new { role = "system", content = "You are an assistant that recommends local events to users based on location preferences." },
                    new { role = "user", content = prompt }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"OpenAI API error: {response.StatusCode} - {errorContent}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseBody);

            return jsonDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "No recommendation was generated.";
        }

        public async Task<string> AskForTimeBasedSuggestionsAsync(string timePreference, ICollection<string> eventSummaries)
        {
            var apiKey = _configuration["OpenAI:ApiKey"];

            var currentDate = DateTime.UtcNow.ToString("f");

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
                model = "gpt-4.1",
                messages = new[]
                {
                    new { role = "system", content = "You are a smart event assistant that matches user availability with upcoming events." },
                    new { role = "user", content = prompt }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"OpenAI API error: {response.StatusCode} - {errorContent}");
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            using var jsonDoc = JsonDocument.Parse(responseBody);

            return jsonDoc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "No recommendation was generated.";
        }

    }
}
