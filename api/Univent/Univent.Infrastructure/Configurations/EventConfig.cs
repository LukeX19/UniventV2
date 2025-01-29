using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Univent.Domain.Models.Events;

namespace Univent.Infrastructure.Configurations
{
    public class EventConfig : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            builder
                .HasOne(e => e.Type)
                .WithMany(et => et.Events)
                .HasForeignKey(e => e.TypeId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(e => e.Author)
                .WithMany(u => u.CreatedEvents)
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
