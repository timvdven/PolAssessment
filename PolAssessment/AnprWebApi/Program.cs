using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using PolAssessment.AnprWebApi.DbContexts;

var builder = WebApplication.CreateBuilder(args);

builder.Logging
    .ClearProviders()
    .AddConfiguration(builder.Configuration.GetSection("Logging"))
    .AddConsole()
    .AddDebug();

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(x =>
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
    })

    .AddDbContext<AnprDbContext>(x => 
        x.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))))

    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AnprDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || builder.Configuration.GetValue<bool>("Swagger:Enabled", defaultValue: false))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
