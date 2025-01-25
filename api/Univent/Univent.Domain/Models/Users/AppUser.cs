using Univent.Domain.Enums;
using Univent.Domain.Models.Associations;
using Univent.Domain.Models.Events;
using Univent.Domain.Models.Universities;

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
        public ICollection<Event> CreatedEvents { get; set; }
        public ICollection<EventParticipant> ParticipatingEvents { get; set; }
        public University University { get; set; }
        public ICollection<Feedback> Feedbacks { get; set; }
    }
}
