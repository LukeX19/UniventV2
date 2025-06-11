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

        private async Task<string> SendAsync(string prompt)
        {
            var stopwatch = Stopwatch.StartNew();

            var requestOptions = new ChatCompletionsOptions()
            {
                Model = _model,
                MaxTokens = 2000,
                Temperature = 0.7f,
                Messages = { new ChatRequestUserMessage(prompt) }
            };

            Response<ChatCompletions> response = await _client.CompleteAsync(requestOptions);

            stopwatch.Stop();
            Console.WriteLine($"{_model} Execution time: {stopwatch.ElapsedMilliseconds} ms");

            return response.Value.Content ?? "No recommendation was generated.";
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
                You are a helpful assistant that recommends events to users based on their location.
                The user is looking for events that match the following location preference:
                ""{locationInfo}""

                Here is a list of upcoming events:
                {string.Join("\n", eventSummaries)}

                Please suggest the most suitable events based on location relevance. Limit suggestions to 1-3 and explain briefly.";
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
            var today = DateTime.UtcNow.ToString("yyyy-MM-dd");

            var weatherDetails = string.Join("\n", forecast.Select(f =>
                $"- Date: {f.Date:yyyy-MM-dd}, Condition: {f.Condition} ({f.Description}), Temp: {f.TempMin}–{f.TempMax}°C, Rain: {(f.RainVolume.HasValue ? $"{f.RainVolume}mm" : "0mm")}, POP: {f.PrecipitationProbability:P0}, UVI: {f.Uvi}, Humidity: {f.Humidity}%, Wind: {f.WindSpeed} m/s"));

            var eventList = string.Join("\n", eventSummaries);

            var prompt = @$"
                You are a helpful assistant that recommends events based on weather conditions in Timișoara, Romania.

                Today is {today}.
                Here is the weather forecast:
                {weatherDetails}

                Here are upcoming events:
                {eventList}

                Suggest 1-3 events that fit the weather.
                Prefer indoor events if it’s expected to rain, be stormy, extremely hot, or very cold. Favor outdoor events when the weather is clear and mild. Avoid suggesting events that wouldn't feel good to attend due to the weather.
                Explain briefly why each event is a good fit.";
            return await SendAsync(prompt);
        }
    }
}
