using Univent.Domain.Models.BasicEntities;

namespace Univent.Domain.Models.Users
{
    public class Feedback : BaseEntity
    {
        public double Rating { get; set; }
        public Guid UserId { get; set; }
        public AppUser User { get; set; }
    }
}
