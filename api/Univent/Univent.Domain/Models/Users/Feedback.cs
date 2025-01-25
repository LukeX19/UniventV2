using Univent.Domain.Models.BasicEntities;

namespace Univent.Domain.Models.Users
{
    public class Feedback : BaseEntity
    {
        public double Score { get; set; }
        public Guid UserId { get; set; }
    }
}
