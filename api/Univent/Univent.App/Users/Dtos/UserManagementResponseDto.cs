using Univent.Domain.Enums;

namespace Univent.App.Users.Dtos
{
    public class UserManagementResponseDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
        public UniversityYear Year { get; set; }
        public string UniversityName { get; set; }
        public bool IsAccountConfirmed { get; set; }
        public bool IsAccountBanned { get; set; }
    }
}
