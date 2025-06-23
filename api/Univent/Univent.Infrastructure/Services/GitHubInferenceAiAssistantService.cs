using Azure;
using Azure.AI.Inference;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using Univent.App.Interfaces;
using Univent.App.Weather.Dtos;

namespace Univent.Infrastructure.Services
{
    public class GitHubInferenceAiAssistantService : IAiAssistantService
    {
        private readonly ChatCompletionsClient _client;
        private readonly string _model;

        public GitHubInferenceAiAssistantService(IConfiguration configuration)
        {
            var endpoint = new Uri(configuration["GitHubAIModels:Endpoint"]);
            var apiKey = configuration["GitHubAIModels:ApiKey"];
            _model = configuration["GitHubAIModels:Model"];

            _client = new ChatCompletionsClient(
                endpoint,
                new AzureKeyCredential(apiKey),
                new AzureAIInferenceClientOptions());
        }

        private async Task<string> SendAsync(string prompt, int timeoutInSeconds = 30)
        {
            var stopwatch = Stopwatch.StartNew();

            var requestOptions = new ChatCompletionsOptions()
            {
                Model = _model,
                MaxTokens = 2000,
                Temperature = 0.7f,
                Messages = { new ChatRequestUserMessage(prompt) }
            };

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutInSeconds));

            try
            {
                Response<ChatCompletions> response = await _client.CompleteAsync(requestOptions, cts.Token);

                stopwatch.Stop();
                Console.WriteLine($"{_model} Execution time: {stopwatch.ElapsedMilliseconds} ms");

                return response.Value.Content ?? "No recommendation was generated.";
            }
            catch (OperationCanceledException)
            {
                throw new OperationCanceledException($"Request timed out after {timeoutInSeconds} seconds.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error during model inference: {ex.Message}");
            }
        }

        public async Task<string> AskForInterestsBasedSuggestionsAsync(string userDescription, ICollection<string> eventSummaries)
        {
            var prompt = @$"
                You are a helpful assistant that recommends events to users based on their interests.
                The user said: ""{userDescription}""

                Here are some upcoming events:
                {string.Join("\n", eventSummaries)}

                Based on the user's preferences, suggest the most relevant events. Limit suggestions to 1-3 events.
                Respond in a friendly, human tone and briefly explain why each event might be a good fit.";
            return await SendAsync(prompt);
        }

        public async Task<string> AskForLocationBasedSuggestionsAsync(string locationInfo, ICollection<string> eventSummaries)
        {
            var prompt = @$"
                You are a helpful assistant that recommends events to users based on their preffered location.

                The user provided the following location preference:
                ""{locationInfo}""

                Here is a list of upcoming events:
                {string.Join("\n", eventSummaries)}

                Select the 1–3 events that are the *best overall match* for the user's location preference, regardless of how soon they are happening.
    
                **Instructions:**
                - Focus only on how well each event's location aligns with the user's stated preference.
                - If the user describes a location using landmarks, neighborhoods, or vague terms (e.g., 'near the student campus', 'downtown', 'close to the dorms'), use your knowledge of Timișoara to infer where those areas are.
                - Use the coordinates of each event to help estimate which ones are likely close to the described area.
                - Do not prioritize events just because they occur sooner — only proximity matters here.
                - Briefly explain why each recommended event is a good match in terms of location.

                Respond with your top 1–3 location-matched event suggestions.";
            return await SendAsync(prompt);
        }

        public async Task<string> AskForTimeBasedSuggestionsAsync(string timePreference, ICollection<string> eventSummaries)
        {
            var currentDate = DateTime.UtcNow.AddHours(3).ToString("f");
            var prompt = @$"
                You are an assistant that helps users find events based on their availability.
                Current date and time: {currentDate}

                User prefers to attend events during:
                ""{timePreference}""

                Here are upcoming events:
                {string.Join("\n", eventSummaries)}

                Suggest 1-3 events that best fit their schedule and explain why.";
            return await SendAsync(prompt);
        }

        public async Task<string> AskForWeatherBasedSuggestionsAsync(ICollection<string> eventSummaries, ICollection<DailyWeatherForecastResponseDto> forecast)
        {
            var today = DateTime.UtcNow.AddHours(3).ToString("yyyy-MM-dd");

            var weatherDetails = string.Join("\n", forecast.Select(f =>
                $"- Date: {f.Date:yyyy-MM-dd}, Condition: {f.Condition} ({f.Description}), Temp: {f.TempMin}–{f.TempMax}°C, Rain: {(f.RainVolume.HasValue ? $"{f.RainVolume}mm" : "0mm")}, POP: {f.PrecipitationProbability:P0}, UVI: {f.Uvi}, Humidity: {f.Humidity}%, Wind: {f.WindSpeed} m/s"));

            var eventList = string.Join("\n", eventSummaries);

            var prompt = @$"
                You are a helpful assistant that recommends events based on weather conditions in Timișoara, Romania.

                Today is {today}.
                Here is the 8-day weather forecast:
                {weatherDetails}

                Here are the upcoming events:
                {eventList}

                Select the 1–3 events that are the *best overall match* for the forecasted weather, regardless of how close they are to today. Consider the date of the event and the weather on that specific day.

                **Instructions:**
                - Favor indoor events if the event date has high rain chance, storms, extreme heat, or cold.
                - Favor outdoor events when the forecast is clear, mild, and comfortable.
                - Base your suggestions only on the quality of the weather match, *not on how soon the event happens*.
                - Briefly explain why each event is a good match for the weather conditions on its specific date.

                Respond with your top 1–3 weather-aligned event suggestions.";
            return await SendAsync(prompt);
        }
    }
}
