using Microsoft.EntityFrameworkCore;
using Univent.App.Interfaces;
using Univent.Domain.Models.BasicEntities;
using Univent.Infrastructure.Exceptions;

namespace Univent.Infrastructure.Repositories.BasicRepositories
{
    public class BaseRepositoryEF<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly AppDbContext _context;

        public BaseRepositoryEF(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateAsync(T entity, CancellationToken ct = default)
        {
            if (_context.Set<T>().Contains(entity))
            {
                throw new EntityAlreadyExistsException(typeof(T).Name, entity.Id);
            }

            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync(ct);
            return entity.Id;
        }

        public async Task<ICollection<T>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync(ct);
        }

        public async Task<T> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id, ct);
            return entity ?? throw new EntityNotFoundException(typeof(T).Name, id);
        }

        public async Task UpdateAsync(T updatedEntity, CancellationToken ct = default)
        {
            var entityExists = await _context.Set<T>().AnyAsync(e => e.Id == updatedEntity.Id, ct);
            if (!entityExists)
            {
                throw new EntityNotFoundException(typeof(T).Name, updatedEntity.Id);
            }

            _context.Set<T>().Update(updatedEntity);
            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var entityToDelete = await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id, ct);
            if (entityToDelete == null)
            {
                throw new EntityNotFoundException(typeof(T).Name, id);
            }

            _context.Set<T>().Remove(entityToDelete);
            await _context.SaveChangesAsync(ct);
        }
    }
}
