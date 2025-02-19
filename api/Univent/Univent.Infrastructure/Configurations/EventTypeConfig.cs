using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Univent.Domain.Models.Events;

namespace Univent.Infrastructure.Configurations
{
    public class EventTypeConfig : IEntityTypeConfiguration<EventType>
    {
        public void Configure(EntityTypeBuilder<EventType> builder)
        {
            builder
                .HasIndex(et => et.Name)
                .IsUnique();
        }
    }
}
