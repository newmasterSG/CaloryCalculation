using CaloryCalculation.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalculation.Infrastructure.Db.Configurations
{
    public class DailyLogExerciseConfiguration : IEntityTypeConfiguration<DailyLogExercise>
    {
        public void Configure(EntityTypeBuilder<DailyLogExercise> builder)
        {
            builder.HasKey(dle => new { dle.DailyLogId, dle.ExerciseId });

            builder.HasOne(dle => dle.DailyLog)
                .WithMany(dl => dl.DailyLogExercises)
                .HasForeignKey(dle => dle.DailyLogId);

            builder.HasOne(dle => dle.Exercise)
                .WithMany(e => e.DailyLogExercises)
                .HasForeignKey(dle => dle.ExerciseId);
        }
    }
}
