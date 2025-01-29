namespace Univent.App.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUniversityRepository UniversityRepository { get; }
        IAppUserRepository AppUserRepository { get; }
        IEventTypeRepository EventTypeRepository { get; }
        IEventRepository EventRepository { get; }
        IFeedbackRepository FeedbackRepository { get; }

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
        Task<int> SaveChangesAsync();
    }
}
