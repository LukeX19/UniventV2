using Univent.App.Interfaces;

namespace Univent.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IUniversityRepository UniversityRepository { get; private set; }
        public IUserRepository UserRepository { get; private set; }
        public IEventTypeRepository EventTypeRepository { get; private set; }
        public IEventRepository EventRepository { get; private set; }
        public IEventParticipantRepository EventParticipantRepository { get; private set; }
        public IFeedbackRepository FeedbackRepository { get; private set; }

        public UnitOfWork(AppDbContext context, IUniversityRepository universityRepository,
            IUserRepository userRepository, IEventTypeRepository eventTypeRepository,
            IEventRepository eventRepository, IEventParticipantRepository eventParticipantRepository,
            IFeedbackRepository feedbackRepository)
        {
            _context = context;
            UniversityRepository = universityRepository;
            UserRepository = userRepository;
            EventTypeRepository = eventTypeRepository;
            EventRepository = eventRepository;
            EventParticipantRepository = eventParticipantRepository;
            FeedbackRepository = feedbackRepository;
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
