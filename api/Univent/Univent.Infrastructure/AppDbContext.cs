using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Univent.Domain.Models.Associations;
using Univent.Domain.Models.Events;
using Univent.Domain.Models.Universities;
using Univent.Domain.Models.Users;

namespace Univent.Infrastructure
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public DbSet<AppUser> Users { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
