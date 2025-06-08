using Univent.App.Interfaces;
using Univent.Domain.Models.Users;
using Univent.Infrastructure.Repositories.BasicRepositories;

namespace Univent.Infrastructure.Repositories
{
    public class FeedbackRepository : BaseRepositoryEF<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(AppDbContext context) : base(context) { }

        public async Task AddRangeAsync(ICollection<Feedback> feedbacks, CancellationToken ct = default)
        {
            await _context.Feedbacks.AddRangeAsync(feedbacks, ct);
        }
    }
}
