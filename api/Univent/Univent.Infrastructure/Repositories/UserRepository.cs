using Microsoft.EntityFrameworkCore;
using Univent.App.Interfaces;
using Univent.App.Pagination.Dtos;
using Univent.Domain.Enums;
using Univent.Domain.Models.Users;
using Univent.Infrastructure.Exceptions;

namespace Univent.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser> GetUserByIdAsync(Guid id, CancellationToken ct = default)
        {
            var user = await _context.Users
                .AsNoTracking()
                .AsSplitQuery()
                .Include(u => u.University)
                .FirstOrDefaultAsync(u => u.Id == id, ct);

            return user ?? throw new EntityNotFoundException("User", id);
        }

        public async Task<PaginationResponseDto<AppUser>> GetAllUsersAsync(PaginationRequestDto pagination, CancellationToken ct = default)
        {
            var query = _context.Users
                .AsNoTracking()
                .AsSplitQuery()
                .Where(u => u.Role == AppRole.Student)
                .Include(u => u.University)
                .AsQueryable();

            query = query.OrderBy(u => u.FirstName);

            int totalUsers = await query.CountAsync(ct);

            var users = await query
                .Skip((pagination.PageIndex - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync(ct);

            int totalPages = (int)Math.Ceiling((double)totalUsers / pagination.PageSize);

            return new PaginationResponseDto<AppUser>(users, pagination.PageIndex, totalPages, totalUsers);
        }

        public async Task<Dictionary<Guid, double>> GetAverageRatingsAsync(ICollection<Guid> userIds, CancellationToken ct)
        {
            return await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .Select(u => new
                {
                    UserId = u.Id,
                    AverageRating = u.Feedbacks.Any() ? u.Feedbacks.Average(f => f.Rating) : 0.0
                })
                .ToDictionaryAsync(u => u.UserId, u => u.AverageRating, ct);
        }

        public async Task UpdateAsync(AppUser updatedEntity, CancellationToken ct = default)
        {
            var entityExists = await _context.Users.AnyAsync(e => e.Id == updatedEntity.Id, ct);
            if (!entityExists)
            {
                throw new EntityNotFoundException(typeof(AppUser).Name, updatedEntity.Id);
            }

            _context.Users.Update(updatedEntity);
            await _context.SaveChangesAsync(ct);
        }
    }
}
