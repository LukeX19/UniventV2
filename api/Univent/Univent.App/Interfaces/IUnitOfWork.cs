namespace Univent.App.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUniversityRepository UniversityRepository { get; }
        IUserRepository UserRepository { get; }
        IEventTypeRepository EventTypeRepository { get; }
        IEventRepository EventRepository { get; }
        IEventParticipantRepository EventParticipantRepository { get; }
        IFeedbackRepository FeedbackRepository { get; }

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<int> SaveChangesAsync();
    }
}
