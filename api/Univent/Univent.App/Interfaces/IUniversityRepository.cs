using Univent.Domain.Models.Universities;

namespace Univent.App.Interfaces
{
    public interface IUniversityRepository : IBaseRepository<University>
    {
        Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default);
        Task<ICollection<University>> SearchUniversityAsync(string query, CancellationToken ct = default);
    }
}
