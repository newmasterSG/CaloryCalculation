using CaloryCalculatiom.Domain.Entities;
using CaloryCalculation.API.Handlers;
using CaloryCalculation.Application.Interfaces;
using CaloryCalculation.Application.Services;
using CaloryCalculation.Db;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CaloryCalculation.Application.Options;

namespace CaloryCalculation.API.Configurations
{
    public static class ConfigurationBuilder
    {
        public static IServiceCollection AddServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddAuth(configuration);
            services.AddAuthorization();
            
            services.AddDb(configuration);

            services.AddOptions(configuration);

            services.AddCustomService();

            services.AddMediator();

            services.AddIdentity(configuration);

            services.AddSwagger();

            services.AddCors(options =>
            {
                var frontUrl = configuration.GetValue<string>("Cors:FrontUrl");

                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.WithOrigins(frontUrl)
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
            
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
            this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddIdentity<User,  IdentityRole<int>>()
                .AddSignInManager<SignInManager<User>>()
                .AddTokenProvider(configuration.GetSection(TokenProvider.Key).Get<TokenProvider>().Name, typeof(DataProtectorTokenProvider<User>))
                .AddEntityFrameworkStores<CaloryCalculationDbContext>().AddDefaultTokenProviders();

            return services;
        }

        private static IServiceCollection AddCustomService(this IServiceCollection services)
        {
            services.AddScoped<IDailyLogService, DailyLogService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<INutrionService, NutrionService>();
            
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

        private static IServiceCollection AddOptions(this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.Location));
            services.Configure<TokenProvider>(configuration.GetSection(TokenProvider.Key));

            return services;
        }

        private static IServiceCollection AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
                    services.AddAuthentication(options =>
                    {
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                    .AddJwtBearer(options =>
                    {
                        var jwtSettings = configuration.GetSection(JwtSettings.Location).Get<JwtSettings>();
                        var key = Encoding.ASCII.GetBytes(jwtSettings.Key);
            
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = jwtSettings.Issuer,
                            ValidAudience = jwtSettings.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(key)
                        };
                    });
            
            return services;
        }
    }
}
