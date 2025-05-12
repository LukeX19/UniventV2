using Univent.App.Pagination.Dtos;
using Univent.Domain.Models.Users;

namespace Univent.App.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByIdAsync(Guid id, CancellationToken ct = default);
        Task<PaginationResponseDto<AppUser>> GetAllUsersAsync(PaginationRequestDto pagination, CancellationToken ct = default);
        Task<Dictionary<Guid, double>> GetAverageRatingsAsync(ICollection<Guid> userIds, CancellationToken ct);
        Task UpdateAsync(AppUser updatedEntity, CancellationToken ct = default);
        Task DeleteAsync(Guid userId, CancellationToken ct = default);
    }
}
