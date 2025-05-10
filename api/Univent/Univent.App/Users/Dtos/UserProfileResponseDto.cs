using Univent.Domain.Enums;

namespace Univent.App.Users.Dtos
{
    public class UserProfileResponseDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string? PictureUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public UniversityYear Year { get; set; }
        public Guid UniversityId { get; set; }
        public string UniversityName { get; set; }
        public double Rating { get; set; }
        public int CreatedEvents { get; set; }
        public int Participations { get; set; }
    }
}
