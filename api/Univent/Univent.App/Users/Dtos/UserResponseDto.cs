using Univent.Domain.Enums;

namespace Univent.App.Users.Dtos
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string? PictureUrl { get; set; }
        public AppRole Role { get; set; }
        public UniversityYear Year { get; set; }
        public bool IsAccountConfirmed { get; set; }
        public Guid? UniversityId { get; set; }
    }
}
