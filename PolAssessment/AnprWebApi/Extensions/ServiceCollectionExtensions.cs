using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PolAssessment.AnprWebApi.DbContexts;


namespace PolAssessment.AnprWebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterSwagger(this IServiceCollection services)
    {
        return services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo { Title = "ANPR Web API", Version = "v1" });

            // Use Authorization within Swagger Try-Its
            x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            x.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme{
                        Reference = new OpenApiReference{
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    public static IServiceCollection RegisterAnprDbContext(this IServiceCollection services, ConfigurationManager config)
    {
        var connectionString = config.GetAnprDatabaseConnectionString();
        return services
            .AddDbContext<AnprDataDbContext>(x => x.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)))
            .AddScoped<IAnprDataDbContext>(provider => provider.GetService<AnprDataDbContext>() ?? throw new InvalidOperationException());
    }

    public static IServiceCollection RegisterWebApiDbContext(this IServiceCollection services, ConfigurationManager config)
    {
        var connectionString = config.GetWebApiDatabaseConnectionString();
        return services
            .AddDbContext<WebApiDbContext>(x => x.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)))
            .AddScoped<IWebApiDbContext>(provider => provider.GetService<WebApiDbContext>() ?? throw new InvalidOperationException());
    }

    public static IServiceCollection RegisterJwt(this IServiceCollection services, ConfigurationManager config)
    {
        return services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config.GetJwtIssuer(),
                ValidAudience = config.GetJwtAudience(),
                IssuerSigningKey = new SymmetricSecurityKey(config.GetJwtKey()),
            };
        }).Services;
    }
}
