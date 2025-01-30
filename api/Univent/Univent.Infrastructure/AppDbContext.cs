using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Univent.Domain.Models.Associations;
using Univent.Domain.Models.BasicEntities;
using Univent.Domain.Models.Events;
using Univent.Domain.Models.Universities;
using Univent.Domain.Models.Users;
using Univent.Infrastructure.Configurations;

namespace Univent.Infrastructure
{
    public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public DbSet<University> Universities { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }

        public AppDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new EventConfig());
            builder.ApplyConfiguration(new EventParticipantConfig());
            builder.ApplyConfiguration(new FeedbackConfig());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            var entries = ChangeTracker.Entries<AuditableEntity>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }

            return await base.SaveChangesAsync(ct);
        }
    }
}
