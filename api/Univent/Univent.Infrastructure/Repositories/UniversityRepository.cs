using Univent.App.Interfaces;
using Univent.Domain.Models.Universities;
using Univent.Infrastructure.Repositories.BasicRepositories;

namespace Univent.Infrastructure.Repositories
{
    public class UniversityRepository : BaseRepositoryEF<University>, IUniversityRepository
    {
        public UniversityRepository(AppDbContext context) : base(context) { }
    }
}
