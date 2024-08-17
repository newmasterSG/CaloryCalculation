using CaloryCalculatiom.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CaloryCalculation.Db
{
    public class CaloryCalculationDbContext(DbContextOptions<CaloryCalculationDbContext> options) : IdentityDbContext<User, IdentityRole<int>, int>(options)
    {
        public DbSet<DailyLog> DailyLogs { get; set; }
        public DbSet<User> Users {  get; set; }
        public DbSet<Exercise> Exercises { get; set; }

        public DbSet<FoodConsumption> FoodConsumptions { get; set; }

        public DbSet<Goal> Goals { get; set; }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
