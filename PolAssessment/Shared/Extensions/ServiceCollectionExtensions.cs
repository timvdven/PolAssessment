using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PolAssessment.Shared.Configuration;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace PolAssessment.Shared.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterSwagger(this IServiceCollection services, string version, string title)
    {
        return services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc(version, new OpenApiInfo { Title = title, Version = version });

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

    public static IServiceCollection RegisterJwt(this IServiceCollection services, JwtConfig config)
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
                ValidIssuer = config.Issuer,
                ValidAudience = config.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Key)),
            };
        }).Services;
    }

    public static IServiceCollection RegisterDbContext<T1, T2>(this IServiceCollection services, DatabaseConfig config)
        where T1 : class
        where T2 : DbContext, T1   
    {
        var connectionString = config.ConnectionString;
        return services
            .AddDbContext<T2>(x => x.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)))
            .AddScoped<T1>(provider => provider.GetService<T2>() ?? throw new InvalidOperationException());
    }
}
