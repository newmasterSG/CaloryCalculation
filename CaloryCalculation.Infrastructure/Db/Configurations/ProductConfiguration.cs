using CaloryCalculatiom.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaloryCalculation.Infrastructure.Db.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(p => p.Calories)
                .IsRequired();

            builder.Property(p => p.Fat)
                .IsRequired();

            builder.Property(p => p.Сarbohydrate)
                .IsRequired();

            builder.Property(p => p.Protein)
                .IsRequired();

            builder.HasOne(p => p.CreatedUser)
                .WithMany(u => u.CreatedProducts)
                .HasForeignKey(p => p.CreatedUserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
