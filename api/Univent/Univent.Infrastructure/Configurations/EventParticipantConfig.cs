using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Univent.Domain.Models.Associations;

namespace Univent.Infrastructure.Configurations
{
    public class EventParticipantConfig : IEntityTypeConfiguration<EventParticipant>
    {
        public void Configure(EntityTypeBuilder<EventParticipant> builder)
        {
            builder
                .HasKey(ep => new { ep.EventId, ep.UserId });

            builder
                .HasOne(ep => ep.Event)
                .WithMany(e => e.Participants)
                .HasForeignKey(ep => ep.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(ep => ep.User)
                .WithMany(u => u.EnrolledEvents)
                .HasForeignKey(ep => ep.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
