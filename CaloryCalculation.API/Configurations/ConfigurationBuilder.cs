using CaloryCalculatiom.Domain.Entities;
using CaloryCalculation.Application.Interfaces;
using CaloryCalculation.Application.Services;
using CaloryCalculation.Db;
using Microsoft.EntityFrameworkCore;

namespace CaloryCalculation.API.Configurations
{
    public static class ConfigurationBuilder
    {
        public static IServiceCollection AddServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuthorization();

            services.AddDb(configuration);

            services.AddCustomService();

            services.AddMediator();

            services.AddIdentity();

            services.AddSwagger();

            return services;
        }

        private static IServiceCollection AddDb(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var conStr = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<CaloryCalculationDbContext>(options =>
            {
                options.UseNpgsql(conStr);
            });

            return services;
        }

        private static IServiceCollection AddIdentity(
            this IServiceCollection services)
        {
            services.AddIdentityApiEndpoints<User>()
                .AddEntityFrameworkStores<CaloryCalculationDbContext>();

            return services;
        }

        private static IServiceCollection AddCustomService(this IServiceCollection services)
        {
            services.AddScoped<IDailyLogService, DailyLogService>();

            return services;
        }

        private static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }

        private static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Application.Handlers.Products.CreateProductHandler).Assembly));

            return services;
        }
    }
}
