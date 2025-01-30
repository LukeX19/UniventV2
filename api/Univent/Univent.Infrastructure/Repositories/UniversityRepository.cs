using Microsoft.EntityFrameworkCore;
using Univent.App.Interfaces;
using Univent.Domain.Models.Universities;
using Univent.Infrastructure.Repositories.BasicRepositories;

namespace Univent.Infrastructure.Repositories
{
    public class UniversityRepository : BaseRepositoryEF<University>, IUniversityRepository
    {
        public UniversityRepository(AppDbContext context) : base(context) { }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default)
        {
            return await _context.Universities.AnyAsync(u => u.Name == name, ct);
        }
    }
}
