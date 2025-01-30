using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Univent.Domain.Models.Users;

namespace Univent.Infrastructure.Configurations
{
    public class UserConfig : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder
                .HasOne(u => u.University)
                .WithMany(univ => univ.Users)
                .HasForeignKey(u => u.UniversityId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
