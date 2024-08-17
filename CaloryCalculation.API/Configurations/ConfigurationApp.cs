using CaloryCalculatiom.Domain.Entities;
using CaloryCalculation.API.Endpoints;

namespace CaloryCalculation.API.Configurations
{
    public static class ConfigurationApp
    {
        public static WebApplication Configure(this WebApplication application)
        {
            // Configure the HTTP request pipeline.

            application.UseHttpsRedirection();

            application.UseAuthorization();

            application.UseAuthentication();

            application.UseSwagger();
            application.UseSwaggerUI();

            application.MapIdentityApi<User>();

            application.ConfigureEndpoints();

            return application;
        } 

        private static WebApplication ConfigureEndpoints(this WebApplication application)
        {
            application
                .MapDailyLogEndpoints()
                .MapProductEndpoints();

            return application;
        }
    }
}
