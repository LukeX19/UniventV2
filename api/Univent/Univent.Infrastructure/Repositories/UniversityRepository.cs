using Microsoft.EntityFrameworkCore;
using Univent.App.Interfaces;
using Univent.App.Pagination.Dtos;
using Univent.Domain.Models.Universities;
using Univent.Infrastructure.Repositories.BasicRepositories;

namespace Univent.Infrastructure.Repositories
{
    public class UniversityRepository : BaseRepositoryEF<University>, IUniversityRepository
    {
        public UniversityRepository(AppDbContext context) : base(context) { }

        public async Task<PaginationResponseDto<University>> GetAllUniversitiesAsync(PaginationRequestDto pagination, CancellationToken ct = default)
        {
            var query = _context.Universities
                .AsNoTracking()
                .AsSplitQuery()
                .OrderBy(u => u.Name)
                .AsQueryable();

            int totalUniversities = await query.CountAsync(ct);

            var universities = await query
                .Skip((pagination.PageIndex - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync(ct);

            int totalPages = (int)Math.Ceiling((double)totalUniversities / pagination.PageSize);

            return new PaginationResponseDto<University>(universities, pagination.PageIndex, totalPages, totalUniversities);
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default)
        {
            return await _context.Universities.AnyAsync(u => u.Name == name, ct);
        }

        public async Task<ICollection<University>> SearchUniversityAsync(string query, CancellationToken ct = default)
        {
            return await _context.Universities
                .Where(u => EF.Functions.Like(u.Name, $"%{query}%"))
                .ToListAsync(ct);
        }
    }
}
