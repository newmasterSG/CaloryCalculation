using CaloryCalculatiom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CaloryCalculation.Infrastructure.Db.Configurations
{
    public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
    {
        public void Configure(EntityTypeBuilder<Exercise> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(p => p.CaloriesBurnedPerHour)
                .IsRequired();

            builder.HasOne(e => e.CreatedUser)
                .WithMany(u => u.CreatedExerices)
                .HasForeignKey(e => e.CreatedUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
