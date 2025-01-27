using Univent.Domain.Models.BasicEntities;

namespace Univent.App.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<T> CreateAsync(T entity, CancellationToken ct = default);
        Task<ICollection<T>> GetAllAsync(CancellationToken ct = default);
        Task<T> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task UpdateAsync(T entity, CancellationToken ct = default);
        Task DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
