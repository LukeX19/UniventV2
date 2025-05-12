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

        public async Task<string> AskQuestionAboutEventsAsync(string question, ICollection<string> eventNames)
        {
            // Real GPT API call
            var apiKey = _configuration["OpenAI:ApiKey"];
            var prompt = $"Here are some event names:\n\n{string.Join("\n\n", eventNames)}\n\nBased on these names, answer this question: \n\"{question}\"";

            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content = "You are a helpful assistant that summarizes customer reviews." },
                    new { role = "user", content = prompt }
                }
            };

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            int maxRetries = 3;
            for (int retry = 0; retry < maxRetries; retry++)
            {
                var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    await Task.Delay(2000 * (retry + 1)); // exponential backoff
                    continue;
                }

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
                    .GetString();
            }

            throw new Exception("OpenAI API request failed after multiple retries due to rate limiting.");
        }
    }
}
