using CaloryCalculatiom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaloryCalculation.Infrastructure.Db.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Gender)
                .HasColumnType("smallint");

            builder.HasMany(u => u.DailyLogs)
                .WithOne(dl => dl.User)
                .HasForeignKey(dl => dl.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
