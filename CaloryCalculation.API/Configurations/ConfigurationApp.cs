using CaloryCalculatiom.Domain.Entities;
using CaloryCalculation.API.Endpoints;

namespace CaloryCalculation.API.Configurations
{
    public static class ConfigurationApp
    {
        public static WebApplication Configure(this WebApplication application)
        {
            application.UseCors("AllowSpecificOrigin");

            // Configure the HTTP request pipeline.

            application.UseHttpsRedirection();

            application.UseAuthentication();
            application.UseAuthorization();

            application.UseSwagger();
            application.UseSwaggerUI();

            application.ConfigureEndpoints();

            return application;
        } 

        private static WebApplication ConfigureEndpoints(this WebApplication application)
        {
            application
                .MapDailyLogEndpoints()
                .MapAuthEndpoints()
                .MapCalculationNutrion()
                .MapUserEndpoints()
                .MapProductEndpoints();

            return application;
        }
    }
}
