namespace Univent.App.Interfaces
{
    public interface IAiAssistantService
    {
        Task<string> AskQuestionAboutEventsAsync(string question, ICollection<string> eventNames);
    }
}
