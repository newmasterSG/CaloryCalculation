using CaloryCalculatiom.Domain.Entities;
using CaloryCalculation.API.Configurations;
using CaloryCalculation.API.Endpoints;
using CaloryCalculation.Db;

namespace CaloryCalculation.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddServices(builder.Configuration);
            builder.Logging.AddConsole()
                 .AddFilter("Microsoft.AspNetCore.Authentication", LogLevel.Debug);
            var app = builder.Build().Configure();

            using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
                var clDbContext = scope.ServiceProvider.GetService<CaloryCalculationDbContext>();
                await clDbContext.Database.EnsureCreatedAsync();


            await app.RunAsync();
        }
    }
}
