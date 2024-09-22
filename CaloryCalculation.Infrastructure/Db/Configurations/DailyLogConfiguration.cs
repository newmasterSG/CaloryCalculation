using CaloryCalculatiom.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalculation.Infrastructure.Db.Configurations
{
    public class DailyLogConfiguration : IEntityTypeConfiguration<DailyLog>
    {
        public void Configure(EntityTypeBuilder<DailyLog> builder)
        {
            builder.HasOne(dl => dl.User)
                .WithMany(u => u.DailyLogs)
                .HasForeignKey(dl => dl.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(dl => dl.FoodConsumptions)
                .WithOne(fc => fc.DailyLog)
                .HasForeignKey(fc => fc.DailyLogId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
