using CaloryCalculatiom.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalculation.Infrastructure.Db.Configurations
{
    public class FoodConsumptionConfiguration : IEntityTypeConfiguration<FoodConsumption>
    {
        public void Configure(EntityTypeBuilder<FoodConsumption> builder)
        {
            builder.Property(u => u.MealType)
                .HasColumnType("smallint");

            builder.HasOne(fc => fc.DailyLog)
                .WithMany(dl => dl.FoodConsumptions)
                .HasForeignKey(fc => fc.DailyLogId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(fc => fc.Product)
                .WithMany(p => p.FoodConsumptions)
                .HasForeignKey(fc => fc.FoodItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
