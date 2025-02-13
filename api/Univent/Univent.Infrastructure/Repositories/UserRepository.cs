using Microsoft.EntityFrameworkCore;
using Univent.App.Interfaces;
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

        public async Task<AppUser> GetUserById(Guid id, CancellationToken ct = default)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id, ct)
                ?? throw new EntityNotFoundException("User", id);
        }
    }
}
