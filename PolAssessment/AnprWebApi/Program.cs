using PolAssessment.AnprWebApi.DbContexts;
using PolAssessment.AnprWebApi.Extensions;
using PolAssessment.AnprWebApi.Services;
using PolAssessment.Shared.Configuration;
using PolAssessment.Shared.Extensions;
using PolAssessment.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile("appsettingsSecrets.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

builder.Logging
    .ClearProviders()
    .AddConfiguration(configuration.GetSection("Logging"))
    .AddConsole()
    .AddDebug();

builder.Services
    .AddCors(x =>
        x.AddPolicy("AllowSpecificOrigins", y => y
            .WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()!)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials())
    )
    .AddEndpointsApiExplorer()
    .RegisterSwagger("v1", "ANPR Web API")

    .RegisterDbContext<IAnprDataDbContext, AnprDataDbContext>(configuration.GetSection("Database:Anpr").Get<DatabaseConfig>()!)
    .RegisterDbContext<IWebApiDbContext, WebApiDbContext>(configuration.GetSection("Database:WebApi").Get<DatabaseConfig>()!)

    .Configure<JwtConfig>(configuration.GetSection("Jwt"))
    .AddScoped<IAuthTokenHandler, AuthTokenHandler>()
    
    .AddScoped<IAnprQueryService, AnprQueryService>()
    .AddScoped<IHashService, HashService>()
    
    .RegisterJwt(configuration.GetSection("Jwt").Get<JwtConfig>()!)

    .AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || builder.Configuration.GetValue<bool>("Swagger:Enabled"))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigins");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
