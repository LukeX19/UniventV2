using Univent.App.Pagination.Dtos;
using Univent.Domain.Models.Universities;

namespace Univent.App.Interfaces
{
    public interface IUniversityRepository : IBaseRepository<University>
    {
        Task<PaginationResponseDto<University>> GetAllAsync(PaginationRequestDto pagination, CancellationToken ct = default);
        Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);
        Task<ICollection<University>> SearchUniversityAsync(string query, CancellationToken ct = default);
    }
}
