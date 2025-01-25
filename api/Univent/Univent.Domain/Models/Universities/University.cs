using Univent.Domain.Models.BasicEntities;
using Univent.Domain.Models.Users;

namespace Univent.Domain.Models.Universities
{
    public class University : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<AppUser> Users { get; set; }
    }
}
