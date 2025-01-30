using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Univent.Domain.Models.Universities;

namespace Univent.Infrastructure.Configurations
{
    public class UniversityConfig : IEntityTypeConfiguration<University>
    {
        public void Configure(EntityTypeBuilder<University> builder)
        {
            builder
                .HasIndex(u => u.Name)
                .IsUnique();
        }
    }
}
