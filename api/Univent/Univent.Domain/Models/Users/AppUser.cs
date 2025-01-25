using Univent.Domain.Enums;

namespace Univent.Domain.Models.Users
{
    public class AppUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string PictureUrl { get; set; }
        public AppRole Role { get; set; }
        public UniversityYear Year { get; set; }
        public bool IsAccountConfirmed { get; set; }
    }
}
